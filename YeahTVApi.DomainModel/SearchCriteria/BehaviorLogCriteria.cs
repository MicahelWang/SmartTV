using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class LogCriteria : BaseSearchCriteria
    {
        public string LogInfo { get; set; }
        public string LogInfoEx { get; set; }
        public string LogType { get; set; }
        public string AppType { get; set; }
        public string AppId { get; set; }
        public string HotelId { get; set; }
        public BehaviorType? BehaviorType { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? CompleteBeginTime { get; set; }
        public DateTime? CompleteEndTime { get; set; }

    }
}
