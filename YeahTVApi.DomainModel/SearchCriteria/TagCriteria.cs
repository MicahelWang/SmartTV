using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class TagCriteria : BaseSearchCriteria
    {
        public string RescorceId { get; set; }
        public int? ParentId { get; set; }         
    }
}
