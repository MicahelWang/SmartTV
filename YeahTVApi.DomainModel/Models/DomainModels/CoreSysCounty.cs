using System;
using System.Collections.Generic;

namespace YeahTVApi.DomainModel.Models
{
    public partial class CoreSysCounty : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int ParentId { get; set; }
    }
}
