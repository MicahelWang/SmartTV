using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class VODOrderCriteria : BaseSearchCriteria
    {
        public string OrderId { get; set; }
        public string SeriseCode { get; set; }
        public string RoomNo { get; set; }
        public string HotelId { get; set; }
        public string MovieId { get; set; }
        public OrderState? orderState { get; set; }
        public PayType? payType { get; set; }
        public DateTime? CompleteBeginTime { get; set; }

        public DateTime? CompleteEndTime { get; set; }


        public string CompleteTimeRange { get; set; }
    }
}
