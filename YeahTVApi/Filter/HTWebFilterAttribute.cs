namespace YeahTVApi.Filter
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Entity;
    using YeahTVApi.Infrastructure;
    using YeahTVApiLibrary.Infrastructure;
    using Microsoft.Practices.Unity;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using YeahTVApiLibrary.Filter;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;


    [AttributeUsage(AttributeTargets.Method)]
    public class HTWebFilterAttribute : HTApiFilterAttribute
    {
        [Dependency]
        public IConstantSystemConfigManager ConstantSystemConfigManager { get; set; }

        public override void SetGuestMemberInfo(ActionExecutingContext filterContext, RequestHeader header)
        {
            //var result = RequestApiService.Get(string.Format("http://localhost:8088/" + Constant.GetCheckAuthUrl, header.Token, header.DEVNO));
            var result = RequestApiService.Get(string.Format(ConstantSystemConfigManager.OpenApiAddress + Constant.GetCheckAuthUrl, header.Token, header.DEVNO));
            //if (result.Contains("\"HasError\": true"))
              //  throw new ApiException("登陆已经过期，请重新登陆.");
            var objs = result.JsonStringToObj<MsgResult>();
            if (objs.HasError)
            {
                throw new ApiException(ApiErrorType.NotLogin,objs.Msg);
            }

            var obj = JObject.Parse(JsonConvert.SerializeObject(objs.Data));
            header.Guest = obj.GetValue("UserName").ToString().Trim();
        }

        public override void CheckBindDevice(HttpContextBase context, RequestHeader header)
        {

        }
    }
}