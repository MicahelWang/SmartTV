using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;

using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Filter;
using YeahTVApi.DomainModel.Models;
using YeahTvHcsApi.ViewModels;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;

namespace YeahTvHcsApi.Controllers
{
    public class PaymentController : ApiController
    {
        private readonly ILogManager _logManager;
        private readonly IVODOrderManager _vodOrderManager;
        private readonly IHotelMovieTraceNoTemplateWrapperFacade _hotelMovieTraceNoTemplateWrapperFacade;
        private readonly IDeviceTraceLibraryManager _deviceTraceManager;
        private readonly ITVHotelConfigManager _tvHotelConfigManager;

        public PaymentController(ILogManager logManager,
            IVODOrderManager vodOrderManager
            , IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceNoTemplateWrapperFacade
            , IDeviceTraceLibraryManager deviceTraceManager
            , ITVHotelConfigManager tvHotelConfigManager)
        {
            _logManager = logManager;
            _vodOrderManager = vodOrderManager;
            _hotelMovieTraceNoTemplateWrapperFacade = hotelMovieTraceNoTemplateWrapperFacade;
            _deviceTraceManager = deviceTraceManager;
            _tvHotelConfigManager = tvHotelConfigManager;
        }

        [HttpPost]
        [ActionName("VodPayment")]
        [HCSCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true)]
        public ResponseData<ApiResult> VodPayment(PostParameters<VODPayment> request)
        {
            Func<string,ResponseData<ApiResult>> resultWrrorFun = errmsg =>
            {
                ResponseData<ApiResult> response = new ResponseData<ApiResult>();
                response.Data = new ApiResult().WithError(errmsg);
                return response;
            };

            var result = new ApiResult();

            if (request == null || request.Data == null || (string.IsNullOrWhiteSpace(request.Data.PayType)) || string.IsNullOrWhiteSpace(request.Data.DeviceSeries))
                return resultWrrorFun("参数错误！") ;

            if (!Enum.GetNames(typeof(PayType)).Any(m => m.ToLower().Equals(request.Data.PayType.ToLower())))
                return resultWrrorFun("支付类型错误！");

            var device = _deviceTraceManager.SearchFromCache(new DeviceTraceCriteria { DeviceSeries = request.Data.DeviceSeries }).FirstOrDefault();
            if (device == null)
                return resultWrrorFun("设备未绑定！");

            HotelMovieTraceNoTemplate movieInfo = null;
            HotelPayment hotelPayment = null;

            if (request.Data.PayType.Trim().ToLower() == PayType.Movie.ToString().ToLower())
            {
                if (string.IsNullOrWhiteSpace(request.Data.MovieId))
                    return resultWrrorFun("电影ID不能为空！");


                movieInfo = _hotelMovieTraceNoTemplateWrapperFacade
                    .SearchHotelMovieTraceNoTemplates(new HotelMovieTraceNoTemplateCriteria()
                    {
                        HotelId = device.HotelId,
                        MovieId = request.Data.MovieId
                    }).FirstOrDefault();

                if (movieInfo == null)
                    return resultWrrorFun("电影未授权或同步状态异常！");

                if (!movieInfo.Active)
                    return resultWrrorFun("电影已下架！");

                if (string.IsNullOrWhiteSpace(movieInfo.DownloadStatus) || movieInfo.DownloadStatus.ToLower() != DownloadStatus.Success.ToString().ToLower())
                    return resultWrrorFun("电影同步状态异常！");
            }

            var hotelPaymentInfo = _tvHotelConfigManager.SearchFromCache(new HotelConfigCriteria() { })
                .FirstOrDefault(
                    m => m.ConfigCode.ToLower().Equals("HotelPayment".ToLower()) && m.HotelId.Equals(device.HotelId));

            if (hotelPaymentInfo == null)
                return resultWrrorFun("未找到支付配置信息！");

            var configs = new string[] { "\"PriceOfDay\"", "\"PaymentModels\"", "\"PayType\"" };

            if (!configs.All(c => hotelPaymentInfo.ConfigValue.ToLower().Contains(c.ToLower())))
                return resultWrrorFun("支付配置信息异常！");

            try
            {
                hotelPayment = JsonConvert.DeserializeObject<HotelPayment>(hotelPaymentInfo.ConfigValue);
            }
            catch (Exception ex)
            {
                return resultWrrorFun("读取支付配置信息错误！");
            }

            if (hotelPayment == null)
                return resultWrrorFun("解析支付配置信息错误！");

            if (!string.IsNullOrWhiteSpace(hotelPayment.PayType) && !Enum.GetNames(typeof(PayType)).Any(m => hotelPayment.PayType.ToLower().Equals(m.ToLower())))
                return resultWrrorFun("支付类型配置错误！");

            if (!hotelPayment.PaymentModels.Any(m => m.ToLower().Equals(PayPaymentModel.FZPAY.ToString().ToLower())))
                return resultWrrorFun("酒店不支持此种支付方式！");

            var tvVodRequest = new TVVODRequest()
            {
                movieId = request.Data.MovieId,
                PayPaymentModel = PayPaymentModel.FZPAY.ToString(),
                PayType = request.Data.PayType
            };
            var newOrder = _vodOrderManager.CreateNewOrder(tvVodRequest, movieInfo, hotelPayment, new RequestHeader()
            {
                RoomNo = device.RoomNo,
                DEVNO = request.Data.DeviceSeries,
                HotelID = device.HotelId
            }, OrderState.Success);
            if (newOrder == null)
                return resultWrrorFun("创建订单失败！");

            newOrder.State = (int)OrderState.Success;
            newOrder.CompleteTime = DateTime.Now;
            _vodOrderManager.UpdateVODOrder(newOrder);

            if (string.IsNullOrWhiteSpace(request.Data.PayType) || request.Data.PayType.ToLower().Equals(PayType.Movie.ToString().ToLower()))
            {
                _vodOrderManager.UpdateOrderCache();
            }
            else if (request.Data.PayType.ToLower().Equals(PayType.Daily.ToString().ToLower()))
            {
                _vodOrderManager.UpdateDailyOrderCache();
            }

            return new ResponseData<ApiResult>() { Data =result.WithOk("Success", (int)OrderResultType.Success)};
        }

    }
}