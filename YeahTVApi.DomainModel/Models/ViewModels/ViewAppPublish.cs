using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    [Serializable]
    public class ViewAppPublish
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DeviceBingType")]
        public BindingType BindType { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelName")]
        public string HotelId { get; set; }
        public AppPublish AppPub { get; set; }

    }
}
