using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class MovieForLocalize : BaseEntity<string>
    {
        public MovieForLocalize()
        { 
            this.Id = Guid.NewGuid().ToString("N");
            this.Tags = new List<IEnumerable<LocalizeResource>>();
        }

        public string Name { get; set; }
        public string Director { get; set; }
        public string Starred { get; set; }
        public string District { get; set; }
        public string Mins { get; set; }
        public Nullable<int> Vintage { get; set; }
        public string MovieReview { get; set; }
        public string PosterAddress { get; set; }
        public string CoverAddress { get; set; }
        public string TagIds { get; set; }
        public string VodUrl { get; set; }
        public string MD5 { get; set; }
        public string Language { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<decimal> DefaultAmount { get; set; }
        public string CurrencyType { get; set; }
        public string Attribute { get; set; }        
        public Nullable<int> Order { get; set; }
        public Nullable<bool> IsTop { get; set; }
        public int? HotelCount { get; set; }
        public DateTime CreateTime { get; set; }
        public System.DateTime LastUpdateTime { get; set; }
        public string LastUpdateUser { get; set; }
        public bool DistributeAll { get; set; }
        public string FirstWord { get; set; }

        [JsonIgnore]
        public ICollection<HotelMovieTraceNoTemplate> HotelMovieTraceNoTemplates { get; set; }

        [NotMapped]
        public virtual ICollection<string> PosterAddressPath { get; set; }

        [NotMapped]
        public virtual IEnumerable<LocalizeResource> Names { get; set; }

        [NotMapped]
        public virtual IEnumerable<LocalizeResource> Directors { get; set; }

        [NotMapped]
        public virtual IEnumerable<LocalizeResource> Starreds { get; set; }

        [NotMapped]
        public virtual IEnumerable<LocalizeResource> Districts { get; set; }

        [NotMapped]
        public virtual IEnumerable<LocalizeResource> MovieReviews { get; set; }

        [NotMapped]
        public virtual IEnumerable<LocalizeResource> Languages { get; set; }

        [NotMapped]
        public virtual ICollection<IEnumerable<LocalizeResource>> Tags { get; set; }
        [NotMapped]
        public virtual string CoverAddressPath { get; set; } 
    }
}
