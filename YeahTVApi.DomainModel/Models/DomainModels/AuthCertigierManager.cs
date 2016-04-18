using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class AuthCertigierManager : BaseEntity<string>
    {
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string Token { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime ExpireTime { get; set; }
        public int Type { get; set; }
    }
}
