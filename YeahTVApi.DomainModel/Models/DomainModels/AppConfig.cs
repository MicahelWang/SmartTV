using System;
using System.Collections.Generic;

namespace YeahTVApi.DomainModel.Models
{
    public partial class AppConfig : BaseEntity<int>
    {
        public string AppId { get; set; }
        public string VersionId { get; set; }
        public string ConfigCode { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
        public bool Active { get; set; }
        public string LastUpdater { get; set; }
        public Nullable<DateTime> LastUpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
