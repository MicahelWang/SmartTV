using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IAuthCertigierManagerRepertory :IBsaeRepertory<AuthCertigierManager>
    {
        bool IsExtisCertigier(string userId, int type);

        AuthCertigierManager GetCertigier(string userId, int type);

        AuthCertigierManager GetCertigierByUserId(string userId, int type);
    }
}
