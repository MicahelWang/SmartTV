using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class AuthUserDeviceTrace : BaseEntity<string>
    {
        public AuthUserDeviceTrace()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_AuthUserDeviceTrace_UserId")]
        public string UserId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_AuthUserDeviceTrace_DeviceNo")]
        public string DeviceNo { get; set; }
        public System.DateTime BindTime { get; set; }
        public System.DateTime LastVisitTime { get; set; }
    }
}
