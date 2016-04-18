namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface ITVChannelManager : IBaseManager<TVChannel, TVChannelCriteria>
    {
        List<TVChannel> SearchTVChannels(TVChannelCriteria criteria);

        void AddTVChannels(List<TVChannel> tVChannels);

        void AddTVChannels(TVChannel tVChannel);

        TVChannel GetById(string id);

        List<TVChannel> SearchChannelsByIds(List<string> keys);
    }
}
