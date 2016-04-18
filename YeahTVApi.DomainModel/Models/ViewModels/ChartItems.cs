using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    [Serializable]
    public class ChartItems<T>
    {
        public ChartItems()
        {
            XAxisList = new List<string>();
            LegendList = new List<string>();
            SeriesList = new List<Series<T>>();
        }

        public List<string> XAxisList { get; set; }
        public List<string> LegendList { get; set; }
        public List<Series<T>> SeriesList { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }

    public class Series<T>
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }
    public class PieItem
    {
        [JsonProperty("value")]
        public double Value { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}