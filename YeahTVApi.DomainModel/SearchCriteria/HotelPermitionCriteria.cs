using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class HotelPermitionCriteria:BaseSearchCriteria
    {
        public string UserId { get; set; }
        public string PermitionType { get; set; }
        public string TypeId { get; set; }
    }
}
