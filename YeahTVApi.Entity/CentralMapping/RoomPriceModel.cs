using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity.CentralMapping
{
    public class RoomPriceModel
    {
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
        /// 实付金额
        /// </summary>
        public decimal Payment { get; set; }

        /// <summary>
        /// 会员/市场活动定制价
        /// </summary>
        public decimal SpecificPrice { get; set; }

        /// <summary>
        /// 可用房量
        /// </summary>
        public int UsableCount { get; set; }
    }
}
