using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZTVApi.Entity;
using HZTVApi.Entity.CentralMapping;
using HZTVApi.Common;
using HZ.Web.Authorization;

namespace HZTVApi.Business.BusinessAttributes
{
    /// <summary>
    /// 酒店详情转换类
    /// </summary>
    public class LoginWithoutPasswordAttribute:BusinessAttribute
    {
          /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public LoginWithoutPasswordAttribute()
            : base(typeof(CentralApiResult<User>))
        {
            
        }

        public override Object ConvertTo(string json, Guest guest)
        {
            var result = base.ConvertTo(json, guest);
            if (result == null)
                return null;
            var data = result as CentralApiResult<User>;
            if (data == null)
                return null;
            if (data.ResultType != Entity.CentralMapping.OperationResultType.Successed)
            {
                throw new ApiException(data.Message);
            }


            guest = new Guest();
            guest.MemberID = data.Data.MemberID;
            guest.MemberLevelID = data.Data.MemberLevelID;
            guest.TOKEN = data.Data.TOKEN;
            guest.VNo = data.Data.VNo;
            guest.MemberLevelDesc = data.Data.MemberLevelDesc;
            guest.Mobile = data.Data.Mobile;
            guest.Name = data.Data.Name;
            guest.Email = data.Data.Email;
            return guest;
        }

       
        ///// <summary>
        ///// 登陆方法
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="memberID"></param>
        ///// <param name="type"></param>
        ///// <param name="isReLogin">重新登陆</param>
        ///// <returns></returns>
        //public Guest Login(BaseRequestData data,String memberID, CompanyMemberLoginType type,Boolean isReLogin)
        //{
        //    String action=APICallFactory.CallAction(APICallFactory.APICallType.LoginWithoutPassword);
        //    Dictionary<String, String> pams = new Dictionary<string, string>();
        //    pams.Add("memberId", memberID);
        //    pams.Add("companyMemberLoginType", ((int)type).ToString());
        //    pams.Add("UserToken", data.TOKEN);           
        //    //完成用户在大促销环境中的登录操作。
        //    var response = CentralApi.GetResponse(action, pams,null , data.language);
        //    var responseJson = response.Content.ReadAsStringAsync().Result;
        //    response.Dispose();
        //    response = null;
        //    Guest guest= ConvertTo(responseJson,null) as Guest;
        //    if (guest != null && isReLogin)
        //    {
        //        var dr = base.apiDBManager.LogUserTrace(data, guest.MemberID);
        //    }

        //    else if (guest != null && !isReLogin && data.TOKEN != guest.TOKEN)
        //    {
        //        //HTSendEngine.Sms.SendSms("13917969197", "出现密码不一致情况,旧Token" + data.TOKEN, "token", "app");
                    
        //    }
        //    return guest;
        //}


    }
}
