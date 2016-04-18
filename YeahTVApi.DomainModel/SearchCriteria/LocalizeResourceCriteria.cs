using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class LocalizeResourceCriteria : BaseSearchCriteria
    {
        public string Lang { get; set; }
        public string Content { get; set; }
    }
}
