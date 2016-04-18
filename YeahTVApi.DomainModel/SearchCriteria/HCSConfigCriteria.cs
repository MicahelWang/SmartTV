using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class HCSConfigCriteria : BaseSearchCriteria
    {
        public string ServerId { get; set; }

        public string Type { get; set; }
    }
}
