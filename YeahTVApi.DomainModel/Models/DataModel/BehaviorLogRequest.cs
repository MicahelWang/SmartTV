using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class BehaviorLogRequest
    {
        public string Message { get; set; }

        public string ObjectInfo { get; set; }
    }
    public class BehaviorLogRequestNew
    {
        public object ObjectInfo { get; set; }
        public string BehaviorType { get; set; }
    }
}
