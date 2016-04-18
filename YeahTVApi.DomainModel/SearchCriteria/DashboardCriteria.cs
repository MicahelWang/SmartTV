using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class DashboardCriteria : BaseSearchCriteria
    {
        public DateTime? VisitTimeBegin { get; set; }
        public DateTime? VisitTimeEnd { get; set; }
        public string HotelId { get; set; }
        public List<string> HotelList { get; set; }
    }
}
