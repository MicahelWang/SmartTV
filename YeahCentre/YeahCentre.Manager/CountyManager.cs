using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Infrastructure;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class CountyManager : ICountyManager
    {
        private readonly ISysCountyRepertory _repertory;
        private readonly IRedisCacheService _redisCacheService;

        public CountyManager(ISysCountyRepertory repertory, IRedisCacheService redisCacheService)
        {
            _repertory = repertory;
            _redisCacheService = redisCacheService;
        }

        private List<CoreSysCounty> Countys
        {
            get
            {
                if (!_redisCacheService.IsSet(RedisKey.CountyKey))
                {
                    _redisCacheService.Add(RedisKey.CountyKey, _repertory.GetAll());
                }
                return _redisCacheService.Get<List<CoreSysCounty>>(RedisKey.CountyKey);
            }
        }

        public List<CoreSysCounty> GetAll()
        {
            return Countys;
        }
        public List<CoreSysCounty> GetCountysByParentId(int parentId)
        {
            return Countys.Where(m => m.ParentId == parentId).ToList();
        }

        public CoreSysCounty GetById(int id)
        {
            return Countys.FirstOrDefault(m => m.Id == id);
        }
    }
}