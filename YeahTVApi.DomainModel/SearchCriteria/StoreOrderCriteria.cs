using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class StoreOrderCriteria : BaseSearchCriteria
    {
        public string Hotelid { get; set; }

        public string Orderid { get; set; }

        public int? Status { get; set; }
        public bool? IsDelete { get; set; }

        public string Roomnumber { get; set; }

        public DateTime? Begindate { get; set; }

        public DateTime? Enddate { get; set; }

        public Transactionstate Transactionstate { get; set; }

    }
}
