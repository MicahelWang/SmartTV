using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class HotelMovieVodOfDay
    {
        public string HotelId { get; set; }
        public string MovieId { get; set; }
        public string MovieName { get; set; }
        public int VodCount { get; set; }
        public DateTime Date { get; set; }
    }

    public class MovieVodItem
    {
        [JsonProperty("movieid")]
        public string MovieId { get; set; }
    }
}
