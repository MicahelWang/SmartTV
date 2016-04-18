using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class BrandManager : IBrandManager
    {
        private readonly ISysBrandRepertory _repertory;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ISysGroupRepertory _groupRepertory;

        public BrandManager(ISysBrandRepertory repertory,
            IRedisCacheService cache,
            IRedisCacheService redisCacheService,
            ISysGroupRepertory _groupRepertory)
        {
            this._repertory = repertory;
            _redisCacheService = redisCacheService;
            this._groupRepertory = _groupRepertory;
        }

        private List<CoreSysBrand> Brands
        {
            get
            {
                if (!_redisCacheService.IsSet(RedisKey.BrandKey))
                {
                    _redisCacheService.Add(RedisKey.BrandKey, _repertory.GetAll());
                }
                return _redisCacheService.Get<List<CoreSysBrand>>(RedisKey.BrandKey);
            }
        }

        private void UpdateCache()
        {
            _redisCacheService.Remove(RedisKey.BrandKey);
        }

        public List<CoreSysBrand> GetAll()
        {
            return Brands;
        }
        public CoreSysBrand GetBrand(string brandId)
        {
            return Brands.FirstOrDefault(m => m.Id == brandId);
        }

        public List<CoreSysBrand> GetBrandsByGroup(string groupId)
        {
            return Brands.Where(m => m.GroupId == groupId).ToList();
        }
        public string Add(CoreSysBrand entity)
        {
            if (IsBrandNameExist(entity.GroupId, entity.BrandName))
                throw new Exception("品牌名称已经存在！");
            entity.Id = Guid.NewGuid().ToString("N");
            entity.BrandCode = string.Format("{0}{1}", _groupRepertory.FindByKey(entity.GroupId).GroupCode, (GetAll().Count(m => m.GroupId.ToLower().Equals(entity.GroupId.ToLower()))+1).ToString().PadLeft(3, '0'));
            _repertory.Insert(entity);
            UpdateCache();
            return entity.Id;
        }
        public void Update(CoreSysBrand entity)
        {
            if (IsBrandNameExist(entity.GroupId, entity.BrandName, entity.Id))
                throw new Exception("品牌名称已经存在！");
            var coreSysBrandDb = _repertory.FindByKey(entity.Id);

            coreSysBrandDb.BrandName = entity.BrandName;
            coreSysBrandDb.Logo = entity.Logo;
            coreSysBrandDb.GroupId = entity.GroupId;
            coreSysBrandDb.TemplateId = entity.TemplateId;

            _repertory.Update(coreSysBrandDb);

            UpdateCache();
        }

        private bool IsBrandNameExist(string groupId, string brandName, string excludeId = "")
        {
            return GetAll().Any(m => m.BrandName.ToLower().Equals(brandName.ToLower())  && (string.IsNullOrWhiteSpace(excludeId) || !m.Id.Equals(excludeId)));
        }

        public List<CoreSysBrand> Search(CoreSysBrandCriteria coreSysBrandCriteria)
        {
            return _repertory.Search(coreSysBrandCriteria);
        }
        public void Delete(string brandId)
        {
            _repertory.Delete(m => m.Id == brandId);
            UpdateCache();
        }
    }
}