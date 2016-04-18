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
    public class TagManager :BaseManager<Tag,TagCriteria>, ITagManager
    {
        private ITagRepertory tagRepertory;
        private IRedisCacheService redisCacheService;
        private ILocalizeResourceManager resourceManager;
        public TagManager(ITagRepertory _tagRepertory,
            ILocalizeResourceManager resourceManager,
            IRedisCacheService redisCacheService):
            base(_tagRepertory)
        {
            this.tagRepertory = _tagRepertory;
            this.redisCacheService = redisCacheService;
            this.resourceManager = resourceManager;
        }         
        public Tag FindByKey(string movieId)
        {
            return base.ModelRepertory.FindByKey(movieId);
        }         

        public List<Tag> GetALLTagWithLocalizeResource()
        {  
            return tagRepertory.SearchALLTagsWithRescource();
        }
    }
}
