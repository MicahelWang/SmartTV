using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
   public class AppVersionCriteria:BaseSearchCriteria
    {
       public bool? Active { get; set; }

       public int? VersionCode { get; set; }

       public DateTime? CreateTime { get; set; }

       public string Id { get; set; }
    }
}
