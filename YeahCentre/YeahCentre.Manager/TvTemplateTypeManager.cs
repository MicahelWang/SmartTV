using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Infrastructure.RepositoriesInterface.EntityFrameworkRepositoryInterface.IRepertory.YeahCentre;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class TvTemplateTypeManager : ITvTemplateTypeManager
    {
        private readonly ITvTemplateTypeRepertory _typeRepertory;
        private readonly ITvTemplateElementRepertory _elementRepertory;
        private readonly ITvTemplateAttributeRepertory _attributeRepertory;
        private readonly IRedisCacheService _redisCacheService;



        public TvTemplateTypeManager(ITvTemplateTypeRepertory typeRepertory, IRedisCacheService redisCacheService, ITvTemplateElementRepertory elementRepertory,
            ITvTemplateAttributeRepertory attributeRepertory)
        {
            _typeRepertory = typeRepertory;
            _redisCacheService = redisCacheService;
            _elementRepertory = elementRepertory;
            _attributeRepertory = attributeRepertory;
        }

        private List<TvTemplateType> GetTemplateTypes()
        {
            if (_redisCacheService.IsSet(RedisKey.TemplateTypesKey))
                return _redisCacheService.Get<List<TvTemplateType>>(RedisKey.TemplateTypesKey);
            var types = _typeRepertory.GetAll().OrderBy(m => m.Id).ToList();
            _redisCacheService.Add(RedisKey.TemplateTypesKey, types);
            return _redisCacheService.Get<List<TvTemplateType>>(RedisKey.TemplateTypesKey);
        }

        private void UpdateTemplateTypes()
        {
            var types = _typeRepertory.GetAll().OrderBy(m => m.Id).ToList();
            if (_redisCacheService.IsSet(RedisKey.TemplateTypesKey))
                _redisCacheService.Set(RedisKey.TemplateTypesKey, types);
            else
            {
                _redisCacheService.Add(RedisKey.TemplateTypesKey, types);
            }
            _redisCacheService.Remove(RedisKey.TemplatesKey);
        }

        public List<TvTemplateType> GetAll()
        {
            return GetTemplateTypes();
        }

        public TvTemplateType GetById(int id)
        {
            return GetTemplateTypes().Find(m => m.Id == id);
        }

        public TvTemplateType GetByName(string name)
        {
            return GetTemplateTypes().Find(m => m.Name == name);
        }
        public void Delete(int id)
        {

            _typeRepertory.Delete(m => m.Id == id);
            UpdateTemplateTypes();
        }

        public int Add(TvTemplateType entity)
        {
            _typeRepertory.Insert(entity);
            UpdateTemplateTypes();
            return entity.Id;
        }

        public int AddWithBaseNode(TvTemplateType entity)
        {
            _typeRepertory.Insert(entity);
            UpdateTemplateTypes();
            _redisCacheService.Remove(RedisKey.TemplateElementsKey);
            _redisCacheService.Remove(RedisKey.TemplateAttributesKey);
            return entity.Id;
        }

        public void Update(TvTemplateType entity)
        {
            var type = _typeRepertory.FindByKey(entity.Id);
            type.Name = entity.Name;
            type.Description = entity.Description;
            _typeRepertory.Update(type);
            UpdateTemplateTypes();

        }

        public void BatchDelete(int[] ids)
        {
            _typeRepertory.Delete(m => ids.Contains(m.Id));
            UpdateTemplateTypes();
        }
        /// <summary>
        /// 批量删除模板结构，及子元素
        /// </summary>
        /// <param name="ids"></param>
        public void BatchDeleteWithChilds(int[] ids)
        {
            //TODO:使用事物级联删除
            ids.AsParallel().ForAll(id => DeleteElements(id));

            //批量删除模板
            _typeRepertory.Delete(m => ids.Contains(m.Id));

            UpdateTemplateTypes();
        }
        /// <summary>
        /// 根据模板ID 删除元素及每个元素的属性
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public void DeleteElements(int templateTypeId)
        {
            var elements = _elementRepertory.GetAll().Where(e => e.TemplateType == templateTypeId).ToList();
            var attributeIds = new List<string>();
            var elementIds = new List<string>();
            if (elements.Count != 0)
            {
                elements.ToList().ForEach(e =>
                {
                    e.Attributes.ToList().ForEach(a => attributeIds.Add(a.Id));
                    elementIds.Add(e.Id);
                });
                //批量删除属性
                _attributeRepertory.BatchDelete(attributeIds.ToArray());
                //批量删除元算
                _elementRepertory.BatchDelete(elementIds.ToArray());
            }
        }

        public IPagedList<TvTemplateType> PagedList(int pageIndex, int pageSize, string keyword)
        {
            var pageList = new PagedList<TvTemplateType>(GetTemplateTypes(), pageIndex, pageSize);
            return pageList;
        }
    }
}