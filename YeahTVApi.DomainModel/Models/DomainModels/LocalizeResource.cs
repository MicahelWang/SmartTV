using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class LocalizeResource : BaseEntity<string>
    {
        public LocalizeResource()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }
        public string Lang { get; set; } 
        public string Content { get; set; }        
    }
}
