using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class AppPublishCriteria : BaseSearchCriteria
    {
        public bool? Active { get; set; }

        public string HotelId { get; set; }

        public string AppId { get; set; }

        public int? VersionCode { get; set; }

        public DateTime? PublishTime { get; set; }
    }
}
