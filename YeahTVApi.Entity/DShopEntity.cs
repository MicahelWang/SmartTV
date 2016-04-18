using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace YeahTVApi.Entity
{
    /// <summary>
    /// Summary description for ShopEntity
    /// </summary>
    public class DShopEntity
    {

        public string shopID { get; set; }
        public string shopName { get; set; }

        public string latitude { get; set; }
        public string longitude { get; set; }

        public string categoryName { get; set; }
        public string address { get; set; }
        public string urlWap { get; set; }

        public string distance { get; set; }


        public string PhotoURL { set; get; }

        public int AvgPrice { get; set; }

        /// <summary>
        /// 距离
        /// </summary>
        public double RedLen { set; get; }

        /// <summary>
        /// 图标文件URL
        /// </summary>
        public String IconURL { set; get; }
    }
}