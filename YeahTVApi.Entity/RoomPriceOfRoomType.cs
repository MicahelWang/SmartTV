using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
  

    /// <summary>
    /// 房价日历结果对象
    /// </summary>
    public class RoomPriceCalendar : IComparable
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
         public decimal DailyPrice { get; set; }

         public int CompareTo(object obj)
         {
             int currentValue = 1;
             if (MemberLevel == "P")
                 currentValue = 1;
             else if (MemberLevel == "A")
                 currentValue = 2;
             else if (MemberLevel == "B")
                 currentValue = 3;
             else if (MemberLevel == "I")
                 currentValue = 4;

             int TargetValue = 1;
             if (obj != null && obj is RoomPriceCalendar)
             {
                 RoomPriceCalendar target = obj as RoomPriceCalendar;
                 if (target.MemberLevel == "P")
                     TargetValue = 1;
                 else if (target.MemberLevel == "A")
                     TargetValue = 2;
                 else if (target.MemberLevel == "B")
                     TargetValue = 3;
                 else if (target.MemberLevel == "I")
                     TargetValue = 4;
             }
             return currentValue.CompareTo(TargetValue);
         }


    }
}
