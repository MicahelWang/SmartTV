using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class ReceiveOrders
    {
        // <summary>
        /// 支付价格
        /// </summary>
        public decimal PaidMoney;
        /// <summary>
        /// 价格
        /// </summary>
        public Dictionary<string, string> OriginalPrice;
        public string BillId { get; set; }
        public string OrderSource { get; set; }
        public string OrderMemeberLevel { get; set; }
        public string PriceLevel { get; set; }
        public bool IsRoomEmpty { get; set; }
        public string ReceiveOrderId { get; set; }
        public string HotelId { get; set; }
        public string HotelName { get; set; }
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string RoomNumber { get; set; }
        public string GuestName { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public string ReservationOrderId { get; set; }
        public string ActivityId { get; set; }
        public string ActivityName { get; set; }
        /// <summary>
        /// 是否支持续租
        /// </summary>
        public bool IsSupportContinue { get; set; }
        /// <summary>
        /// 是否至此退房
        /// </summary>
        public bool IsSupportCheckout { get; set; }
        /// <summary>
        /// 是否正在支付
        /// </summary>
        public bool IsWaitingPayment { get; set; }
        public bool IsNeedRecption { get; set; }
    }
}
