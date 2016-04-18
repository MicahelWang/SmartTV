namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;

    public interface ITVAppsRepertory : IAppsLibraryRepertory, IBsaeRepertory<Apps>
    {
    }
}
