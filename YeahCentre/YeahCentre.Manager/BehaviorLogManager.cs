using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public partial class BehaviorLogManager : IBehaviorLogManager
    {
        private readonly IBehaviorLogRepertory _repertory;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IConstantSystemConfigManager _constantSystemConfigManager;

        public BehaviorLogManager(IBehaviorLogRepertory repertory, IRedisCacheService redisCacheService
            , IConstantSystemConfigManager constantSystemConfigManager)
        {
            _repertory = repertory;
            _redisCacheService = redisCacheService;
            _constantSystemConfigManager = constantSystemConfigManager;
        }
        public BehaviorLog GetById(string id)
        {
            return _repertory.FindByKey(id);
        }
        public List<BehaviorLog> Search(LogCriteria criteria)
        {
            return _repertory.Search(criteria);
        }
    }
}