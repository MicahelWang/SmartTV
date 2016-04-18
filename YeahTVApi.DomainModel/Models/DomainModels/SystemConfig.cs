using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class SystemConfig : BaseEntity<int>
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayConfigName")]
        public string ConfigName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayConfigValue")]
        public string ConfigValue { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayConfigType")]
        public string ConfigType { get; set; }


        public string AppId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayActive")]
        public bool Active { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
