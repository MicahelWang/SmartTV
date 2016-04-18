using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 酒店路线实体
    /// </summary>
    public class HotelRoute
    {

        public String RouteDesc {get;set;}
        public bool IsBus {get;set;}
        public bool IsDrive { get; set; }
        public bool IsTaxi { get; set; }
        public bool IsMetro { get; set; }
        public bool IsWalk { get; set; }
        public String RouteName { get; set; }
        public String RouteFrom { get; set; }   
    }
}
