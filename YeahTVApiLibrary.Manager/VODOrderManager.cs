using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;

namespace YeahTVApiLibrary.Manager
{

    public partial class VODOrderManager : IVODOrderManager
    {
        private IVODOrderRepertory vodOrderRepertory;
        private IVODPaymentResultManager vODPaymentResultManager;
        private readonly IRedisCacheService redisCacheService;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IGlobalConfigManager globalConfigManager;


        public VODOrderManager(IVODOrderRepertory vodOrderRepertory,
            IVODPaymentResultManager vODPaymentResultManager,
            IRedisCacheService redisCacheService,
            IConstantSystemConfigManager constantSystemConfigManager,
            IGlobalConfigManager globalConfigManager
            )
        {
            this.vodOrderRepertory = vodOrderRepertory;
            this.vODPaymentResultManager = vODPaymentResultManager;
            this.redisCacheService = redisCacheService;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.globalConfigManager = globalConfigManager;
        }

        #region Redis

        private List<VODOrder> GetFromCache(string key, PayType payType)
        {
            if (redisCacheService.IsSet(key))
                return redisCacheService.Get<List<VODOrder>>(key);
            var orders = SearchNotExpiresOrders(payType);
            redisCacheService.Add(key, orders);
            return redisCacheService.Get<List<VODOrder>>(key);
        }

        private void UpdateCache(string key, PayType payType)
        {
            var orders = SearchNotExpiresOrders(payType);
            redisCacheService.Remove(key);
            if (redisCacheService.IsSet(key))
                redisCacheService.Set(key, orders);
            else
            {
                redisCacheService.Add(key, orders);
            }
        }

        public List<VODOrder> GetOrdersFromCache()
        {
            return GetFromCache(RedisKey.VODOrdersKey, PayType.Movie);
        }
        public void UpdateOrderCache()
        {
            UpdateCache(RedisKey.VODOrdersKey, PayType.Movie);
        }


        public List<VODOrder> GetDailyOrdersFromCache()
        {
            return GetFromCache(RedisKey.VODDailyOrdersKey, PayType.Daily);
        }
        public void UpdateDailyOrderCache()
        {
            UpdateCache(RedisKey.VODDailyOrdersKey, PayType.Daily);
        }

        #endregion

        /// <summary>
        /// 创建订单，自动生成订单号
        /// </summary>
        /// <param name="vOdOrder"></param>
        public void Add(VODOrder vOdOrder)
        {
            vOdOrder.Id = vodOrderRepertory.GetNewOrderId();
            vodOrderRepertory.Insert(vOdOrder);
        }

        public List<VODOrder> SearchOrders(VODOrderCriteria orderCriteria)
        {
            return vodOrderRepertory.Search(orderCriteria);
        }
        public List<VODOrder> SearchNotExpiresOrders(PayType payType)
        {

            int order = Math.Abs(constantSystemConfigManager.VodOrderExpires);
            int dailyOrder = Math.Abs(constantSystemConfigManager.VodDailyOrderExpires);

            int maxDate = constantSystemConfigManager.VodOrderExpires;
            if (order < dailyOrder)
            {
                maxDate = constantSystemConfigManager.VodDailyOrderExpires;
            }

            return vodOrderRepertory.Search(new VODOrderCriteria()
            {
                CompleteBeginTime = DateTime.Now.AddHours(maxDate),
                orderState = OrderState.Success,
                payType = payType
            });
        }

        public void UpdateVODOrder(VODOrder vOdOrder)
        {
            var order = vodOrderRepertory.Search(new VODOrderCriteria { OrderId = vOdOrder.Id }).FirstOrDefault();

            vOdOrder.CopyTo(order, new[] { "id" });

            vodOrderRepertory.Update(order);
        }

        /// <summary>
        /// 支付网关回调，更新订单状态
        /// </summary>
        /// <param name="paymentInfo"></param>
        /// <param name="paymentCallBackData"></param>
        [UnitOfWork]
        public VODOrder PayMentCallBack(PaymentInfo paymentInfo, PaymentCallBackData paymentCallBackData)
        {
            var order = vodOrderRepertory.FindByKey(paymentCallBackData.ReturnInfo.OrderId);
            if (order == null || order.State != (int)OrderState.Paying)
                throw new Exception("订单不存在或订单状态异常！");

            var paymentResult = new VODPaymentResult()
            {
                CreateTime = DateTime.Now,
                ResultSign = paymentInfo.Sign,
                ResultMessage = paymentInfo.Message,
                ResultCode = paymentInfo.ResultCode,
                OrderId = paymentCallBackData.ReturnInfo.OrderId,
                NotifyTime = paymentCallBackData.ReturnInfo.NotifyTime
            };
            vODPaymentResultManager.Add(paymentResult);

            order.State = (int)(paymentInfo.ResultCode.Equals(PayMentOrderState.Success.GetValueStr()) ? OrderState.Success : OrderState.Fail);
            order.CompleteTime = DateTime.Now;

            vodOrderRepertory.Update(order);

            return order;
        }

        public void UpdateOrderCache(VODOrder order)
        {
            if (string.IsNullOrWhiteSpace(order.PayType) || order.PayType.ToLower().Equals(PayType.Movie.ToString().ToLower()))
            {
                UpdateOrderCache();
            }
            else if (order.PayType.ToLower().Equals(PayType.Daily.ToString().ToLower()))
            {
                UpdateDailyOrderCache();
            }
        }

        public VODOrder OrderPaySuccess(PaymentInfo paymentInfo, PaymentCallBackData paymentCallBackData)
        {
            var order = PayMentCallBack(paymentInfo, paymentCallBackData);
            UpdateOrderCache(order);
            return order;
        }

        /// <summary>
        /// 根据设备号、电影ID检测是否存在已支付的有效期内的订单 单部点播
        /// </summary>
        /// <param name="header"></param>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public VODOrder GetSuccessOrderByMovieId(RequestHeader header, string movieId)
        {
            return GetOrdersFromCache().FirstOrDefault(m =>
                m.HotelId == header.HotelID &&
                m.RoomNo == header.RoomNo &&
                m.SeriseCode == header.DEVNO &&
                m.MovieId == movieId &&
                m.CompleteTime >= DateTime.Now.AddHours(constantSystemConfigManager.VodOrderExpires));
        }

        /// <summary>
        /// 检查是否包天
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public VODOrder GetSuccessDailyOrder(RequestHeader header)
        {
            return GetDailyOrdersFromCache().FirstOrDefault(m =>
                m.HotelId == header.HotelID &&
                m.RoomNo == header.RoomNo &&
                m.SeriseCode == header.DEVNO &&
                m.CompleteTime >= DateTime.Now.AddHours(constantSystemConfigManager.VodDailyOrderExpires));
        }


        public VODOrder CreateNewOrder(TVVODRequest tvVODRequest, HotelMovieTraceNoTemplate movieInfo, HotelPayment hotelPayment, RequestHeader header, OrderState orderState = OrderState.Unpaid)
        {
            var order = new VODOrder
            {
                CreateTime = DateTime.Now,
                State = (int)orderState,
                CompleteTime = DateTime.Now,
                SeriseCode = header.DEVNO,
                RoomNo = header.RoomNo,
                HotelId = header.HotelID,
                GoodsDesc = "",
                PayInfo = tvVODRequest.PayPaymentModel.ToUpper(),
                PayType = tvVODRequest.PayType,
                IsDelete = false
            };

            if (tvVODRequest.PayType.ToLower().Equals(PayType.Daily.ToString().ToLower()))
            {
                order.MovieId = "";
                order.GoodsName = "畅看全网影片";
                order.Price = hotelPayment.PriceOfDay;
            }
            else
            {
                order.MovieId = tvVODRequest.movieId;
                order.Price = movieInfo.Price ?? 0m;

                var localizeResource = movieInfo.MovieForLocalize.Names.FirstOrDefault(m => m.Lang.ToLower() == "zh-cn");
                order.GoodsName = localizeResource != null ? "单片点播-" + localizeResource.Content.Replace("&", "").Replace("%", "").Replace("+", "") : "单片点播";
            }

            if (order.PayInfo.ToLower().Equals(PayPaymentModel.FZPAY.ToString().ToLower()))
            {
                order.State = (int)OrderState.Paying;
            }

            Add(order);

            return order;
        }

        public int GetIntScoresbyOderId(string OrderId)
        {
            return Convert.ToInt32(Math.Ceiling(GetScoresbyOderId(OrderId)));
        }

        public decimal GetScoresbyOderId(string OrderId)
        {
            var order = vodOrderRepertory.Search(new VODOrderCriteria
            {
                OrderId = OrderId
            }).FirstOrDefault();

            if (order != null)
            {
                string configValue = globalConfigManager.GetHotelScoreRate(order.HotelId);
                string increaseRaio = globalConfigManager.GetHotelIncreaseRatio(order.HotelId);
                if (!string.IsNullOrEmpty(configValue))
                {
                    increaseRaio = string.IsNullOrWhiteSpace(increaseRaio) ? "1" : increaseRaio;
                    decimal scores = order.Price * configValue.ToDecimal() * increaseRaio.ToDecimal();

                    return scores;
                }
                else
                {
                    throw new Exception("酒店未配置相关兑换比例！");
                }
            }
            else
            {
                throw new Exception("订单异常！");
            }
        }

        public string GetPorductId(VODOrder order)
        {
            return string.IsNullOrWhiteSpace(order.MovieId) ? "畅看".StringToMd5() : order.MovieId;
        }

        public VODOrder GetScoreOrderInfo(string orderId)
        {
            if (orderId.StartsWith("1000"))
            {
                orderId = orderId.Substring(4);
            }

            return SearchOrders(new VODOrderCriteria() { OrderId = orderId }).FirstOrDefault();
        }
        public string GetScoreOrderId(string orderId, string hotelId)
        {
            var hotelBusinessCode = globalConfigManager.GetHotelScoreBusinessCode(hotelId);
            return string.Format("{0}{1}", hotelBusinessCode, orderId);
        }

    }
}
