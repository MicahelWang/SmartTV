namespace YeahTVApiLibrary.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;

    /// <summary>
    /// 
    /// </summary>
    public class LocalizeResourceManager : BaseManager<LocalizeResource, LocalizeResourceCriteria>, ILocalizeResourceManager
    {
        private ILocalizeResourceRepertory localizeResourceRepertory;
        private IRedisCacheService redisCacheService;

        public LocalizeResourceManager(ILocalizeResourceRepertory _localizeResourceRepertory,
            IRedisCacheService redisCacheService)
            : base(_localizeResourceRepertory)
        {
            this.localizeResourceRepertory = _localizeResourceRepertory;
            this.redisCacheService = redisCacheService;
        }

        public List<LocalizeResource> SearchLocalizeResources(LocalizeResourceCriteria criteria)
        {
            return localizeResourceRepertory.Search(criteria);
        }

        public void AddLocalizeResources(List<LocalizeResource> resources)
        {
            localizeResourceRepertory.Insert(resources);
        }

        public void AddLocalizeResource(LocalizeResource resource)
        {
            localizeResourceRepertory.Insert(resource);
        }

        public void Update(LocalizeResource resource)
        {
            localizeResourceRepertory.Update(resource);
        }

        public void Delete(LocalizeResource entity)
        {
            localizeResourceRepertory.Delete(m => m.Id == entity.Id && m.Lang == entity.Lang);
        }
        public void Delete(string[] ids)
        {
            localizeResourceRepertory.Delete(m => ids.Contains(m.Id));
        }
    }
}
