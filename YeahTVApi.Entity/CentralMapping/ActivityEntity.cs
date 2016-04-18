using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 活动实体
    /// </summary>
    public class ActivityEntity
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public string ActivityID { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// 特殊活动类型标记(尾房, 连续入住, 提前入住)
        /// </summary>
        public ConditionType ConditionType { get; set; }

        /// <summary>
        /// 是否控制房量
        /// </summary>
        public bool IsControlRoomNumber { get; set; }

        /// <summary>
        /// 是否控制房型
        /// PS:当前市场活动系统还不支持
        /// </summary>
        public bool IsControlRoomType { get; set; }

        /// <summary>
        /// 是否绑定会员价
        /// </summary>
        public bool IsBindMemberPrice { get; set; }

        /// <summary>
        /// 是否配置促销码
        /// </summary>
        public bool IsSetPromotionCode { get; set; }

        /// <summary>
        /// 是否仅在专题页面显示
        /// </summary>
        public bool IsDisplayForThematic { get; set; }

        /// <summary>
        /// 是否可以使用优惠券
        /// </summary>
        public bool IsCanUseCoupon { get; set; }

        /// <summary>
        /// 活动开始日期
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 活动结束日期
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 每日开始时段;格式:20:00
        /// </summary>
        public string BeginTailTime { get; set; }

        /// <summary>
        /// 每日结束时段;格式:10:00
        /// </summary>
        public string EndTailTime { get; set; }

        /// <summary>
        /// 参与市场活动的房型
        /// </summary>
        public List<string> RoomTypes { get; set; }

        /// <summary>
        /// 市场活动房量
        /// </summary>
        public List<RoomStockEntity> RoomStockList { get; set; }

        ///// <summary>
        ///// 图片渠道 ios android 等
        ///// </summary>
        //public int DisplayChannel { get; set; }

        ///// <summary>
        ///// 图片类型 icon, 大图片
        ///// </summary>
        //public int DisplayType { get; set; }

        ///// <summary>
        ///// 图片链接
        ///// </summary>
        //public string Url { get; set; }

        /// <summary>
        /// 活动图片链接
        /// </summary>
        public string ActivityUrl { get; set; }

        /// <summary>
        /// Banner 图片链接
        /// </summary>
        public string BannerUrl { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public int PKID { get; set; }

        /// <summary>
        /// SEO 描述
        /// </summary>
        public string SeoDescription { get; set; }

        /// <summary>
        /// SEO 关键字
        /// </summary>
        public string SeoKeyWord { get; set; }

        /// <summary>
        /// SEO 标题
        /// </summary>
        public string SeoTitle { get; set; }

        /// <summary>
        /// 分享描述
        /// </summary>
        public string ShareDescription { get; set; }

        /// <summary>
        /// 分享照片链接
        /// </summary>
        public string SharePhotoUrl { get; set; }

        /// <summary>
        /// 支持的会员级别;该字段值为空时，表示不限制会员级别
        /// </summary>
        public string[] SupportMemberLevel { get; set; }
    }
}
