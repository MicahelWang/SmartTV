using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class Tag : BaseEntity<int>
    {
        public Tag()
        {
            this.LocalizeResources = new List<LocalizeResource>();
        }

        public string RescorceId { get; set; }
        public int ParentId { get; set; } 
        public string Icon { get; set; }
        [NotMapped]
        public virtual IEnumerable<LocalizeResource> LocalizeResources { get; set; }
    }
}
