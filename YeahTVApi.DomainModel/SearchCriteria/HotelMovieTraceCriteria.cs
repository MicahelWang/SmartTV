using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class HotelMovieTraceCriteria : BaseSearchCriteria
    {
        public string HotelId { get; set; }
        public string MovieId { get; set; }
        public string VideoServerAddress { get; set; }
        public Nullable<int> MaxViewCount { get; set; }
        public Nullable<int> MinViewCount { get; set; }
        public bool? IsDownload { get; set; }
        public bool? Active { get; set; }
        public System.DateTime LastViewTime { get; set; }
        public string MoiveTemplateId { get; set; }

        public string MoiveTemplateName { get; set; }
    }
}
