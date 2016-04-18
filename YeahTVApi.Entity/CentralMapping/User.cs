using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 会员实体
    /// </summary>
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        public string MemberID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string VNo { get; set; }
         /// <summary>
        /// 会员级别
        /// </summary>
        public String MemberLevelID { get; set; }

        /// <summary>
        /// 会员级别描述
        /// </summary>
        public String MemberLevelDesc { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String TOKEN { get; set; }

        /// <summary>
        /// 公司卡登录类型
        /// </summary>
        public CompanyMemberLoginType CompanyMemberLoginType { get; set; }



    }

    /// <summary>
    /// 预定类型
    /// </summary>
    public enum CompanyMemberLoginType
    {
        /// <summary>
        /// 没有设定
        /// </summary>
        NoSet = 0,
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 1,
        /// <summary>
        /// 预订员
        /// </summary>
        User = 2
    }



}
