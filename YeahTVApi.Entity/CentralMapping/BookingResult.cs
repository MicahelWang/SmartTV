using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class BookingResult
    {
        /// <summary>
        /// 使用失败的优惠券号, 可以失败多条不影响流程
        /// </summary>
        public Dictionary<DateTime, string> FailedECoupons { get; set; }
        /// <summary>
        /// 使用失败的促销券号码, 一旦失败就会返回. 中断下单流程
        /// </summary>
        public KeyValuePair<DateTime, string> FailedPromotionCode { get; set; }
        /// <summary>
        /// pms订单号
        /// </summary>
        public string PmsResvNo { get; set; }
        /// <summary>
        /// 中央库订单号
        /// </summary>
        public string CenterResNo { get; set; }

        public string PayUrl { get; set; }
    }
}
