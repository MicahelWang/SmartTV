using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class AuthTVToken : BaseEntity<string>
    {
        public string AuthTicket { get; set; }
        public string AuthToken { get; set; }
        public string Code { get; set; }        
        public int Type { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime ExpireTime { get; set; }
    }
}
