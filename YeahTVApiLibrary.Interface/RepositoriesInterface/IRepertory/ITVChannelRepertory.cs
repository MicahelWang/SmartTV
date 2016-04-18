namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;

    public interface ITVChannelRepertory : IBsaeRepertory<TVChannel>
    {
        List<TVChannel> SearchChannelsByIds(List<string> keys);
    }
}
