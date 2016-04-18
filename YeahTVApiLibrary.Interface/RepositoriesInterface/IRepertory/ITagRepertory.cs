namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;

    public interface ITagRepertory : IBsaeRepertory<Tag>
    {
        List<Tag> SearchALLTagsWithRescource(); 
    }
}
