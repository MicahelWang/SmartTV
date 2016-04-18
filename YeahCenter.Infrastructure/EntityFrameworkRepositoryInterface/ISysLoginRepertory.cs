using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCenter.Infrastructure
{
    public interface ISysLoginRepertory : IBsaeRepertory<CoreSysLogin>
    {
        CoreSysLogin Login(LoginModel model);
        CoreSysLogin Login(string userId, string password);

        CoreSysLogin GetLoginByUserName(string username);
    }
}
