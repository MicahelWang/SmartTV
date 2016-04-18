using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class HotelModuleUsedTime
    {
        public List<ModuleUsedTimeItem> ModuleUsedTimeItems { get; set; }
        public string HotelId { get; set; }
        public DateTime Date { get; set; }
    }
    public class ModuleUsedTimeItem
    {
        [JsonProperty("modulename")]
        public string ModuleName { get; set; }
        [JsonProperty("usedtime")]
        public double UsedTime { get; set; }
    }

}
