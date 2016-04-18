using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 汉庭所属的类别
    /// </summary>
    public class HTCategory
    {
        public String Code { get; set; }
        public String Name { get; set; }
        public String URL { get; set; }
        public String BrandCode;
        public String GeoInfo;
    }


    /// <summary>
    /// 点评网下的类别
    /// </summary>
    public class DPCategory
    {
        public string categoryID { get; set; }
        public string categoryName { get; set; }
        public string parentID { get; set; }
        public string shopType { get; set; }
        public string categoryIcon { get; set; }
    }
   
}
