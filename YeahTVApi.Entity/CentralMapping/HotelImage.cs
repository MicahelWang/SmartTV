using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class HotelImage
    {
        
        public int ImageID { get; set; }
        public int ImageTypeID { get; set; }
        
        public string ImageName { get; set; }
        
        public string HotelID { get; set; }
        
        public string ImageDesc { get; set; }
        
        public string ImageUrl { get; set; }
        
        public string RoomTypeID { get; set; }
        
        public int? FacilityType { get; set; }
    }

}
