using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class ViewCopyTemplate
    {
        public string SourceTemplateId { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_Display_SourceTemplateName")]
        public string SourceTemplateName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTvTemplateName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_ViewCopyTemplate_Name")]
        public string Name { get; set; }
        public string CreateUser { get; set; }
    }
}
