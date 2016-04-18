namespace YeahTVApiLibrary.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface ITagManager:IBaseManager<Tag,TagCriteria>
    {  
        Tag FindByKey(string movieId);
        [Cache(CacheTime = 24*60)]
        List<Tag> GetALLTagWithLocalizeResource();
    }
}
