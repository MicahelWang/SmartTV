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
    public partial class Movie : BaseEntity<string>
    {
        public Movie()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayMovieName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_Movie_Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayMovieNameEn")]
        public string NameEn { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayMovieReview")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required_Movie_MovieReview")]
        public string MovieReview { get; set; }
        public string MovieReviewEn { get; set; }
        public string PosterAddress { get; set; }
        public string CoverAddress { get; set; }
        public DateTime LastUpdateTime { get; set; }

        [NotMapped]
        public string CoverPath { get; set; }

        [NotMapped]
        public List<string> PosterPaths { get; set; }

        [JsonIgnore]
        public virtual ICollection<HotelMovieTrace> HotelMovieTraces { get; set; }

        [JsonIgnore]
        public virtual ICollection<MovieTemplateRelation> MovieTemplateRelations { get; set; }
    }
}
