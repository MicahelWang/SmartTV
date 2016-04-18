using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Web.Utility
{
    public class SiteSecurityHelper
    {
        public static void Logout()
        {
            var session = HttpContext.Current.Session;
            if (session != null)
            {
                session.Clear();
            }
            FormsAuthentication.SignOut();
        }

        public static CurrentUser GetCurrentUserData(LoginModel loginModel, ref string errorStr)
        {
            var usersService = DependencyResolver.Current.GetService<IUserManager>();
            var constantSystemConfigManager = DependencyResolver.Current.GetService<IConstantSystemConfigManager>();
            var requestApiService = DependencyResolver.Current.GetService<IRequestApiService>();
            var password = loginModel.Password;

            var loginResult = usersService.Login(loginModel);
            if (loginResult == null)
            {
                errorStr = "账户信息错误！";
                return null;
            }
            var loginState = (StateEnum) loginResult.State;
            if (loginState!=StateEnum.Normal)
            {
                const string errorStrFormat = "当前账户状态为{0}，禁止登陆！";
                errorStr = string.Format(errorStrFormat, loginState.GetDescription());
                return null;
            }
            //获取User表数据
            var loginUserDto = usersService.GetCurrentUserData(loginModel.UserName);
            if (loginUserDto == null)
            {
                errorStr = "账户信息错误！";
            }

            var url = string.Format(constantSystemConfigManager.OpenApiAddress + Constant.GetAuthForAppUrl + "userName={0}&password={1}", loginModel.UserName, password);//
            var response = requestApiService.HttpRequest(url, "Post");
            var objs = response.JsonStringToObj<MsgResult>();//AuthModel

            if (!objs.HasError)
            {
                var authModel = objs.Data.ToString().JsonStringToObj<AuthModel>();

                loginUserDto.Token = authModel.Token;
            }

            return loginUserDto;
        }
    }
}
