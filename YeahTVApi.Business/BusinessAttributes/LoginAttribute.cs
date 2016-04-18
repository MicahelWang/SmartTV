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
    public class LoginAttribute:BusinessAttribute
    {
        /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public LoginAttribute()
            : base(typeof(CentralApiResult<User>))
        {
            
        }

        public override object ConvertTo(string json, Guest guest)
        {
            var result = base.ConvertTo(json, guest);
            if (result == null)
                return null;
            var data = result as CentralApiResult<User>;
            if (data == null)
                return null;
            if (data.ResultType != Entity.CentralMapping.OperationResultType.Successed)
            {
               // HTinns.HTLog4net.Log(this.ToString(), data.Message+"", json);
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



        public Guest Login(BaseRequestData data, String account, String password)
        {
            String action = APICallFactory.CallAction(APICallFactory.APICallType.Login);
            Dictionary<String, String> pams = new Dictionary<string, string>();
            pams.Add("account", account);
            pams.Add("password", password);
            pams.Add("sign", CentralApi.GetDoubleCheckSign(action, account + password));

            DateTime StartTime = DateTime.Now;
            //完成用户在大促销环境中的登录操作。
            var response = CentralApi.GetResponse(action, pams, null, data.language);
            var responseJson = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            response = null;
            DateTime EndTime = DateTime.Now;
            var guest= ConvertTo(responseJson,null);
            DateTime ProcessEndTime = DateTime.Now;
            return guest as Guest;
        }


    }
}
