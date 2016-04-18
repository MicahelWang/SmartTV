using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahCatchHotel.Models
{
    public class QitianHotelDataModel
    {
        public string innId { get; set; }
        public string  name { get; set; }
        public string firstCharsOfPinyin { get; set; }
        public string cityName { get; set; }
        public string cityId { get; set; }
        public string districtId { get; set; }
        public string address { get; set; }
        public decimal blat { get; set; }
        public decimal blng { get; set; }
        public string telephone { get; set; }
        public string districtName { get; set; }
        public string brandId { get; set; }
    }
}