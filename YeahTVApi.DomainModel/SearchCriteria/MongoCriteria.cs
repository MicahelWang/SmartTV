using YeahTVApi.DomainModel.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class MongoCriteria : BaseSearchCriteria
    {
        public LogType? LogType { get; set; }

        public AppType? AppType { get; set; }

        public string Url { get; set; }

        public string Message { get; set; }
        public string MessageEx { get; set; }
         [Display(ResourceType = typeof(Resource.Resource), Name = "Common_LogCriteria_CurrentTime")]
        public DateTime? LogDate { get; set; }
    }
}
