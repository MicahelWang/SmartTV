using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{

    public enum UserTypeEnum
    {
        /// <summary>
        /// 客户
        /// </summary>
        [Description("行悦用户")]
        Customer = 1,
        /// <summary>
        /// 内部员工
        /// </summary>
        [Description("酒店用户")]
        Employee = 2

    }

    public enum PermitionEnum
    {
        [Description("集团权限")]
        Group = 0,
        [Description("品牌权限")]
        Brand = 1,
        [Description("店面权限")]
        Hotel = 2
    }

    public enum StateEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,
        /// <summary>
        /// 未激活
        /// </summary>
        //[Description("未激活")]
        //InActive = 2,
        ///// <summary>
        ///// 休眠
        ///// </summary>
        //[Description("休眠")]
        //Dormancy = 3,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disable = 4
    }
}
