using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class AppsCriteria : BaseSearchCriteria
    {
        public bool? Active { get; set; }

        public bool? ShowInStroe { get; set; }

        public string Platform { get; set; }

        public string AppName { get; set; }

        public bool NeedVersion { get; set; }

        public int? AppVersion { get; set; }

        public bool? AppVersionActive { get; set; }
        public string PackageName { get; set; }
    }
}
