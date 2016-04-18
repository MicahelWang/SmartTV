using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using YeahCentreApi.ViewModels;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using System.Linq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahCentreApi.Controllers
{
    [RoutePrefix("api/PayMent")]
    public class PayMentController : ApiController
    {
        private readonly IConstantSystemConfigManager constantSystemConfigManager;
        private readonly IVODOrderManager vODOrderManager;
        private readonly IStoreOrderManager storeOrderManager;
        public readonly ILogManager logManager;

        public PayMentController(IVODOrderManager vOdOrderManager, IStoreOrderManager storeOrderManager, ILogManager logManager, IConstantSystemConfigManager constantSystemConfigManager)
        {
            this.vODOrderManager = vOdOrderManager;
            this.storeOrderManager = storeOrderManager;
            this.logManager = logManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
        }

        /// <summary>
        /// 1.查找订单是否存在，如果订单存在，更新数据库中的订单表,然后再将请求支付结果信息入库；否则返回Null。
        /// </summary>
        /// <param name="paymentInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VodPaymentCallBack")]
        [CheckSignFilter]
        [PayMentException]
        public HttpResponseMessage VodPaymentCallBack(PaymentInfo paymentInfo)
        {
            return CheckPaymentInfo(paymentInfo, (payInfo, callBackData) =>
            {
                var order = vODOrderManager.PayMentCallBack(payInfo, callBackData);

                if (string.IsNullOrWhiteSpace(order.PayType) || order.PayType.ToLower().Equals(PayType.Movie.ToString().ToLower()))
                {
                    vODOrderManager.UpdateOrderCache();
                }
                else if (order.PayType.ToLower().Equals(PayType.Daily.ToString().ToLower()))
                {
                    vODOrderManager.UpdateDailyOrderCache();
                }
            });
        }

        /// <summary>
        /// 通用支付回调
        /// </summary>
        /// <param name="paymentInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PaymentCallBack")]
        [CheckSignFilter]
        [PayMentException]
        public HttpResponseMessage PaymentCallBack(PaymentInfo paymentInfo)
        {
            return CheckPaymentInfo(paymentInfo, (payInfo, callBackData) =>
            {
                var order = storeOrderManager.PaymentCallBack(payInfo, callBackData);
                if (order.GetTransactionstate() == Transactionstate.Paid)
                    OrderCompleteCallBack(order);
            });
        }

        /// <summary>
        /// YeahTvApi通知前台支付订单
        /// </summary>
        /// <param name="orderId"></param>
        [HttpGet]
        [Route("QtPayCallBack")]
        public HttpResponseMessage QtPayCallBack(string orderId)
        {
            string message = "Success";
            if (string.IsNullOrWhiteSpace(orderId))
                message = "订单号不能为空！";
            else
            {
                var order =
                    storeOrderManager.SearchStoreOrder(new StoreOrderCriteria() {Orderid = orderId}).FirstOrDefault();
                if (order != null && order.Status == (int) OrderState.Paying &&
                    order.PayInfo.ToLower() == PayPaymentModel.QTPAY.ToString().ToLower())
                {
                    OrderCompleteCallBack(order);
                }
                else
                    message = "订单不存在或状态不为等待前台支付！";
            }
            return new HttpResponseMessage { Content = new StringContent(message) };
        }


        private HttpResponseMessage CheckPaymentInfo(PaymentInfo paymentInfo, Action<PaymentInfo, PaymentCallBackData> callBackAction)
        {
            var errorMessage = string.Empty;
            var paymentCallBackData = JsonConvert.DeserializeObject<PaymentCallBackData>(paymentInfo.Data);
            if (paymentCallBackData != null)
            {
                callBackAction(paymentInfo, paymentCallBackData);
            }
            else
                errorMessage = "参数错误！";


            logManager.SaveInfo(string.Format("{0} {1}", errorMessage, JsonConvert.SerializeObject(paymentInfo)), "支付回调", AppType.CommonFramework);

            return new HttpResponseMessage { Content = new StringContent((string.IsNullOrWhiteSpace(errorMessage) ? "Success" : errorMessage)) };
        }

        private void OrderCompleteCallBack(StoreOrder order)
        {
            var notifyUrl = constantSystemConfigManager.ShoppingMallUrl + "/HotelMall/webapi/notifyPendingOrder.do";
            var orderNotifyInfo = new OrderNotifyInfo()
            {
                Amount = order.Price.ToString(),
                OrderId = order.Id,
                OrderTime = order.CreateTime,
                PayType = order.PayInfo,
                RoomNum = order.RoomNo,
                Status = order.GetTransactionstate().ToString(),
                StatusText = order.GetTransactionstate().GetDescription(),
                HotelId = order.Hotelid,
                DeliveryType = order.DeliveryType.ParseAsEnum<DeliveryType>().GetDescription(),
                Goods = order.OrderProducts.Select(m => new GoodsInfo()
                {
                    GoodsId = m.ProductId,
                    Name = m.ProductName,
                    Price = m.UnitPrice.ToString(),
                    Quantity = m.Quantity
                }).ToList()
            };


            var data = new PostParameters<OrderNotifyInfo>()
            {
                Data = orderNotifyInfo,
                Sign = new StringBuilder().Append(JsonConvert.SerializeObject(orderNotifyInfo)).Append(constantSystemConfigManager.StoreSignPrivateKey).ToString().StringToMd5()
            };

            bool isOk = false;
            var tryCount = 0;
            do
            {
                tryCount++;
                var result = (new HttpHelper { ContentType = "application/json" }).Post(notifyUrl,
                    JsonConvert.SerializeObject(data));

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var response = JsonConvert.DeserializeObject<ResponseData<OrderNotifyRespon>>(result);
                    isOk = response.Data.Success;
                }


                logManager.SaveInfo(string.Format("{0} {1} {2}", notifyUrl, result, JsonConvert.SerializeObject(data)), "订单通知", AppType.CommonFramework);

            } while (!isOk && tryCount <= 3);
        }
    }
}