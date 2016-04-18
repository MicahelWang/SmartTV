using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
   public class RequestHCSServerKey
    {
       public string server_id { get; set; }

       public string server_ip { get; set; }

       public string publicKey { get; set; }
    }
}
