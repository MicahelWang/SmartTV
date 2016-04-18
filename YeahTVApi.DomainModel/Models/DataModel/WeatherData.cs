using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class WeatherData
    {
        public string Date { get; set; }

        public string DayPictureUrl { get; set; }

        public string Weather { get; set; }

        public string Wind { get; set; }

        public string Ttemperature { get; set; }
    }

    public class WeatherDataWithPm25 : WeatherData
    {
        public string Pm25 { get; set; }
    }

}
