using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class CatchHotel
    {
        public string HoteId { get; set; } 
        public string HotelName { get; set; } 
        public string HotelAdress { get; set; } 
        public string BrandId { get; set; }
        public string BrandName { get; set; } 
        public string districtId { get; set; }
        public string districtName { get; set; }
        public string HotelNameEn { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string Tel { get; set; }
        public string Image { get; set; }
    }
}
