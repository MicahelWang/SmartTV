using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 酒店告示
    /// </summary>
    public class HotelNotice
    {
        public System.DateTime BeginTime { get; set; }
       
        public System.DateTime EndTime { get; set; }
       
        public string Description { get; set; }
       
        public bool InUse { get; set; }
      
        public bool IsAboutResv { get; set; }

   
    }
}
