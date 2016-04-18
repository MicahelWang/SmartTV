using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DomainModels
{
    public class GlobalConfig : BaseEntity<string>
    {
        public GlobalConfig()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
        public string PermitionType { get; set; }
        public string TypeId { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayConfigName")]
        public string ConfigName { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayConfigValue")]
        public string ConfigValue { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayConfigDescribe")]
        public string ConfigDescribe { get; set; }
        public bool Active { get; set; }
        public int PriorityLevel { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastUpdateUser")]
        public string LastUpdateUser { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayLastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }

    }
}
