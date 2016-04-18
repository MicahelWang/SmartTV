using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 来源渠道
    /// </summary>
    public enum Src
    {
        /// <summary>
        /// 未设置
        /// </summary>
        NoSet = 0,
        /// <summary>
        /// 酒店(PMS前端)
        /// </summary>
        Hotel = 1,
        /// <summary>
        /// CallCenter
        /// </summary>
        CRS = 2,
        /// <summary>
        /// 网站
        /// </summary>
        WEB = 3,
        /// <summary>
        /// 未知？
        /// </summary>
        GDS = 4,
        /// <summary>
        /// 中介
        /// </summary>
        ETT = 5,
        /// <summary>
        /// WAP/Android/iPhone
        /// </summary>
        APP = 6,
        /// <summary>
        /// 门店PAD
        /// </summary>
        Pad1 = 7,
        /// <summary>
        /// 门店PAD(还有无用?)
        /// </summary>
        Pad2 = 8,
        /// <summary>
        /// 其他
        /// </summary>
        OTH = 99
    }

    public static partial class EnumExtensions {
        public static string GetID(this Src pSrc) {
            switch (pSrc) { 
                case Src.NoSet:
                case Src.WEB:
                    return "Src03";
                case Src.Hotel:
                    return "Src01";
                case Src.CRS:
                    return "Src02";
                case Src.GDS:
                    return "Src04";
                case Src.ETT:
                    return "Src05";
                case Src.APP:
                    return "Src06";
                case Src.Pad1:
                    return "Src07";
                case Src.Pad2:
                    return "Src08";
                case Src.OTH:
                    return "Src99";
                default:
                    return "Src03";
            }
        }
        /// <summary>
        /// 将字符串转换为Src枚举
        /// </summary>
        /// <param name="pSrcDesc"></param>
        /// <returns></returns>
        public static Src ToSrc(this string pSrcDesc) {
            switch (pSrcDesc.ToLower())
            {
                case "src01":
                    return Src.Hotel;
                case "src02":
                    return Src.CRS;
                case "src03":
                    return Src.WEB;
                case "src04":
                    return Src.GDS;
                case "src05":
                    return Src.ETT;
                case "src06":
                    return Src.APP;
                case "src07":
                    return Src.Pad1;
                case "src08":
                    return Src.Pad2;
                case "src099":
                    return Src.OTH;
                default:
                    return Src.WEB;
            }
        }
    }
}
