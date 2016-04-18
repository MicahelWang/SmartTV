using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class MovieApiNewModel
    {
        public string MovieId { get; set; }
        public string Name { get; set; }
        public string MovieReview { get; set; }
        public string Director { get; set; }
        public string Starred { get; set; }
        public string District { get; set; }
        public string Language { get; set; }
        public List<string> Tags { get; set; }
        public string Mins { get; set; }
        public string Vintage { get; set; }
        public string VodLocalUrl { get; set; }
        public double Rate { get; set; }
        public string Attribute { get; set; }
        public List<string> PosterAddress { get; set; }
        public string CoverAddress { get; set; }
        
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public bool IsTop { get; set; }
        public System.DateTime LastViewTime { get; set; }
    }
}
