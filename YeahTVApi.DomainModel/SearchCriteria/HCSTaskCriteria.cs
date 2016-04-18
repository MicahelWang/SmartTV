using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class HCSTaskCriteria : BaseSearchCriteria
    {
        public string ServerId { get; set; }

        public string JobId { get; set; }

        public string Type { get; set; }

        public bool NotSuccessResultStatus { get; set; }

        public string TaskNo { get; set; }
    }
}
