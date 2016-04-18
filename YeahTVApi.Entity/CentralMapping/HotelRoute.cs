using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class HotelRoute 
    {
        
        public int RouteID { get; set; }
        
        public string RouteFrom { get; set; }
        
        public string Description { get; set; }

        public bool? IsBus { get; set; }
        public bool? IsMetro { get; set; }
        public bool? IsDrive { get; set; }
        public bool? IsTaxi { get; set; }
        public bool? IsWalk { get; set; }
        public bool? StateCode { get; set; }
        public bool? DeletionStateCode { get; set; }

    }

}
