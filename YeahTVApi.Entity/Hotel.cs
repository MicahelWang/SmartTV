using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 会员实体
    /// </summary>
    public class Hotel
    {
        public String hotelStyleName;
        /// <summary>
        /// 该酒店所有的活动列表
        /// </summary>
        public Dictionary<String,String> Activity { get; set; }
        /// <summary>
        /// 酒店ID
        /// </summary>
        public string hotelID { get; set; }

        public String BrandTitle;
        public String BrandDescription;

        /// <summary>
        /// 酒店名称
        /// </summary>
        public string hotelName { get; set; }

        /// <summary>
        /// 酒店简称
        /// </summary>
        public string hotelShortName { get; set; }

        /// <summary>
        /// 酒店品牌
        /// </summary>
        public string hotelStyle { get; set; }

        /// <summary>
        /// 酒店地址
        /// </summary>
        public string address { get; set; }

        public List<HotelImage> Images { get; set; }

        /// <summary>
        /// 点评分数
        /// </summary>
        public decimal commentScore { get; set; }


        /// <summary>
        /// 点评个数
        /// </summary>
        public int commentCount { get; set; }
        
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal? lowestPrice { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// 备注/描述
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 经纬度
        /// </summary>
        public string geoInfo { get; set; }




        /// <summary>
        /// 房型信息
        /// </summary>
        public IList<RoomInfo> rooms { get; set; }

        /// <summary>
        /// 服务设施
        /// </summary>
        public IList<HotelService> services { get; set; }

        /// <summary>
        /// 允许的卡
        /// </summary>
        public string allowCardType { get; set; }

        /// <summary>
        /// 酒店所在城市Code
        /// </summary>
        public string cityCode { get; set; }

        /// <summary>
        /// 酒店所在城市
        /// </summary>
        public string cityName { get; set; }

        /// <summary>
        /// 酒店区域
        /// </summary>
        public string hotelArea { get; set; }


        /// <summary>
        /// 提示信息
        /// </summary>
        public List<HotelNotice> Notice { get; set; }

        /// <summary>
        /// 是否有360全景
        /// </summary>
        public String URL360 { get; set; }

        /// <summary>
        /// 是否可以卡支付
        /// </summary>
        public bool IsCanCardPay { get; set; }
        /// <summary>
        /// 是否可以第三方支付
        /// </summary>
        public bool IsCanThirdPay { get; set; }

        /// <summary>
        /// 是否开放自助CheckIn
        /// </summary>
        public bool IsOpenCheckIn { get; set; }


    }
}
