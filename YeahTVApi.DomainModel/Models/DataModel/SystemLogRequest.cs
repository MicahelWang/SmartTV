using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.Models.DataModel
{
   public class SystemLogRequest
    {
       public string Message { get; set; }
       public LogType LogType { get; set; }
       public AppType AppType { get; set; }
       public string MoreInfo { get; set; }
       public DateTime LogCreateTime { get; set; }
       public string MaxAddress { get; set; }
    }
}
