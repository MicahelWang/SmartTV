using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class HotelMovieTraceNoTemplateCriteria : BaseSearchCriteria
    {
        public string HotelId { get; set; }
        public string MovieId { get; set; }
        public string DownloadStatus { get; set; }
        public string CategoryId { get; set; }
        public bool ?Active { get; set; }
        public bool ?IsDelete { get; set; }

    }
}
