using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class HotelStartPercentage
    {
        public string HotelId { get; set; }
        public double StartPercentageOfDay { get; set; }
        public DateTime Date { get; set; }
        public int DeviceCount { get; set; }
        public int ActiveCount { get; set; }
    }

}
