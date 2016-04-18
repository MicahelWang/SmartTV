using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;

namespace YeahCentre.Manager
{
    public class TvTemplateAttributeManager : ITvTemplateAttributeManager
    {
        private readonly ITvTemplateAttributeRepertory _manager;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITvTemplateElementManager _elementManager;
        private readonly ITvDocumentElementCacheManager _tvDocumentElementCacheManager;

        public TvTemplateAttributeManager(ITvTemplateAttributeRepertory manager, IRedisCacheService redisCacheService, ITvTemplateElementManager elementManager
            , ITvDocumentElementCacheManager tvDocumentElementCacheManager)
        {
            _manager = manager;
            _redisCacheService = redisCacheService;
            _elementManager = elementManager;
            _tvDocumentElementCacheManager = tvDocumentElementCacheManager;
        }

        #region Redis

        private IEnumerable<TvTemplateAttribute> GetList()
        {
            if (_redisCacheService.IsSet(RedisKey.TemplateAttributesKey))
                return _redisCacheService.Get<List<TvTemplateAttribute>>(RedisKey.TemplateAttributesKey);
            var elements = _manager.GetAll().OrderBy(m => m.Id).ToList();
            _redisCacheService.Add(RedisKey.TemplateAttributesKey, elements);
            return _redisCacheService.Get<List<TvTemplateAttribute>>(RedisKey.TemplateAttributesKey);
        }

        private void UpdateCache()
        {
            var elements = _manager.GetAll().OrderBy(m => m.Id).ToList();
            if (_redisCacheService.IsSet(RedisKey.TemplateAttributesKey))
                _redisCacheService.Set(RedisKey.TemplateAttributesKey, elements);
            else
            {
                _redisCacheService.Add(RedisKey.TemplateAttributesKey, elements);
            }
            _elementManager.UpdateTemplateElements();

            _redisCacheService.Remove(RedisKey.DocumentAttributesKey);
            _redisCacheService.Remove(RedisKey.DocumentElementsKey);
        }
        #endregion

        public TvTemplateAttribute GetById(string id)
        {
            return GetList().FirstOrDefault(m => m.Id == id);
        }

        public void Delete(string id)
        {
            var attribute = _manager.FindByKey(id);
            var element=_elementManager.GetById(attribute.ElementId);

            _manager.Delete(m => m.Id == id);
            UpdateCache();
            
            _tvDocumentElementCacheManager.RemoveRootNodeCacheByTemplateTypeElementId(element.Id);
        }

        public string Add(TvTemplateAttribute entity)
        {
            CheckEntity(entity);
            entity.Id = Guid.NewGuid().ToString("N").ToUpper();
            _manager.Insert(entity);
            UpdateCache();
            return entity.Id;

        }

        public void Update(TvTemplateAttribute entity)
        {
            CheckEntity(entity);
            var obj = _manager.FindByKey(entity.Id);
            entity.CopyTo(obj, new[] { "Id" });
            _manager.Update(obj);

            if (entity.DataType != 4)
            {
                var list = GetList().Where(p => p.ParentId == entity.Id && p.ElementId == entity.ElementId).ToList();
                var deleteList = list.Select(a => a.Id).ToArray();

                _manager.Delete(m => deleteList.Contains(m.Id));
            }

            UpdateCache();
            _tvDocumentElementCacheManager.RemoveRootNodeCacheByTemplateTypeAttributeId(entity.Id);
        }

        private void CheckEntity(TvTemplateAttribute entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Text))
            {
                throw new ArgumentException("属性名称不能为空！");
            }
            if (AttributeNameIsExist(entity))
            {
                throw new ArgumentException("属性名称已存在！");
            }
        }

        public bool AttributeNameIsExist(TvTemplateAttribute entity)
        {
            var existElements = GetList().Where(e => (e.ElementId == entity.ElementId && (e.Text.Trim().ToLower().Equals(entity.Text.Trim().ToLower()))));
            return string.IsNullOrWhiteSpace(entity.Id) ? existElements.Any() : existElements.Any(e => e.Id != entity.Id);
        }

        public List<TvTemplateAttribute> GetAttributes(string elementId)
        {
            throw new System.NotImplementedException();
        }

        public List<TvTemplateAttribute> GetAttributesByPrentId(string elementId, string parrentId)
        {
            return GetList().Where(p => p.ElementId == elementId && (string.IsNullOrWhiteSpace(p.ParentId) ? string.IsNullOrWhiteSpace(parrentId) : p.ParentId.Equals(parrentId.Trim()))).ToList<TvTemplateAttribute>();
        }
    }
}