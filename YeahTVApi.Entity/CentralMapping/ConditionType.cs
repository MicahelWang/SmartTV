using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public enum ConditionType
    {
        /// <summary>
        /// 其他
        /// </summary>
        Other = 0,
        /// <summary>
        /// 连续入住
        /// </summary>
        ContinuousCheckIn = 1,
        /// <summary>
        /// 提前预定
        /// </summary>
        EarlyCheckIn = 2,
        /// <summary>
        /// 尾房
        /// </summary>
        LastCheckIn = 3
    }

    public static partial class EnumExtensions
    {
        /// <summary>
        /// 根据字符串返回ConditionType枚举值
        /// </summary>
        /// <param name="conditionTypeString"></param>
        /// <returns></returns>
        public static ConditionType ToConditionType(this string conditionTypeString)
        {
            switch (conditionTypeString.ToLower())
            {
                case "co": return ConditionType.ContinuousCheckIn;
                case "ar": return ConditionType.EarlyCheckIn;
                case "lr": return ConditionType.LastCheckIn;
                default: return ConditionType.Other;
            }
        }
    }
}
