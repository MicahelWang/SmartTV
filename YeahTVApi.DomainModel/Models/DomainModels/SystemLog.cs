using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class SystemLog : BaseEntity<int>
    {   
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogMessageInfo")]
        public string MessageInfo { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogMessageInfoEx")]
        public string MessageInfoEx { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogMessageType")]
        public string MessageType { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogAppType")]
        public string AppType { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogCreateTime")]
        public System.DateTime CreateTime { get; set; }
    }
}
