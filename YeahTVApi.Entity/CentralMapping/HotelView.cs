
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 酒店信息视图
    /// </summary>
    [Serializable]
    public class HotelView
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        
        public string CityName { get; set; }

        /// <summary>
        /// 酒店最低价集合
        /// </summary>
        
        public IEnumerable<HotelLowestPrice> HotelLowestPrice { get; set; }

        /// <summary>
        /// 酒店通知集合
        /// </summary>
        public IEnumerable<HotelNotice> HotelNotice { get; set; }

        /// <summary>
        /// 酒店设施
        /// </summary>
        
        public IEnumerable<HotelFacility> HotelFacility { get; set; }

        /// <summary>
        /// 酒店排序
        /// </summary>
        public IEnumerable<HotelSortValue> HotelSortValue { get; set; }

 
        /// <summary>
        /// 酒店房型扩展信息
        /// </summary>
        public IEnumerable<HotelRoomTypeEx> HotelRoomTypeEx { get; set; }

        /// <summary>
        /// 酒店促销信息
        /// </summary>
        
        public IEnumerable<HotelPromotionView> HotelPromotion { get; set; }

        /// <summary>
        /// 酒店照片
        /// </summary>
        
        public IEnumerable<HotelPhoto> HotelPhoto { get; set; }

        /// <summary>
        /// 酒店支持的信用卡
        /// </summary>
        public IEnumerable<HotelSupportCreditCardView> HotelSupportCreditCard { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        
        public string ID { get; set; }
        /// <summary>
        /// 酒店名称
        /// </summary>
        
        public string HotelName { get; set; }
        /// <summary>
        /// 酒店英文名称
        /// </summary>
        
        public string HotelNameEn { get; set; }
        /// <summary>
        /// 酒店品牌
        /// </summary>
        
        public HotelStyle HotelStyle { get; set; }
        /// <summary>
        /// 所属国家位置
        /// </summary>
        
        public string Nation { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        
        public string Zip { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        
        public string Telephone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        
        public string Fax { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        
        public string Address { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        
        public string Descript { get; set; }
        /// <summary>
        /// 财务编号;不需要输出
        /// </summary>
        //
        //public string NCID { get; set; }
        /// <summary>
        /// 所在城市编号
        /// </summary>
        
        public int CityID { get; set; }
        /// <summary>
        /// 归属管辖分公司ID,从SSC库中同步;SSC表中的AreaID字段,在这里统称分公司ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 总房间数量
        /// </summary>
        
        public int RoomCount { get; set; }
        /// <summary>
        /// 经营类型 直营/加盟/市场联盟等
        /// </summary>
        
        public int HotelType { get; set; }
        /// <summary>
        /// 是否开业
        /// </summary>
        
        public bool IsOpening { get; set; }
        /// <summary>
        /// 营业日期(开业日期)
        /// </summary>
        
        public DateTime? BusinessDate { get; set; }
        /// <summary>
        /// 是否筹备中
        /// </summary>
        
        public bool IsShowPrepare { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        
        public decimal CommentScore { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        
        public int CommentCount { get; set; }
        /// <summary>
        /// 酒店照片
        /// </summary>
        
        public string HeaderPhoto { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        
        public decimal Lng { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        
        public decimal Lat { get; set; }
        /// <summary>
        /// 是否仅限接待内宾
        /// </summary>
        
        public bool IsOnlyChinese { get; set; }
        /// <summary>
        /// 是否支持储值卡
        /// </summary>
        
        public bool IsSupportValueCardPay { get; set; }
        /// <summary>
        /// 是否支持360全景
        /// </summary>
        
        public bool IsSupport360View { get; set; }
        /// <summary>
        /// 是否开放预定
        /// </summary>
        
        public bool IsOpenResv { get; set; }
        /// <summary>
        /// 是否开放在线预定
        /// </summary>
        
        public bool IsOpenOnlineCheckIn { get; set; }
    }

    /* 
CityName
ID
HotelName
HotelNameEn
HotelStyle
Nation
Zip
Telephone
Fax
Address
Descript
NCID
CityID
CompanyID
RoomCount
HotelType
IsOpening
IsShowPrepare
CommentScore
CommentCount
HeaderPhoto
Lng
Lat
IsOnlyChinese
IsSupportValueCardPay
IsSupport360View
IsOpenResv
IsOpenOnlineCheckIn
     */
}
