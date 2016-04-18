using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace YeahTVApi.DomainModel.Models
{
    public partial class MovieTemplate : BaseEntity<string>
    {
        public MovieTemplate()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTitle")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_MovieTemplate_Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayTags")]
        public string Tags { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayMovieDescription")]
        public string Description { get; set; }

        public double MovieCount { get; set; }
        public double HotelCount { get; set; }
        public string LastUpdateUser { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public virtual ICollection<MovieTemplateRelation> MovieTemplateRelations { get; set; }
        [JsonIgnore]
        public virtual ICollection<HotelMovieTrace> HotelMovieTraces { get; set; }
    }
}
