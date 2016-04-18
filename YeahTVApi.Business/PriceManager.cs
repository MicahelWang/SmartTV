namespace HZTVApi.Manager
{
    using HZTVApi.Entity.CentralMapping;
    using HZTVApi.Infrastructure;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class PriceManager : IPriceManager
    {
        private IPriceService priceService;
        private IPriceFutureService priceFutureService;

        public PriceManager(IPriceService priceService, IPriceFutureService priceFutureService)
        {
            this.priceService = priceService;
            this.priceFutureService = priceFutureService;
        }

        /// <summary>
        /// 获取当前接待单的房价
        /// </summary> 
        /// <returns></returns>
        public ReceiveOrders GetOrderPrice(string hotelId, string vNumber, string receiveOrderId)
        {
            try
            {
                return priceService.GetOrderPrice(hotelId, vNumber, receiveOrderId);

            }
            catch (Exception)
            {

                return null;
            }


        }
        /// <summary>
        /// 获取当前接待单的房价
        /// </summary> 
        /// <returns></returns>
        public ReceiveOrders GetOrderPriceByRoomId(string hotelId, string roomId, string receiveOrderId)
        {
            PriceAttribute priceAttribute = new PriceAttribute();
            try
            {
                return priceAttribute.GetOrderPriceByRoomId(hotelId, roomId, receiveOrderId);

            }
            catch (Exception)
            {

                return null;
            }


        }
        // <summary>
        /// 获取当前接单未来房价
        /// </summary> 
        /// <returns></returns>
        public Dictionary<DateTime, decimal> GetFutureOrderPrice(string hotelId, string vNumber, string receiveOrderId, int continueDays, bool isHalfDay, string nationalId)
        {
            try
            {

                return priceFutureService.GetFutureOrderPrice(hotelId, vNumber, receiveOrderId, continueDays, isHalfDay, nationalId);
            }
            catch (Exception)
            {

                return null;
            }
        }

    }
}
