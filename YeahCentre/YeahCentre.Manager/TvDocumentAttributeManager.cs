using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class TvDocumentAttributeManager : ITvDocumentAttributeManager
    {

        private readonly ITvDocumentAttributeRepertory _docAttributeRepertory;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITvDocumentElementRepertory _docElementRepertory;
        private readonly ITvTemplateManager _templateManager;
        private readonly ITvDocumentElementManager _docElementManager;
        private readonly ITvDocumentElementCacheManager _tvDocumentElementCacheManager;

        public TvDocumentAttributeManager(ITvDocumentAttributeRepertory docAttributeRepertory, IRedisCacheService redisCacheService, ITvDocumentElementRepertory documentRepertory
            , ITvTemplateManager templateManager, ITvDocumentElementManager docElementManager, ITvDocumentElementCacheManager tvDocumentElementCacheManager)
        {
            _docAttributeRepertory = docAttributeRepertory;
            _redisCacheService = redisCacheService;
            _docElementRepertory = documentRepertory;
            _templateManager = templateManager;
            _docElementManager = docElementManager;
            _tvDocumentElementCacheManager = tvDocumentElementCacheManager;
        }

        #region Redis

        private IEnumerable<TvDocumentAttribute> GetList()
        {
            if (_redisCacheService.IsSet(RedisKey.DocumentAttributesKey))
                return _redisCacheService.Get<List<TvDocumentAttribute>>(RedisKey.DocumentAttributesKey);
            var elements = _docAttributeRepertory.GetAllWithAttributes().ToList();
            _redisCacheService.Add(RedisKey.DocumentAttributesKey, elements);
            return _redisCacheService.Get<List<TvDocumentAttribute>>(RedisKey.DocumentAttributesKey);
        }

        private void UpdateCache()
        {
            var elements = _docAttributeRepertory.GetAllWithAttributes().ToList();
            if (_redisCacheService.IsSet(RedisKey.DocumentAttributesKey))
                _redisCacheService.Set(RedisKey.DocumentAttributesKey, elements);
            else
            {
                _redisCacheService.Add(RedisKey.DocumentAttributesKey, elements);
            }
            _templateManager.UpdateTemplates();
            _docElementManager.UpdateDocumentElements();
        }
        #endregion

        public TvDocumentAttribute GetById(string id)
        {
            return GetList().Where(p => p.Id == id).FirstOrDefault();
        }

        public void Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public string Add(TvDocumentAttribute entity)
        {
            throw new System.NotImplementedException();
        }

        public void Update(TvDocumentAttribute entity)
        {
            CheckEntity(entity);
            var dto = _docAttributeRepertory.FindByKey(entity.Id);
            dto.Value = entity.Value;
            _docAttributeRepertory.Update(dto);

            var element = _docElementManager.GetById(dto.ElementId);
            if (element != null)
            {
                _tvDocumentElementCacheManager.RemoveRootNodeCache(element.TemplateId, dto.ElementId);
                _tvDocumentElementCacheManager.AddDocumentRootElementCache(element.TemplateId, dto.ElementId);
            }

            UpdateCache();

        }
        private void CheckEntity(TvDocumentAttribute entity)
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
        public bool AttributeNameIsExist(TvDocumentAttribute entity)
        {
            var existElements = GetList().Where(e => (e.ElementId == entity.ElementId && (e.Text.Trim().ToLower().Equals(entity.Text.Trim().ToLower()))));
            return string.IsNullOrWhiteSpace(entity.Id) ? existElements.Any() : existElements.Any(e => e.Id != entity.Id);
        }

        public void BatchDelete(string[] ids)
        {
            throw new System.NotImplementedException();
        }

        public List<TvDocumentAttribute> GetAttributes(string elementId)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 获取带有结构对象的属性
        /// </summary>
        /// <returns></returns>
        public List<TvDocumentAttribute> GetAllWithAttributes()
        {
            return GetList().ToList();
        }


        public List<TvDocumentAttribute> GetAttributesByPrentId(string elementId, string parrentId)
        {
            return GetList().Where(p => p.ElementId == elementId && (string.IsNullOrWhiteSpace(p.ParentId) ? string.IsNullOrWhiteSpace(parrentId) : p.ParentId.Equals(parrentId.Trim()))).ToList<TvDocumentAttribute>();
        }

    }
}