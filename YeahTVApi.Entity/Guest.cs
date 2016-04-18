using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 会员实体
    /// </summary>
    public class Guest
    {
   
        public string ReceiveID { get; set; }
      
   
        public string MemberID { get; set; }

        /// <summary>
        /// 主接待单
        /// </summary>
        public string InmateRecID { get; set; }

        /// <summary>
        /// 会员提示
        /// </summary>
        public String MemberHint;


        public Boolean IsCompanyMember()
        {
            return "CDJKRcdjkr".Contains(MemberLevelID);
        }

        
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string VNo { get; set; }
        /// <summary>
        /// 设置的闹钟时间
        /// </summary>
        public string AlarmClock { get; set; }
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
        /// 积分数
        /// </summary>
        public String Point;
        public String idCode;
        public String idType;
        public String sex;
        public String idTypeDesc;
        public String birthDay;
        public String CardCreditValue;
        public String RegisterTime;

        public Boolean IsMember;

        public decimal BreakfastPrice { get; set; }

        public long Points { get; set; }

        public string HotelName { get; set; }

        public DateTime ExpiredTime { get; set; }

        public string SignKey { get; set; }
    }
}
