namespace YeahTVApiLibrary.Manager
{
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;

    /// <summary>
    /// 
    /// </summary>
    public class TVChannelManager : BaseManager<TVChannel, TVChannelCriteria>, ITVChannelManager
    {
        private ITVChannelRepertory tVChannelRepertory;
        private readonly IRedisCacheService _redisCacheService;

        public TVChannelManager(ITVChannelRepertory tVChannelRepertory, IRedisCacheService redisCacheService)
            : base(tVChannelRepertory)
        {
            this.tVChannelRepertory = tVChannelRepertory;
            _redisCacheService = redisCacheService;
        }

        public List<TVChannel> SearchTVChannels(TVChannelCriteria criteria)
        {
            return base.ModelRepertory.Search(criteria);
        }

        public TVChannel GetById(string id)
        {
            //return GetAllFromCache().FirstOrDefault(m => m.Id.Equals(id));
            return base.ModelRepertory.Search(new TVChannelCriteria() { }).FirstOrDefault(m => m.Id.Equals(id));
        }

        public void AddTVChannels(List<TVChannel> tVChannels)
        {
            base.ModelRepertory.Insert(tVChannels);
        }

        public void AddTVChannels(TVChannel tVChannel)
        {
            tVChannel.LastUpdateTime = System.DateTime.Now;
            base.ModelRepertory.Insert(tVChannel);
        }

        public override void Update(TVChannel tVChannel)
        {
            base.ModelRepertory.Update(tVChannel);
        }

        public new void Delete(TVChannel tVChannel)
        {
            base.ModelRepertory.Delete(t => t.Id.Equals(tVChannel.Id));
        }

        public List<TVChannel> SearchChannelsByIds(List<string> keys)
        {
            return tVChannelRepertory.SearchChannelsByIds(keys);
        }
    }
}
