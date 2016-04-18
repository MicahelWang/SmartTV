using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    #region Result
    public class QueryDailyRoomPriceResult
    {
        //public List<RoomPriceCalendar> PriceCalender { get; set; }
    }

    public class QueryDailyRoomPriceCommand
    {
        private QueryDailyRoomPriceResult result;
        internal QueryDailyRoomPriceResult Result
        {
            get
            {
                return result ?? (result = new QueryDailyRoomPriceResult());
            }
        }

        public string HotelID { get; set; }
        public string RoomTypeID { get; set; }
        public List<string> MemberLevelList { get; set; }
        public string ActivityID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CurrentMemberLevel { get; set; }

        public Src Src { get; set; }
        public RcpType RcpTypeID { get; set; }
        public string CusCategory { get; set; }


    }

    #endregion
}
