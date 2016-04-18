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
    public class IsAuthorizedAttribute:BusinessAttribute
    {
         /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public IsAuthorizedAttribute()
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



        public Guest Authorized(BaseRequestData data)
        {
            String action=APICallFactory.CallAction(APICallFactory.APICallType.IsAuthorized);
            //完成用户在大促销环境中的登录操作。
            var response = CentralApi.GetResponse(action, null, data.TOKEN, data.language);
            var responseJson = response.Content.ReadAsStringAsync().Result;
            return ConvertTo(responseJson,null) as Guest;
        }


    }
}
