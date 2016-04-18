using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.FactoryInterface;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using OrderInfo = YeahTVApi.DomainModel.Models.ViewModels.OrderInfo;

namespace YeahTVApi.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IConstantSystemConfigManager constantSystemConfigManager;
        private readonly IMovieManager moviemanager;
        private readonly IMovieTemplateRelationManager movieTemplateRelationManager;
        private readonly IVODOrderManager vODOrderManager;
        private IDeviceTraceLibraryManager deviceTraceLibraryManager;
        private IHotelMovieTraceManager hotelMovieTraceManager;
        private IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceNoTemplateWrapperFacade;
        private IRequestApiService requestApiService;
        private ITVHotelConfigManager tvHotelConfigManager;
        private IStoreOrderManager storeOrderManager;
        private IGlobalConfigManager globalConfigManager;
        private IHotelTypeFactory hotelTypeFactory;

        public PaymentController(IDeviceTraceLibraryManager deviceTraceLibraryManager,
            IVODOrderManager vODOrderManager,
            IMovieTemplateRelationManager movieTemplateRelationManager,
            IMovieManager moviemanager,
            IConstantSystemConfigManager constantSystemConfigManager,
            IHotelMovieTraceManager hotelMovieTraceManager,
            IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceNoTemplateWrapperFacade,
            IRequestApiService requestApiService,
            ITVHotelConfigManager tvHotelConfigManager,
            IStoreOrderManager storeOrderManager,
            IGlobalConfigManager globalConfigManager,
            IHotelTypeFactory hotelTypeFactory)
        {
            this.deviceTraceLibraryManager = deviceTraceLibraryManager;
            this.vODOrderManager = vODOrderManager;
            this.movieTemplateRelationManager = movieTemplateRelationManager;
            this.moviemanager = moviemanager;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.hotelMovieTraceManager = hotelMovieTraceManager;
            this.hotelMovieTraceNoTemplateWrapperFacade = hotelMovieTraceNoTemplateWrapperFacade;
            this.requestApiService = requestApiService;
            this.tvHotelConfigManager = tvHotelConfigManager;
            this.storeOrderManager = storeOrderManager;
            this.globalConfigManager = globalConfigManager;
            this.hotelTypeFactory = hotelTypeFactory;
        }

        #region 通用支付接口

        [HTApiFilter]
        public PaymentApiResult GetPaymentQRCode(TvPayRequest tvPayRequest)
        {
            var result = new PaymentApiResult();

            if (tvPayRequest == null || string.IsNullOrWhiteSpace(Header.HotelID) || (string.IsNullOrWhiteSpace(tvPayRequest.OrderId))
                || string.IsNullOrWhiteSpace(Header.DEVNO) || string.IsNullOrWhiteSpace(tvPayRequest.PayPaymentModel))
                return result.WithError("参数错误！", tvPayRequest.OrderId);

            var order = storeOrderManager.SearchStoreOrder(new StoreOrderCriteria() { Hotelid = Header.HotelID, Orderid = tvPayRequest.OrderId }).FirstOrDefault();
            if (order == null)
                return result.WithError("订单不存在！", tvPayRequest.OrderId);

            if (order.Status == (int)OrderState.Success)
                return result.WithOk("", tvPayRequest.OrderId, (int)OrderResultType.Success);

            if (order.GetTransactionstate() == Transactionstate.Cancel)
                return result.WithOk("订单已失效！", tvPayRequest.OrderId, (int)OrderResultType.Error);

            if (order.IsDelete)
                return result.WithOk("订单已删除！", tvPayRequest.OrderId, (int)OrderResultType.Error);

            if (!Enum.GetNames(typeof(PayPaymentModel)).Any(m => m.ToLower().Equals(tvPayRequest.PayPaymentModel.ToLower())))
                return result.WithError("支付方式错误！", order.Id);

            #region 支付配置检查

            var hotelPaymentInfo = tvHotelConfigManager.GetHotelConfig(new HotelConfigCriteria() { HotelId = Header.HotelID, ConfigCodes = "StorePaymentModel" });

            if (hotelPaymentInfo == null)
                return result.WithError("未找到支付配置信息！", order.Id);

            if (!hotelPaymentInfo.Active.HasValue || !hotelPaymentInfo.Active.Value)
                return result.WithError("支付配置项未启用！", order.Id);

            StorePayConfig hotelPayment;

            try
            {
                hotelPayment = JsonConvert.DeserializeObject<StorePayConfig>(hotelPaymentInfo.ConfigValue);
            }
            catch (Exception ex)
            {
                return result.WithError("读取支付配置信息错误！", order.Id);
            }

            if (hotelPayment == null)
                return result.WithError("解析支付配置信息错误！", order.Id);


            if (!hotelPayment.PaymentModels.Any(m => m.ToLower().Equals(tvPayRequest.PayPaymentModel.ToLower())))
                return result.WithError("酒店不支持此种支付方式！", order.Id);

            #endregion


            #region 如果非首次拉取验证码则创建订单

            if (!string.IsNullOrWhiteSpace(order.PayInfo))
            {
                order.IsDelete = true;
                storeOrderManager.Update(order);

                var newOrder = new StoreOrder();
                order.CopyTo(newOrder, new string[] { "Id" });

                var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + Header.HotelID;
                //var requestHotelUrl = "http://localhost:8099/" + Constant.GetHotelApiUrl + Header.HotelID;
                var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();

                newOrder.Id = storeOrderManager.GetNewOrderId(hotel.HotelCode);
                newOrder.IsDelete = false;
                newOrder.OrderProducts = order.OrderProducts.Select(m => new OrderProducts()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    CreateTime = DateTime.Now,
                    OrderId = newOrder.Id,
                    ProductId = m.ProductId,
                    ProductInfo = m.ProductInfo,
                    ProductName = m.ProductName,
                    Quantity = m.Quantity,
                    UnitPrice = m.UnitPrice
                }).ToList();
                storeOrderManager.Add(newOrder);

                order = newOrder;
            }

            #endregion

            order.PayInfo = tvPayRequest.PayPaymentModel;

            if (order.PayInfo.ToLower().Equals(PayPaymentModel.FZPAY.ToString().ToLower()) || order.PayInfo.ToLower().Equals(PayPaymentModel.QTPAY.ToString().ToLower()))
            {
                order.Status = (int)OrderState.Paying;
            }

            storeOrderManager.Update(order);

            if (order.PayInfo.ToLower().Equals(PayPaymentModel.QTPAY.ToString().ToLower()))
            {
                var url = string.Format("{0}{1}?orderId={2}", constantSystemConfigManager.AppCenterUrl, Constant.GetQtPayCallBackUrl, order.Id);
                var responseData = "";
                var tryCount = 0;
                do
                {
                    tryCount++;
                    responseData = requestApiService.HttpRequest(url, "Get");
                } while (responseData.ToLower() != "success" && tryCount <= 3);

                return result.WithOk("", order.Id, (int)OrderResultType.Unpaid);
            }
            else
            {
                var paymentResult = RequestPayment(order);

                if (!order.PayInfo.ToLower().Equals(PayPaymentModel.FZPAY.ToString().ToLower()))
                {
                    order.Status =
                        (int)
                            ((paymentResult.ResultCode == PayMentOrderState.Success.GetValueStr())
                                ? OrderState.Paying
                                : OrderState.Fail);
                    order.CompleteTime = DateTime.Now;
                    storeOrderManager.Update(order);
                }

                return paymentResult.ResultCode == PayMentOrderState.Success.GetValueStr()
                    ? result.WithOk(paymentResult.Data.ReturnInfo.QrcodeUrl, order.Id, (int)OrderResultType.Unpaid)
                    : result.WithError(paymentResult.Message, order.Id, (int)OrderResultType.Error);
            }
        }

        [HTApiFilter]
        public ApiObjectResult<StorePaymentModel> GetHotelPaymentModels()
        {
            var result = new ApiObjectResult<StorePaymentModel>();
            var configs = new StorePaymentModel();

            var paymentModels = tvHotelConfigManager.GetHotelConfig(new HotelConfigCriteria() { HotelId = Header.HotelID, ConfigCodes = "StorePaymentModel" });
            if (paymentModels != null)
            {
                var models = JsonConvert.DeserializeObject<StorePayConfig>(paymentModels.ConfigValue);
                configs.PayPaymentModel = models.PaymentModels;
            }
            result.obj = configs;

            return result;
        }

        /// <summary>
        /// 交易状态查询 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>
        /// ResultType：0(已支付)  Message：无
        /// ResultType：1(未支付)  Message：无
        /// ResultType：-1(错误)    Message：异常信息
        /// </returns>
        [HTApiFilter(ShouldNotBindDevice = false)]
        public ApiResult PayState(string orderId)
        {
            var result = new ApiResult();

            var order = storeOrderManager.FindByKey(orderId);
            if (order == null)
                return result.WithError("订单不存在！");

            if (Header.HotelID != order.Hotelid)
                return result.WithError("操作失败，只能查询本酒店订单！");

            switch (order.Status)
            {
                case (int)OrderState.Success:
                    result = result.WithOk();
                    break;
                case (int)OrderState.Fail:
                    result = result.WithError("支付失败！", (int)OrderResultType.Error);
                    break;
                default:
                    result = result.WithError("未支付！", (int)OrderResultType.Unpaid);
                    break;
            }

            return result;
        }

        private PaymentResponseInfo RequestPayment(StoreOrder order)
        {
            var paymentrequestdata = new Paymentrequestdata
            {
                orderInfo = new OrderInfo
                {
                    goodsId = order.Id,
                    bizHotle = order.Hotelid,
                    bizRoom = order.RoomNo,
                    memo = "",
                    orderAmount = order.Price.ToString(),
                    bizMember = "",
                    notifyUrl = constantSystemConfigManager.PaymentNotifyUrl,
                    bizDevice = order.SeriseCode,
                    goodsDesc = order.GoodsDesc,
                    goodsName = order.GoodsName,
                    orderId = order.Id
                }
            };
            paymentrequestdata.orderInfo.payInfo.Add(order.PayInfo, order.Price.ToString());

            return GetQRCodeUrl(paymentrequestdata);
        }

        #endregion

        #region 1.8版 支持包天 和 单部电影点播

        [HTApiFilter]
        public VodPaymentApiResult VodRequestNew(TVVODRequest tvVODRequest)
        {
            var result = new VodPaymentApiResult();

            if (tvVODRequest == null || string.IsNullOrWhiteSpace(Header.HotelID) || (string.IsNullOrWhiteSpace(tvVODRequest.PayType)) || string.IsNullOrWhiteSpace(Header.DEVNO))
                return result.WithError("参数错误！");

            if (!Enum.GetNames(typeof(PayPaymentModel)).Any(m => m.ToLower().Equals(tvVODRequest.PayPaymentModel.ToLower())))
                return result.WithError("支付方式错误！");

            if (!Enum.GetNames(typeof(PayType)).Any(m => m.ToLower().Equals(tvVODRequest.PayType.ToLower())))
                return result.WithError("支付类型错误！");

            if (CheckAuthority(tvVODRequest.movieId))
                return result.WithOk();

            HotelMovieTraceNoTemplate movieInfo = null;
            HotelPayment hotelPayment = null;

            if (tvVODRequest.PayType.Trim().ToLower() == PayType.Movie.ToString().ToLower())
            {
                if (string.IsNullOrWhiteSpace(tvVODRequest.movieId))
                    return result.WithError("电影ID不能为空！");

                //检测该酒店是否能点播此电影
                //movieInfo = hotelMovieTraceNoTemplateWrapperFacade
                //    .GetAllFromCache().FirstOrDefault(m => m.HotelId.Equals(Header.HotelID) && m.MovieId.Equals(tvVODRequest.movieId));
                movieInfo = hotelMovieTraceNoTemplateWrapperFacade
                    .SearchHotelMovieTraceNoTemplates(new HotelMovieTraceNoTemplateCriteria()
                    {
                        HotelId = Header.HotelID,
                        MovieId = tvVODRequest.movieId
                    }).FirstOrDefault();

                if (movieInfo == null)
                    return result.WithError("电影未授权！");

                if (!movieInfo.Active)
                    return result.WithError("电影已下架！");

                if (string.IsNullOrWhiteSpace(movieInfo.DownloadStatus) || movieInfo.DownloadStatus.ToLower() != DownloadStatus.Success.ToString().ToLower())
                    return result.WithError("电影同步状态异常！");
            }

            var hotelPaymentInfo = tvHotelConfigManager.SearchFromCache(new HotelConfigCriteria() { })
                .FirstOrDefault(
                    m => m.ConfigCode.ToLower().Equals("HotelPayment".ToLower()) && m.HotelId.Equals(Header.HotelID));

            if (hotelPaymentInfo == null)
                return result.WithError("未找到支付配置信息！");

            var configs = new string[] { "\"PriceOfDay\"", "\"PaymentModels\"", "\"PayType\"" };

            if (!configs.All(c => hotelPaymentInfo.ConfigValue.ToLower().Contains(c.ToLower())))
                return result.WithError("支付配置信息异常！");

            try
            {
                hotelPayment = JsonConvert.DeserializeObject<HotelPayment>(hotelPaymentInfo.ConfigValue);
            }
            catch (Exception ex)
            {
                return result.WithError("读取支付配置信息错误！");
            }

            if (hotelPayment == null)
                return result.WithError("解析支付配置信息错误！");

            if (!string.IsNullOrWhiteSpace(hotelPayment.PayType) && !Enum.GetNames(typeof(PayType)).Any(m => hotelPayment.PayType.ToLower().Equals(m.ToLower())))
                return result.WithError("支付类型配置错误！");

            if (!hotelPayment.PaymentModels.Any(m => m.ToLower().Equals(tvVODRequest.PayPaymentModel.ToLower())))
                return result.WithError("酒店不支持此种支付方式！");

            if (tvVODRequest.PayType.Trim().ToLower() == PayType.Daily.ToString().ToLower())
            {
                if (string.IsNullOrWhiteSpace(hotelPayment.PayType) || !hotelPayment.PayType.ToLower().Equals(PayType.Daily.ToString().ToLower()))
                    return result.WithError("该酒店不支持包天点播！");
            }


            var newOrder = vODOrderManager.CreateNewOrder(tvVODRequest, movieInfo, hotelPayment, Header);
            if (newOrder == null)
                return result.WithError("创建订单失败！");

            var paymentResult = RequestQRCode(newOrder);

            if (!newOrder.PayInfo.ToLower().Equals(PayPaymentModel.FZPAY.ToString().ToLower()))
            {
                newOrder.State =
                    (int)
                        ((paymentResult.ResultCode == PayMentOrderState.Success.GetValueStr())
                            ? OrderState.Paying
                            : OrderState.Fail);
                newOrder.CompleteTime = DateTime.Now;
                vODOrderManager.UpdateVODOrder(newOrder);
            }

            var orderPrice = newOrder.Price.ToString();
            if (newOrder.PayInfo.ToLower().Equals(PayPaymentModel.JFPAY.ToString().ToLower()))
            {
                orderPrice = vODOrderManager.GetIntScoresbyOderId(newOrder.Id).ToString();
            }

            return paymentResult.ResultCode == PayMentOrderState.Success.GetValueStr()
                ? result.WithOk(paymentResult.Data.ReturnInfo.QrcodeUrl, orderPrice, (int)OrderResultType.Unpaid)
                : result.WithError(paymentResult.Message, (int)OrderResultType.Error);
        }

        private PaymentResponseInfo RequestQRCode(VODOrder order)
        {
            var paymentResult = new PaymentResponseInfo();

            if (String.Equals(order.PayInfo, PayPaymentModel.JFPAY.ToString(),
          StringComparison.CurrentCultureIgnoreCase))
            {
                var hotelBusinessCode = globalConfigManager.GetHotelScoreBusinessCode(order.HotelId);
                if (string.IsNullOrWhiteSpace(hotelBusinessCode))
                    return new PaymentResponseInfo()
                    {
                        Message = "获取品牌业务代码错误!",
                        ResultCode = PayMentOrderState.Fail.GetValueStr()
                    };

                var hotelMemberInfoManager = hotelTypeFactory.MakeHotelType(hotelBusinessCode);
                if (hotelMemberInfoManager == null)
                    return new PaymentResponseInfo()
                    {
                        Message = "不支持当前业务!",
                        ResultCode = PayMentOrderState.Fail.GetValueStr()
                    };

                try
                {
                    paymentResult = hotelMemberInfoManager.GetScoreQRCode(order.Id, order.HotelId);
                }
                catch (Exception ex)
                {
                    return new PaymentResponseInfo()
                    {
                        Message = "获取积分二维码异常：" + ex.Message,
                        ResultCode = PayMentOrderState.Fail.GetValueStr()
                    };
                }
            }
            else
            {
                paymentResult = RequestPayment(order);

            }

            return paymentResult;
        }


        /// <summary>
        /// Vod交易状态查询 电影ID 可以为空 同时检查包天情况
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns>
        /// ResultType：0(已支付)  Message：无
        /// ResultType：1(未支付)  Message：无
        /// ResultType：-1(错误)    Message：异常信息
        /// </returns>
        [HTApiFilter(ShouldNotBindDevice = false)]
        public ApiResult VodPayStateNew(string movieId)
        {
            var result = new ApiResult();

            if (!string.IsNullOrWhiteSpace(Header.HotelID) && !string.IsNullOrWhiteSpace(Header.DEVNO))
            {
                return CheckAuthority(movieId) ? result.WithOk() : result.WithError("未支付！", (int)OrderResultType.Unpaid);
            }

            return result.WithError("参数错误！");
        }

        /// <summary>
        /// 获取点播有效时间及类型
        /// </summary>
        /// <returns></returns>
        [HTApiFilter]
        public ApiObjectResult<PayTypeState> QueryVodState()
        {
            var result = new ApiObjectResult<PayTypeState>();
            var payTypeState = new PayTypeState();


            if (!string.IsNullOrWhiteSpace(Header.HotelID) && !string.IsNullOrWhiteSpace(Header.DEVNO))
            {
                var vodorder = vODOrderManager.GetSuccessDailyOrder(Header);

                if (vodorder == null || vodorder.CompleteTime == null) return result.WithError("未点播！", (int)OrderResultType.Unpaid);

                payTypeState.BeginTime = vodorder.CompleteTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                payTypeState.EndTime = vodorder.CompleteTime.Value.AddHours(Math.Abs(constantSystemConfigManager.VodDailyOrderExpires)).ToString("yyyy-MM-dd HH:mm:ss");
                payTypeState.PayType = vodorder.PayType;

                return result.WithOk(payTypeState);
            }

            return result.WithError("参数错误！");
        }

        private bool CheckAuthority(string movieId)
        {
            var vodorder = vODOrderManager.GetSuccessDailyOrder(Header);

            if (vodorder == null && !string.IsNullOrWhiteSpace(movieId))
            {
                vodorder = vODOrderManager.GetSuccessOrderByMovieId(Header, movieId);
            }

            return vodorder != null;
        }


        public string GetHotelNameByHotelId(string hotelId)
        {
            var hoteLUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + hotelId;
            var hotelEf = requestApiService.HttpRequest(hoteLUrl, "GET").JsonStringToObj<Hotel>();
            return hotelEf == null ? "" : hotelEf.hotelName;
        }

        #endregion

        #region 1.2版本

        /// <summary>
        /// Vod申请点播
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HTApiFilter]
        public ApiResult VodRequest(string movieId)
        {
            var result = new ApiResult();

            if (string.IsNullOrWhiteSpace(Header.HotelID) || string.IsNullOrWhiteSpace(movieId) ||
                string.IsNullOrWhiteSpace(Header.DEVNO)) return result.WithError("参数错误！");

            //1.通过交易完成时间、电影ID、设备号、房间号、酒店ID，订单状态(支付完成)来确定订单（缓存）
            var vodorder = vODOrderManager.GetSuccessOrderByMovieId(Header, movieId);

            //2.如果存在已支付的订单则，直接返回安卓为已支付
            if (vodorder != null) return result.WithOk();

            //3.如果订单不存在，则创建订单            
            var moviehoteltrace = hotelMovieTraceManager.GetAllFromCache().FirstOrDefault(m => m.MovieId.Equals(movieId) && m.HotelId.Equals(Header.HotelID));
            if (moviehoteltrace == null) return result.WithError("电影未授权！");

            var newOrder = CreateOrder(movieId, moviehoteltrace.MoiveTemplateId);
            if (newOrder == null) return result.WithError("创建订单失败！");

            //4.根据订单信息，请求支付网关。若返回二维码成功或失败，更新订单状态
            var paymentResult = RequestPayment(newOrder);

            newOrder.State = (int)((paymentResult.ResultCode == PayMentOrderState.Success.GetValueStr()) ? OrderState.Paying : OrderState.Fail);
            newOrder.CompleteTime = DateTime.Now;
            vODOrderManager.UpdateVODOrder(newOrder);


            //5.若支付网关成功生成二维码接口，则返回二维码至安卓端，否则返回生成支付二维码失败至安卓端
            return paymentResult.ResultCode == PayMentOrderState.Success.GetValueStr()
                ? result.WithOk(paymentResult.Data.ReturnInfo.QrcodeUrl, (int)OrderResultType.Unpaid)
                : result.WithError(paymentResult.Message, (int)OrderResultType.Error);
        }

        /// <summary>
        /// 根据电影ID创建订单
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="movieTemplateId"></param>
        /// <returns></returns>
        private VODOrder CreateOrder(string movieId, string movieTemplateId)
        {
            VODOrder order = null;
            var movieTemplateRelation = movieTemplateRelationManager.GetAllFromCache().FirstOrDefault(m => m.MovieId.Equals(movieId) && m.MovieTemplateId.Equals(movieTemplateId));
            var movie = moviemanager.FindByKey(movieId);

            if (movie != null && movieTemplateRelation != null && movieTemplateRelation.Price != null)
            {
                //从安卓端获取参数信息并添加订单
                order = new VODOrder
                {
                    CreateTime = DateTime.Now,
                    Price = (decimal)movieTemplateRelation.Price,
                    State = (int)OrderState.Unpaid,
                    CompleteTime = DateTime.Now,
                    MovieId = movieId,
                    SeriseCode = Header.DEVNO,
                    RoomNo = Header.RoomNo,
                    HotelId = Header.HotelID,
                    GoodsName = movie.Name.Replace("&", "").Replace("%", "").Replace("+", ""),
                    GoodsDesc = "",
                    PayInfo = constantSystemConfigManager.VodDefaultPayInfo,
                    IsDelete = false
                };
                vODOrderManager.Add(order);
            }
            return order;
        }

        /// <summary>
        /// 请求支付网关获取二维码
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private PaymentResponseInfo RequestPayment(VODOrder order)
        {
            var paymentrequestdata = new Paymentrequestdata
            {
                orderInfo = new OrderInfo
                {
                    goodsId = order.Id,
                    bizHotle = order.HotelId,
                    bizRoom = order.RoomNo,
                    memo = "",
                    orderAmount = order.Price.ToString(),
                    bizMember = "",
                    notifyUrl = constantSystemConfigManager.VodPaymentNotifyUrl,
                    bizDevice = order.SeriseCode,
                    goodsDesc = order.GoodsDesc,
                    goodsName = order.GoodsName,
                    orderId = order.Id
                }
            };
            paymentrequestdata.orderInfo.payInfo.Add(order.PayInfo, order.Price.ToString());

            return GetQRCodeUrl(paymentrequestdata);
        }

        private PaymentResponseInfo GetQRCodeUrl(Paymentrequestdata paymentrequestdata)
        {
            PaymentResponseInfo result = null;

            try
            {
                var postData = paymentrequestdata.ToJsonString();

                var sign =
                    new StringBuilder().Append(postData)
                        .Append(constantSystemConfigManager.VodPaymentSignKey)
                        .ToString()
                        .StringToMd5();

                var requestData = string.Format("pid={0}&sign={1}&data={2}", constantSystemConfigManager.VodPaymentPid,
                    sign, postData);

                var paymentResult = (new HttpHelper()).Get(constantSystemConfigManager.VodPaymentRequestUrl, "", "", "",
                    requestData, "Post");

                if (!string.IsNullOrWhiteSpace(paymentResult))
                    result = JsonConvert.DeserializeObject<PaymentResponseInfo>(paymentResult);

                if (result == null || result.Data == null || result.Data.ReturnInfo == null)
                {
                    result = new PaymentResponseInfo
                    {
                        ResultCode = PayMentOrderState.Fail.GetValueStr(),
                        Message = "获取二维码失败！"
                    };
                }
                else if (!string.IsNullOrWhiteSpace(result.Data.ReturnInfo.QrcodeUrl))
                {
                    result.Data.ReturnInfo.QrcodeUrl = Regex.Unescape(result.Data.ReturnInfo.QrcodeUrl);
                }
            }
            catch (Exception ex)
            {
                result = new PaymentResponseInfo
                {
                    ResultCode = PayMentOrderState.Fail.GetValueStr(),
                    Message = ex.Message
                };
            }

            return result;
        }

        /// <summary>
        /// Vod交易状态查询
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns>
        /// ResultType：0(已支付)  Message：无
        /// ResultType：1(未支付)  Message：无
        /// ResultType：-1(错误)    Message：异常信息
        /// </returns>
        [HTApiFilter(ShouldNotBindDevice = false)]
        public ApiResult VodPayState(string movieId)
        {
            var result = new ApiResult();

            if (!string.IsNullOrWhiteSpace(Header.HotelID) && !string.IsNullOrWhiteSpace(movieId) &&
                !string.IsNullOrWhiteSpace(Header.DEVNO))
            {
                return vODOrderManager.GetSuccessOrderByMovieId(Header, movieId) == null
                    ? result.WithError("未支付！", (int)OrderResultType.Unpaid)
                    : result.WithOk();
            }
            return result.WithError("参数错误！");
        }
        #endregion
    }
}