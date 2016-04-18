using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 房型详细信息（包括标准房价房量、各活动的房价房量）
    /// </summary>
    public class RoomDetailModel
    {
        internal RoomModel RoomHost { get; set; }

        internal ActivityEntity ActivityInfo { get; set; }

        
        /// <summary>
        /// 价格名称，如标准价，XXX活动价
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 市场活动ID，标准价时此值为空
        /// </summary>
        public string ActivityID { get; set; }

        /// <summary>
        /// 查询会员级别
        /// </summary>
        public string QueryMemberLevel { get; private set; }

        public bool IsOverBooked
        {
            get;
            set;
        }
        /// <summary>
        /// 房间类型
        /// </summary>
        public string RoomType { get; set; }
        /// <summary>
        /// 酒店编号, 便于前台绑定
        /// </summary>
        public string HotelID { get; set; }
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
        /// 是否绑定会员价
        /// </summary>
        public bool IsBindMemberPrice { get; set; }

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
        /// 每日价
        /// </summary>
        
        public List<RoomPriceEntity> DailyPriceInfo { get; set; }
        /// <summary>
        /// 每日房量
        /// </summary>
        public List<RoomStockEntity> DailyRoomStock { get; set; }
    }
}
