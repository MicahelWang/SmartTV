using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    [Serializable]
    public class HotelInfoStatistics
    {
        public string HotelId { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }
        public int DeviceTraceSeriesCount { get; set; }
        public int BackUpDeviceSeriesCount { get; set; }
        public int DeviceSeriesTotal { get; set; }
        public int YesterdayActive { get; set; }
        public decimal YesterdayMovieIncome { get; set; }
        public double YesterdayUsedTime { get; set; }
        public CoreSysHotel CoreSysHotel { get; set; }
        public CoreSysBrand CoreSysBrand { get; set; }
        public int ValidityDays { get; set; }
    }
}
