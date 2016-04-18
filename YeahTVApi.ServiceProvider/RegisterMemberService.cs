namespace YeahTVApi.ServiceProvider
{
    using HZ.Web.Authorization;
    using YeahTVApi.Common;
    using YeahTVApi.Entity;
    using YeahTVApi.Entity.CentralMapping;
    using YeahTVApi.Infrastructure;
    using System;
    using System.Collections.Generic;

    public class RegisterMemberService : CentralGetwayServiceBase, IRegisterMemberService
    {
        public RegisterMemberService()
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


        /// <summary>
        /// 注册会员号
        /// </summary>
        /// <param name="header"></param>
        /// <param name="IDNO"></param>
        /// <param name="IDType"></param>
        /// <param name="Mobile"></param>
        /// <param name="Name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Guest Register(RequestHeader header,String Sex, String IDNO, String IDType, String Mobile, String Name, String Password)
        {
            String action = APICallFactory.CallAction(APICallFactory.APICallType.RegisterMember);
            Dictionary<String, String> pams = new Dictionary<string, string>();
            String Email = null;
            String VNoHead = ChannelFactory.GetVhead(header.Channel);
            pams.Add("IDNO", IDNO);
            pams.Add("IDType", IDType);
            pams.Add("Mobile", Mobile);
            pams.Add("Name", Name);
            pams.Add("Password", Password);
            pams.Add("VNoHead", VNoHead);
            pams.Add("Sex", Sex);
            pams.Add("sign", CentralApi.GetDoubleCheckSign(action, string.Concat(Name
                                                , Mobile
                                                , Password
                                                , Email
                                                , IDType
                                                , IDNO
                                                , VNoHead)));
            //完成用户在大促销环境中的登录操作。
            var response = CentralApi.GetResponse(action, pams, null, header.Language);
            var responseJson = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            response = null;
            return ConvertTo(responseJson, null) as Guest;
        }
    }
}
