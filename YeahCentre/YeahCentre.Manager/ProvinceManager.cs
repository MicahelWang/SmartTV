using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class ProvinceManager : IProvinceManager
    {
        private readonly ISysProvinceRepertory _repertory;
        private readonly IRedisCacheService _redisCacheService;

        public ProvinceManager(ISysProvinceRepertory repertory, IRedisCacheService redisCacheService)
        {
            _repertory = repertory;
            _redisCacheService = redisCacheService;
        }

        private List<CoreSysProvince> Provinces
        {
            get
            {
                if (!_redisCacheService.IsSet(RedisKey.ProvinceKey))
                {
                    _redisCacheService.Add(RedisKey.ProvinceKey, _repertory.GetAll());
                }
                return _redisCacheService.Get<List<CoreSysProvince>>(RedisKey.ProvinceKey);
            }
        }

        public List<CoreSysProvince> GetAll()
        {
            return Provinces;
        }

        public CoreSysProvince GetById(int id)
        {
            return Provinces.FirstOrDefault(m => m.Id == id);
        }
    }
}