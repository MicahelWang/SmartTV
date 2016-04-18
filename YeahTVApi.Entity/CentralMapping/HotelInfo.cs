using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 酒店实体信息
    /// </summary>
    [Serializable]
    public class HotelInfo
    {
        
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
        
        public int HotelStyle { get; set; }
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
        /// 营业日期(开业日期)
        /// </summary>
        
        public DateTime? BusinessDate { get; set; }
        /// <summary>
        /// NCID
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
        /// <summary>
        /// 与酒店相关的通知
        /// </summary>
        
        public virtual ICollection<HotelNotice> HotelNotice { get; set; }
        /// <summary>
        /// 与酒店相关的照片
        /// </summary>
        
        public virtual ICollection<HotelPhoto> HotelPhoto { get; set; }
        /// <summary>
        /// 与酒店相关的最低价格
        /// </summary>
        
        public virtual ICollection<HotelLowestPrice> HotelLowestPrice { get; set; }
        /// <summary>
        /// 与酒店相关的排序数据
        /// </summary>
        
        public virtual ICollection<HotelSortValue> HotelSortValue { get; set; }
        /// <summary>
        /// 与酒店相关的暂停营业表
        /// </summary>
        
        public virtual ICollection<HotelClosed> HotelClosed { get; set; }
        /// <summary>
        /// 与酒店相关的到达线路
        /// </summary>
        
        public virtual ICollection<HotelRoute> HotelRoute { get; set; }
        /// <summary>
        /// 酒店设施
        /// </summary>
        
        public virtual ICollection<HotelFacility> HotelFacility { get; set; }
        // <summary>
        // 与酒店相关的常用联系人
        // </summary>
        //
        //public virtual ICollection<MemberUsualHotel> MemberUsualHotel { get; set; }
    }

}
