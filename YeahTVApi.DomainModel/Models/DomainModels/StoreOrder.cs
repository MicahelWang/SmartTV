using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.Models
{
    public class StoreOrder : BaseEntity<string>
    {
        [JsonConverter(typeof(StringDateTimeConverter))]
        public DateTime CreateTime { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        [JsonConverter(typeof(StringDateTimeConverter))]
        public DateTime CompleteTime { get; set; }
        public string SeriseCode { get; set; }
        public string RoomNo { get; set; }
        public string Hotelid { get; set; }
        public string HotelName { get; set; }
        public string GoodsName { get; set; }
        public string GoodsDesc { get; set; }
        public string DeliveryType { get; set; }
        public string PayInfo { get; set; }
        public bool IsDelete { get; set; }
        public string InvoicesTitle { get; set; }
        public int DeliveryState { get; set; }
        [JsonConverter(typeof(StringDateTimeConverter))]
        public DateTime ExpirationDate { get; set; }
        public ICollection<OrderProducts> OrderProducts { get; set; }

        [NotMapped]
        public string TransactionState { get; set; }
        /// <summary>
        ///支付完成，待配送：Status=3 and DeliveryState=0  
        ///等待前台支付：Status=1 and PayInfo=’QTPAY’ 
        ///未支付：Status=0 
        ///已完成：Status=3 and DeliveryState=1 
        ///已取消：Status=4 or ( Status in (0,1) and getdate() > ExpirationDate )
        /// </summary>
        /// <returns></returns>
        public Transactionstate GetTransactionstate()
        {
            Transactionstate transactionstate = Transactionstate.Cancel;

            if (Status == (int)OrderState.Cancel || ((Status == (int)OrderState.Unpaid || Status == (int)OrderState.Paying || Status ==(int)OrderState.Fail) && DateTime.Now > ExpirationDate && PayInfo.ToLower().Trim() != PayPaymentModel.QTPAY.ToString().ToLower().Trim()))
            {
                transactionstate = Transactionstate.Cancel;
            }
            else if ((Status == (int)OrderState.Unpaid || Status == (int)OrderState.Paying || Status ==(int)OrderState.Fail) && PayInfo.ToLower() == PayPaymentModel.QTPAY.ToString().ToLower())
            {
                transactionstate = Transactionstate.Waiting;
            }
            else if (Status == (int)OrderState.Fail || (Status == (int)OrderState.Unpaid || Status == (int)OrderState.Paying) && DateTime.Now <= ExpirationDate && PayInfo.ToLower().Trim() != PayPaymentModel.QTPAY.ToString().ToLower().Trim())
            {
                transactionstate = Transactionstate.Unpaid;
            }
            else if (Status == (int)OrderState.Success && DeliveryState == (int)Enum.DeliveryState.UnDelivery)
            {
                transactionstate = Transactionstate.Paid;
            }
            else if (Status == (int)OrderState.Success && DeliveryState == (int)Enum.DeliveryState.Delivery)
            {
                transactionstate = Transactionstate.Transactionscomplete;
            }

            return transactionstate;
        }
    }
}