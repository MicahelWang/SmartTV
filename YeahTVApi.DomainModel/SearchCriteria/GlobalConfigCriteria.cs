using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class GlobalConfigCriteria : BaseSearchCriteria
    {
        public string PermitionType { get; set; }
        public string TypeId { get; set; }
    }
    public class GlobalConfigSearchInfo
    {
        public GlobalConfigType GlobalConfigType { get; set; }
        public string TypeId { get; set; }
        public string ConfigName { get; set; }
    }
}
