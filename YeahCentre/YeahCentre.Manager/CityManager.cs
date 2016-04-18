using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class CityManager : ICityManager
    {
        private readonly ISysCityRepertory _repertory;
        private readonly IRedisCacheService _redisCacheService;

        public CityManager(ISysCityRepertory repertory, IRedisCacheService redisCacheService)
        {
            _repertory = repertory;
            _redisCacheService = redisCacheService;
        }

        private List<CoreSysCity> Citys
        {
            get
            {
                if (!_redisCacheService.IsSet(RedisKey.CityKey))
                {
                    _redisCacheService.Add(RedisKey.CityKey, _repertory.GetAll());
                }
                return _redisCacheService.Get<List<CoreSysCity>>(RedisKey.CityKey);
            }
        }

        public List<CoreSysCity> GetByIds(int[] idArray)
        {
            return Citys.Where(m => idArray.Contains(m.Id)).ToList();
        }

        public List<CoreSysCity> GetAll()
        {
            return Citys;
        }

        public CoreSysCity GetById(int id)
        {
            return Citys.FirstOrDefault(m => m.Id == id);
        }
        public List<CoreSysCity> GetCitysByParentId(int parentId)
        {
            return Citys.Where(m => m.ParentId == parentId).ToList();
        }
    }
}