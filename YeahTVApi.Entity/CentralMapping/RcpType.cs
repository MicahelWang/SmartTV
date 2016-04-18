using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 租用类型
    /// </summary>
    public enum RcpType
    {
        NoSet = 0,
        /// <summary>
        /// 正常(自然日/营业日/一天)
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 长包(一个月/...)
        /// </summary>
        Permanent = 2,
        /// <summary>
        /// 旅游
        /// </summary>
        Travel = 3,
        /// <summary>
        /// 会议
        /// </summary>
        Meeting = 4,
        /// <summary>
        /// 时租4小时
        /// </summary>
        Hour4 = 54,
        /// <summary>
        /// 时租2小时
        /// </summary>
        Hour2 = 52,
        /// <summary>
        /// 时租3小时
        /// </summary> 
        Hour3 = 53,
        /// <summary>
        /// 时租5小时
        /// </summary> 
        Hour5 = 55,
        /// <summary>
        /// 时租9小时
        /// </summary>public const string Hour6 = "RcpType056";
        Hour9 = 59,
        /// <summary>
        /// 自用
        /// </summary>
        Self = 6,
        /// <summary>
        /// 免费
        /// </summary>
        Free = 7
    }

    public static partial class EnumExtensions
    {
        public static string GetID(this RcpType pRcpType)
        {
            switch (pRcpType)
            {
                case RcpType.NoSet:
                case RcpType.Normal:
                    return "RcpType01";
                case RcpType.Permanent:
                    return "RcpType02";
                case RcpType.Travel:
                    return "RcpType03";
                case RcpType.Meeting:
                    return "RcpType04";
                case RcpType.Hour4:
                    return "RcpType05";
                case RcpType.Hour2:
                    return "RcpType052";
                case RcpType.Hour3:
                    return "RcpType053";
                case RcpType.Hour5:
                    return "RcpType055";
                case RcpType.Hour9:
                    return "RcpType059";
                case RcpType.Self:
                    return "RcpType06";
                case RcpType.Free:
                    return "RcpType07";
                default:
                    return "RcpType01";
            }
        }

        public static string Descript(this RcpType pRcpType)
        {
            switch (pRcpType)
            {
                case RcpType.NoSet:
                case RcpType.Normal:
                    return "正常";
                case RcpType.Permanent:
                    return "长包";
                case RcpType.Travel:
                    return "旅游";
                case RcpType.Meeting:
                    return "会议";
                case RcpType.Hour4:
                    return "时租4小时";
                case RcpType.Hour2:
                    return "时租2小时";
                case RcpType.Hour3:
                    return "时租3小时";
                case RcpType.Hour5:
                    return "时租5小时";
                case RcpType.Hour9:
                    return "时租9小时";
                case RcpType.Self:
                    return "自用";
                case RcpType.Free:
                    return "免费";
                default:
                    return "正常";
            }
        }

        public static RcpType ToRcpType(this string pRcpType)
        {
            switch (pRcpType)
            {
                case "RcpType01":
                    return RcpType.Normal;
                case "RcpType02":
                    return RcpType.Permanent;
                case "RcpType03":
                    return RcpType.Travel;
                case "RcpType04":
                    return RcpType.Meeting;
                case "RcpType05":
                    return RcpType.Hour4;
                case "RcpType052":
                    return RcpType.Hour2;
                case "RcpType053":
                    return RcpType.Hour3;
                case "RcpType055":
                    return RcpType.Hour5;
                case "RcpType059":
                    return RcpType.Hour9;
                case "RcpType06":
                    return RcpType.Self;
                case "RcpType07":
                    return RcpType.Free;
                default:
                    return RcpType.Normal;
            }
        }
    }
}
