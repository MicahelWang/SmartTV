using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel
{
    public static partial class Constant
    {
        public const string GetHotelApiUrl = "api/Hotel/";
        public const string GetTemplateUrl = "api/Template/{0}?templateRootName={1}";
        public const string GetGroupUrl = "api/Group/";
        public const string GetCitiesUrl = "api/City/";
        public const string GetAuthUrl = "api/Auth?username={0}&password={1}";
        public const string GetAuthForAppUrl = "api/AuthForApp?";
        public const string GetCheckAuthUrl = "api/CheckAuth?token={0}&deviceNo={1}";
        public const string GetBrandUrl = "api/Brand/";
        public const string DefaultHostAddress = "239.255.1.";
        public const string GetQtPayCallBackUrl = "api/PayMent/QtPayCallBack";
        public const string VerificationTokenUrl = "api/AuthTVToken/TokenVerification";
        public const string OpenApiGetToken = "/api/AuthTVToken/GetToken";
        public const string VodPaymentCallBackUrl = "api/PayMent/VodPaymentCallBack";
    }
}
