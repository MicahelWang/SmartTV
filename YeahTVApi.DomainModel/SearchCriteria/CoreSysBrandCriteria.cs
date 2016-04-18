using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public partial class CoreSysBrandCriteria : BaseSearchCriteria
    {
        public int HotelId { get; set; }
        public string GroupId { get; set; }
        public string BrandName { get; set; }
    }
}
