using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    [Serializable]
    public partial class ErpPowerResource : BaseEntity<int>
    {
        public string Type { get; set; }
        public string Path { get; set; }
        public string DisplayName { get; set; }
        public int ParentFuncId { get; set; }
        public int Orders { get; set; }
        public int Display { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public string Logo { get; set; }
    }
}
