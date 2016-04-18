using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class HCSCacheVersionCriteria : BaseSearchCriteria
    {
        public string TypeId { get; set; }
        public string PermitionType { get; set; }
    }
}
