using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class BehaviorLog : BaseEntity<string>
    {
        public BehaviorLog ()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayBehaviorLogBehaviorInfo")]
        public string BehaviorInfo { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayBehaviorLogBehaviorType")]
        public string BehaviorType { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelName")]
        public string HotelId { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayBehaviorLogCreateTime")]
        public DateTime CreateTime { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayDeviceSeries")]
        public string DeviceSerise { get; set; }
    }
}
