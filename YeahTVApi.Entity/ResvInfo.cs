using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 每日价格
    /// </summary>
    public class DailyPrice
    {

        /// <summary>
        /// 市场活动
        /// </summary>
        public string activityCode { get; set; }
        /// <summary>
        /// 门市价
        /// </summary>
        public decimal marketPrice { get; set; }
        /// <summary>
        /// 当前享有的价格
        /// </summary>
        public decimal currentPrice { get; set; }
        /// <summary>
        /// 营业日
        /// </summary>
        public DateTime bizDate { get; set; }
        /// <summary>
        /// 实付价
        /// </summary>
        public decimal payment { get; set; }
        /// <summary>
        /// 积分额
        /// </summary>
        public int point { get; set; }
        /// <summary>
        /// 房价码
        /// </summary>
        public string rateCode { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal discountPrice { get; set; }
        /// <summary>
        /// 早餐数量
        /// </summary>
        public int breakfastCount { get; set; }
        /// <summary>
        /// 可用房量
        /// </summary>
        public int usableRoomCount { get; set; }

        /// <summary>
        /// 每日所需积分数量
        /// </summary>
        public int exchangeRoomPoint { get; set; }
    }
    /// <summary>
    /// 减价信息
    /// </summary>
    public class ReduceInfo
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string date;

        /// <summary>
        /// 该天减少的金额
        /// </summary>
        public decimal amount;
    }

    /// <summary>
    /// 预定信息
    /// </summary>
    public class ResvInfo : OrderInfo
    {
        /// <summary>
        /// 到达时间
        /// </summary>
 
        /// <summary>
        /// 常用发票抬头
        /// </summary>
        public List<Entity.CentralMapping.InvoiceTitle> FrequentInvoices { get; set; }


        /// <summary>
        /// 积分加速提示信息
        /// </summary>
        public String ExtraPointsDescription;

        /// <summary>
        /// 是否允许选用常用联系人
        /// </summary>
        public int isAllowUseContactUsers { get; set; }

        /// <summary>
        /// 是否可选用积分加速
        /// </summary>
        public int isCanExtraPoints { get; set; }



        /// <summary>
        /// 是否可选用电子优惠券
        /// </summary>
        public int isCanUseECoupon { get; set; }


        /// <summary>
        /// 是否必须线上预付款
        /// </summary>
        public int isMustOnlinePay { get; set; }

        /// <summary>
        /// 每日价
        /// </summary>
        public List<DailyPrice> dailyPriceList { get; set; }
        /// <summary>
        /// 最大可选预定量
        /// </summary>
        public int memberCanBookingMaxNum { get; set; }

 

        public int BookingDays { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>

        public int InvoiceType { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>

        public string InvoiceTitle { get; set; }

        public String HotelImage { get; set; }

        public Boolean ExistIdentity;

    }


}
