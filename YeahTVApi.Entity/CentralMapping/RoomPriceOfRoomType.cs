using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class RoomPriceOfRoomType
    {
        /// <summary>
        /// 当前查询的活动编号
        /// </summary>
        public string ActivityID { get; set; }
        public List<RoomPriceCalendar> RoomPriceCalendars { get; set; }

    }

    /// <summary>
    /// 房价日历结果对象
    /// </summary>
    public class RoomPriceCalendar
    {
        /// <summary>
        /// 会员级别
        /// </summary>
        public string MemberLevel { get; set; }
        /// <summary>
        /// 会员级别描述
        /// </summary>
        public string MemberLevelDescript { get; set; }

        /// <summary>
        /// 会员级别对应的每日价格
        /// </summary>
        public List<DailyRoomPriceOfMemberLevel> DailyRoomPriceOfMemberList { get; set; }
    }

    /// <summary>
    /// 会员级别对应的每日价格
    /// </summary>
    public class DailyRoomPriceOfMemberLevel
    {
        public DateTime? BizDate { get; set; }
        public decimal Price { get; set; }
    }

    public class RoomPriceOfRoomTypeResult : OperationResult
    {
        public List<RoomPriceOfRoomType> RoomPriceOfRoomTypeList { get; set; }
    }





}
