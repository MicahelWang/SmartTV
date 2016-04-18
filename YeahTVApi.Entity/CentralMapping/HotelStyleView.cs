using System;
using System.Runtime.Serialization;

namespace YeahTVApi.Entity.CentralMapping
{
    [Serializable]
    public class HotelStyleView
    {
        /// <summary>
        /// 酒店品牌值
        /// </summary>
       
        public int ID { get; set; }
        /// <summary>
        /// 酒店品牌名称
        /// </summary>
       
        public string Name { get; set; }
    }
}
