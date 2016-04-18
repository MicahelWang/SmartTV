using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 查询会员信息结果类
    /// </summary>
    public class QueryMemberResult : OperationResult
    {
        public MemberInfo MemberInfoDetail { get; set; }
         
    }

    public class MemberInfo
    {
        #region 会员个人信息
        /// <summary>
        /// 会员姓名
        /// </summary>
        
        public string Name { get; set; }
        /// <summary>
        /// 会员昵称
        /// </summary>
        
        public string WebName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        
        public string Gender { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 是否永久铂金
        /// </summary>
        
        public bool IsForever { get; set; }

        #endregion

        #region 会员联系方式
        /// <summary>
        /// 邮编
        /// </summary>
        
        public string ZipCode { get; set; }
        
        public string Town { get; set; }
        /// <summary>
        /// 城市编号
        /// </summary>
        
        public string CityCode { get; set; }
        /// <summary>
        /// 国家编号
        /// </summary>
        
        public string CountryCode { get; set; }
        /// <summary>
        /// 省的编号
        /// </summary>
        
        public string ProvinceCode { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        
        public string Address { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        
        public string Email { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        
        public string Fax { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        
        public string Mobile { get; set; }
        

        public string Phone { get; set; }
        #endregion

        #region 系统相关信息
        /// <summary>
        /// 个人主页
        /// </summary>
        
        public string WebSite { get; set; }

        /// <summary>
        /// 修改会员信息必须指定,操作者，网站:website
        /// </summary>
        
        public string OperateBy { get; set; }
        /// <summary>
        /// 修改会员信息必须指定
        /// </summary>
        
        public string ModifyBy { get; set; }
        
        public string VNoHead { get; set; }
        /// <summary>
        /// 会员未读站内信数
        /// </summary>
        
        public int MemberNewSystemNoticeCount { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        
        public string Password { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        
        public string IDType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        
        public string IDNo { get; set; }
        
        public decimal Value { get; set; }
        
        public decimal Point { get; set; }
        
        public string DefaultVCardNo { get; set; }
        
        public string DefaultExtCardNo { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        
        public DateTime RegisterTime { get; set; }
        
        public string SourceChannel { get; set; }
        
        public string SourceDetailCode { get; set; }
        
        public string SourceType { get; set; }
        
        public string StatusCode { get; set; }
        
        public DateTime ExpireTime { get; set; }
        
        public string ExtraFlags { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        
        public string MemberId { get; set; }
        /// <summary>
        /// 会员级别ID
        /// </summary>
        
        public string MemberLevelID { get; set; }
        /// <summary>
        /// 会员级别描述
        /// </summary>
        
        public string MemberLevelDesc { get; set; }
        /// <summary>
        /// BD卡号
        /// </summary>
        
        public string BDCardNo { get; set; }
        /// <summary>
        /// BD卡类型
        /// </summary>
        
        public string BDCardType { get; set; }
        /// <summary>
        /// 客人总积分
        /// </summary>
        
        public decimal CreditPoint { get; set; }
        
        public decimal CreditValue { get; set; }
        #endregion

        #region 注册需要的其他字段
        //注册需要的其他字段
        
        public string HotelID { get; set; }
        
        public string CardType { get; set; }
        
        public string CodeAfterV { get; set; }
        
        public string Remark { get; set; }
        
        public string ActivityId { get; set; }
        
        public string ModifyWay { get; set; }
        
        public string ModifyPrice { get; set; }
        /// <summary>
        /// 已入住间页数
        /// </summary>
        
        public decimal RoomDay { get; set; }
        #endregion

        #region 会员登录相关
        //会员登录相关
        
        public string UserName { get; set; }
        /// <summary>
        /// 调用者
        /// </summary>
        
        public int Caller { get; set; }

        public bool LoginSuccess { get; set; }
        public string LoginMsg { get; set; }

        public CompanyMemberLoginType CompanyMemberLoginType { get; set; }
        #endregion
    }

}
