namespace YeahTVApiLibrary.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface ILocalizeResourceManager : IBaseManager<LocalizeResource, LocalizeResourceCriteria>
    {
        List<LocalizeResource> SearchLocalizeResources(LocalizeResourceCriteria criteria);
        [UnitOfWork]
        void AddLocalizeResources(List<LocalizeResource> resources);
        [UnitOfWork]
        void AddLocalizeResource(LocalizeResource resource);
        void Update(LocalizeResource resource);
        [UnitOfWork]
        void Delete(LocalizeResource resource);
        [UnitOfWork]
        void Delete(string[] ids);
    }
}
