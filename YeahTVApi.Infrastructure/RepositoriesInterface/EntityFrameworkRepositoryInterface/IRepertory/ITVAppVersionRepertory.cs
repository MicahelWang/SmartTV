namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;

    public interface ITVAppVersionRepertory : IBsaeRepertory<AppVersion>
    {
        string GetLastestAppVersion(string appId, string seriesCode);
    }
}
