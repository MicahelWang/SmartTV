using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 房价对象
    /// </summary>
    public class RoomPriceEntity
    {
        /// <summary>
        /// 市场活动ID
        /// </summary>
        
        public string ActivityID { get; set; }

        /// <summary>
        /// 营业日
        /// </summary>
        
        public DateTime BizDate { get; set; }

        /// <summary>
        /// 早餐数量
        /// </summary>
        
        public int BreakfastCount { get; set; }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 兑换免房所需积分
        /// </summary>
        
        public int ExchangePoint { get; set; }

        /// <summary>
        /// 门市价
        /// </summary>
        
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 实付金额=会员价/市场活动价/促销价
        /// </summary>
        
        public decimal Payment { get; set; }

        /// <summary>
        /// 房价码，下单时需要
        /// </summary>
        public string RateCode { get; set; }
    }
}
