using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace YeahTVApi.DomainModel.Models
{
    public partial class TVChannel : BaseEntity<string>
    {
        public TVChannel()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVChannelName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_TvChannel_Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVChannelNameEn")]
        public string NameEn { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVChannelIcon")]
        public string Icon { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVChannelCategory")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_TvChannel_Category")]
        public string Category { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVChannelCategoryEn")]
        public string CategoryEn { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTVChannelDefaultCode")]
        public string DefaultCode { get; set; }

        public DateTime LastUpdateTime { get; set; }

        [NotMapped]
        public string IconPath { get; set; }
    }
}
