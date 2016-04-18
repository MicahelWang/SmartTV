using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class VODRecord : BaseEntity<string>
    {
        public string MovieId { get; set; }
        public string ViewDate { get; set; }
        public string ChargeType { get; set; }
        public string ChargeNo { get; set; }
    }
}
