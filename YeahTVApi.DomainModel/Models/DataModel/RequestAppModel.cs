using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
  public  class RequestAppModel
    {
      [JsonProperty(Order = 2)]
      public string Brand { get; set; }
       [JsonProperty(Order = 6)]
      public string DEVNO { get; set; }
       [JsonProperty(Order = 10)]
      public string IP { get; set; }
       [JsonProperty(Order = 3)]
      public string Manufacturer { get; set; }
       [JsonProperty(Order = 4)]
      public string Model { get; set; }
       [JsonProperty(Order = 9)]
      public string OSVersion { get; set; }
       [JsonProperty(Order = 7)]
      public int ScreenDpi { get; set; }
       [JsonProperty(Order = 12)]
      public int ScreenHeight { get; set; }
       [JsonProperty(Order = 13)]
      public int ScreenWidth { get; set; }
       [JsonProperty(Order = 8)]
      public string PackageName { get; set; }
      [JsonProperty(Order=1)]
       public string Ver { get; set; }
      [JsonProperty(Order = 5)]
      public string Product { get; set; }

      [JsonProperty(Order = 11)]
      public string Language { get; set; }

    }
   
}
