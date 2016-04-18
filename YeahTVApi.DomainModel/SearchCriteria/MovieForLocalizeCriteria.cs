using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class MovieForLocalizeCriteria : BaseSearchCriteria
    {
        public MovieForLocalizeCriteria()
        { HotelCount = -1; }
        public string Name { get; set; }
        public string TagId { get; set; }
        public bool IsTop { set; get; }
        public int HotelCount { get; set; }
        public bool? DistributeAll { get; set; }
    }
}
