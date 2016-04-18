using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 点评网下的城市
    /// </summary>
    public class DPCity
    {
        public string cityID { get; set; }
        public string cityName { get; set; }
    }


    /// <summary>
    /// Summary description for DCityEntity
    /// </summary>
    public class DCityEntity
    {
        /// <summary>
        /// 城市区号
        /// </summary>
        public string CityAreaCode { set; get; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityID { set; get; }
        /// <summary>
        /// 城市名
        /// </summary>
        public string CityName { set; get; }
        /// <summary>
        /// 城市名拼音的第一个英文字母
        /// </summary>
        public string FirstChar { set; get; }
        /// <summary>
        /// 城市中心点纬度坐标
        /// </summary>
        public double Latitude { set; get; }
        /// <summary>
        /// 城市中心点经度坐标
        /// </summary>
        public double Longitude { set; get; }
        /// <summary>
        /// 该城市所属类型（如直辖市／华北地区／华东地区）
        /// </summary>
        public string RegionType { set; get; }

    }
}