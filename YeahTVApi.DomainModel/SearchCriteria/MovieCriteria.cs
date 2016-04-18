using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class MovieCriteria : BaseSearchCriteria
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string MovieReview { get; set; }
        public string MovieReviewEn { get; set; }
        public string PosterAddress { get; set; }
        public string CoverAddress { get; set; }
        public string VideoServerAddress { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public System.DateTime LastViewTime { get; set; }

        public string TemplateTitle { get; set; }
        public string TemplateTags { get; set; }
        public string TemplateDescription { get; set; }
        public double MovieCount { get; set; }
        public double HotelCount { get; set; }
        public string MovieId { get; set; }
        public string MovieTemplateId { get; set; }
    }
}
