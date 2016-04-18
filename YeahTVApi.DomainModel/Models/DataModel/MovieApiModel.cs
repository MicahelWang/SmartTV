using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class MovieApiModel
    {
        public string MovieId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string MovieReview { get; set; }
        public string MovieReviewEn { get; set; }
        public List<string> PosterAddress { get; set; }
        public string CoverAddress { get; set; }
        public string VideoUrl { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<bool> IsFree { get; set; }
        public System.DateTime LastViewTime { get; set; }
    }
}
