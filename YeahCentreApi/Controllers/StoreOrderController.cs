using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Filter;
using YeahTVApi.DomainModel.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YeahCentreApi.ViewModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.Entity;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.DomainModel.Models.DataModel;


namespace YeahCentreApi.Controllers
{
    [RoutePrefix("api/StoreOrder")]
    public class StoreOrderController : ApiController
    {
        private readonly IConstantSystemConfigManager _constantSystemConfigManager;
        private readonly IStoreOrderManager storeOrderManager;
        public StoreOrderController(IConstantSystemConfigManager constantSystemConfigManager, IStoreOrderManager storeOrderManager)
        {
            _constantSystemConfigManager = constantSystemConfigManager;
            this.storeOrderManager = storeOrderManager;
        }

        [HttpGet]
        [Route("GetPrivateKey")]
        [CenterApiCheckSignFilter(GetPrivateKey = true, NeedCheckSign = false, IsCheckDeviceBind = false)]
        public HttpResponseMessage GetPrivateKey()
        {
            var key = PostParameters<string>.EncryptContent(_constantSystemConfigManager.StoreSignPublicKey, _constantSystemConfigManager.StoreSignPrivateKey);

            return new HttpResponseMessage { Content = new StringContent(key) };
        }

        [HttpPost]
        [Route("DataDictionarySearching")]
        [CenterApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = false)]
        public ResponseData<List<Datadictionary>> DataDictionarySearching(PostParameters<DictRequest> request)
        {
            var result = new List<EnumItem>();
            if (request.Data.DataType.ToLower().Trim() != "PayPaymentModel".ToString().Trim().ToLower() && request.Data.DataType.ToLower().Trim() != "Orderstatus".ToString().Trim().ToLower())
                throw new Exception("参数错误！");
            if (request.Data.DataType.ToLower().Trim() == "PayPaymentModel".ToString().Trim().ToLower())
            {
                result = EnumExtensions.GetItems(typeof(PayPaymentModel)).Where(m => m.Value != (int)PayPaymentModel.FZPAY).ToList();
            }
            else if (request.Data.DataType.ToLower().Trim() == "Orderstatus".ToString().Trim().ToLower())
            {
                result = EnumExtensions.GetItems(typeof(Transactionstate));
            }
            var datadictionary = result.Select(m => new Datadictionary
            {
                Code = m.Text,
                Text = m.Description,
            }).ToList();

            return new ResponseData<List<Datadictionary>> { Data = datadictionary };
        }

        [HttpPost]
        [Route("OrderStateChanging")]
        [CenterApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = false)]
        public ResponseData<BackData> OrderStateChanging(PostParameters<StoreOrderState> request)
        {
            Func<int, string, ResponseData<BackData>> resulFun = (resultcode, message) =>
            {

                ResponseData<BackData> response = new ResponseData<BackData>();
                var backdata = new BackData();
                backdata.ResultCode = resultcode;
                backdata.Message = message;
                response.Data = backdata;
                return response;
            };
            var data = new BackData();

            if (request == null || request.Data == null || (string.IsNullOrWhiteSpace(request.Data.HotelId)) || (string.IsNullOrWhiteSpace(request.Data.OrderId)) || !(Enum.GetNames(typeof(Transactionstate)).Any(m => m.ToUpper().Trim().Equals(request.Data.Status.ToUpper().Trim()))) ||
                (string.IsNullOrWhiteSpace(request.Data.Status.ToString())))
                return resulFun((int)BackState.PostDataError, "参数错误!");

            var storeorder = storeOrderManager.GetStoreOrder(new StoreOrderCriteria { Hotelid = request.Data.HotelId, Orderid = request.Data.OrderId });
            if (storeorder == null)
                return resulFun((int)BackState.NotFind, "订单不存在!");
            else
            {
                if (request.Data.Status.ToUpper().Trim() == Transactionstate.Transactionscomplete.GetText().ToUpper().Trim())
                {
                    storeorder.Status = (int)OrderState.Success;
                    storeorder.DeliveryState = (int)DeliveryState.Delivery;
                    storeorder.CompleteTime = DateTime.Now;
                }
                if (request.Data.Status.ToUpper().Trim() == Transactionstate.Cancel.GetText().ToUpper().Trim())
                {
                    storeorder.Status = (int)OrderState.Cancel;
                    storeorder.CompleteTime = DateTime.Now;
                }

                storeOrderManager.Update(storeorder);
                data.Message = "订单状态变更成功！";
                data.ResultCode = (int)BackState.Success;

            }

            return new ResponseData<BackData> { Data = data };
        }

        [HttpPost]
        [Route("OrderSearching")]
        [CenterApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = false)]
        public ResponseData<OrderSearch> OrderSearching(PostParameters<SearchCondition> request)
        {
            if (request == null || request.Data == null || (string.IsNullOrWhiteSpace(request.Data.HotelId)) || (string.IsNullOrWhiteSpace(request.Data.Begindate.ToString())) || (string.IsNullOrWhiteSpace(request.Data.Enddate.ToString())) ||
               (string.IsNullOrWhiteSpace(request.Data.Status)) || !(Enum.GetNames(typeof(Transactionstate)).Any(m => m.ToUpper().Trim().Equals(request.Data.Status.ToUpper().Trim()))) || (string.IsNullOrWhiteSpace(request.Data.Pageindex.ToString())) || (string.IsNullOrWhiteSpace(request.Data.Pagesize.ToString())))
                throw new Exception("参数错误！");

            if (request.Data.Begindate > request.Data.Enddate)
            {
                throw new Exception("开始时间不能大于结束时间！");
            }
            if (request.Data.Pageindex <= 0)
            {
                throw new Exception(string.Format("当前页数是{0},页数必须大于0！", request.Data.Pageindex));
            }

            var search = new StoreOrderCriteria
            {
                Hotelid = request.Data.HotelId,
                Roomnumber = request.Data.Roomnumber,
                Begindate = request.Data.Begindate,
                Enddate = request.Data.Enddate,
                PageSize = request.Data.Pagesize,
                Page = request.Data.Pageindex - 1,
                OrderAsc = false,
                SortFiled = "CreateTime",
                NeedPaging = true,
                IsDelete = false,
                Transactionstate = request.Data.Status.ParseAsEnum<Transactionstate>()
            };


            var storeList = storeOrderManager.SearchStoreOrder(search);

            storeList.ForEach(m =>
            {
                m.TransactionState = m.GetTransactionstate().ToString();
                m.DeliveryType = string.IsNullOrWhiteSpace(m.DeliveryType) ? "" : m.DeliveryType.ParseAsEnum<DeliveryType>().GetDescription();
            });

            return new ResponseData<OrderSearch> { Data = new OrderSearch { PageTotal = search.TotalPages, Pageindex = search.Page + 1, Pagesize = search.PageSize, Storeorders = storeList } };
        }
    }
}