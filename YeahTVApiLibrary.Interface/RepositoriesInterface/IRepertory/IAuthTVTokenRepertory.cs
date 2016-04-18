using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IAuthTVTokenRepertory : IBsaeRepertory<AuthTVToken>
    {
        bool IsExtisCertigier(string authTicket, int type);
        AuthTVToken CheckToken(RequestTokenParameter criteria);
    }
}
