namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Entity;
    using System.Collections.Generic;
    using YeahTVApiLibrary.Infrastructure;
    using YeahTVApi.DomainModel.SearchCriteria;

    public interface IAuthUserDeviceTraceManager
    {
        List<AuthUserDeviceTrace> SearhAuthUserDeviceTrace(AuthUserDeviceTraceCriteria authUserDeviceTraceCriteria);

        void AddAuthUserDeviceTrace(AuthUserDeviceTrace authUserDeviceTrace);

        void UpdateAuthUserDeviceTrace(AuthUserDeviceTrace authUserDeviceTrace);

        void DeleteAuthUserDeviceTrace(AuthUserDeviceTrace authUserDeviceTrace);

        AuthUserDeviceTrace GetEntity(string id);

        AuthUserDeviceTrace GetAuthUserDeviceTrace(AuthUserDeviceTraceCriteria criteria);
    }
}
