using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public enum OrderStatus
    {
        /// <summary>
        /// 未设置
        /// </summary>
        NoSet = 0,
        /// <summary>
        /// 预订中
        /// </summary>
        Reserving = 1,
        /// <summary>
        /// 完成
        /// </summary>
        Completed = 2,
        /// <summary>
        /// 预订未到
        /// </summary>
        NoShow = 3,
        /// <summary>
        /// 取消
        /// </summary>
        Canceled = 4,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 5,
    }

    public static partial class EnumExtensions
    {
        public static string GetID(this OrderStatus pOrderStatus)
        {
            switch (pOrderStatus)
            {

                case OrderStatus.Reserving:
                    return "R";
                case OrderStatus.Completed:
                    return "E";
                case OrderStatus.NoShow:
                    return "N";
                case OrderStatus.Canceled:
                    return "X";
                case OrderStatus.Delete:
                    return "D";
                default:
                    return string.Empty;
            }
        }

        public static string Descript(this OrderStatus pOrderStatus)
        {
            switch (pOrderStatus)
            {
                case OrderStatus.Reserving:
                    return "预订中";
                case OrderStatus.Completed:
                    return "完成";
                case OrderStatus.NoShow:
                    return "NoShow";
                case OrderStatus.Canceled:
                    return "取消";
                case OrderStatus.Delete:
                    return "删除";
                default:
                    return string.Empty;
            }
        }

        public static OrderStatus ToOrderStatus(this string pOrderStatus)
        {
            if (string.IsNullOrEmpty(pOrderStatus)) {
                return OrderStatus.NoSet;
            }

            switch (pOrderStatus.ToUpper())
            {
                case "R":
                    return OrderStatus.Reserving;
                case "E":
                    return OrderStatus.Completed;
                case "N":
                    return OrderStatus.NoShow;
                case "X":
                    return OrderStatus.Canceled;
                case "D":
                    return OrderStatus.Delete;
                default:
                    return OrderStatus.NoSet;
            }
        }
    }
}
