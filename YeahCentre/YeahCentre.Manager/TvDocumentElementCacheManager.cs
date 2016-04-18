using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class TvDocumentElementCacheManager : ITvDocumentElementCacheManager
    {
        private readonly ITvDocumentAttributeRepertory _docAttributeRepertory;
        private readonly ITvDocumentElementRepertory _docElementRepertory;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITvTemplateAttributeRepertory _typeAttributeRepertory;
        private readonly ITvTemplateElementRepertory _typeElementRepertory;

        public TvDocumentElementCacheManager(ITvDocumentAttributeRepertory docAttributeRepertory, IRedisCacheService redisCacheService
            , ITvDocumentElementRepertory docElementRepertory
            , ITvTemplateAttributeRepertory typeAttributeRepertory
            , ITvTemplateElementRepertory typeElementRepertory)
        {
            _docAttributeRepertory = docAttributeRepertory;
            _redisCacheService = redisCacheService;
            _docElementRepertory = docElementRepertory;
            _typeAttributeRepertory = typeAttributeRepertory;
            _typeElementRepertory = typeElementRepertory;
        }

        private string GetCacheKey(string templateId, string elementName)
        {
            return string.Format("{0}_{1}_{2}", RedisKey.TemplatesRootKey, templateId, elementName.ToLower());
        }

        private void UpdateCacheByTvTemplateElement(TvTemplateElement element)
        {
            if (element != null)
            {
                var docElements = _docElementRepertory.GetAll().Where(m => m.TemplateElementId.Equals(element.Id));
                var elements = _docElementRepertory.GetAll();
                docElements.ToList().AsParallel().ForAll(m =>
                {
                    var docElement = GetRootElementByElementId(m.TemplateId, m.Id, elements);
                    if (docElement == null) return;
                    RemoveRootNodeCache(docElement.TemplateId, docElement.Id);
                    AddDocumentRootElementCache(docElement.TemplateId, docElement.Id);
                });
            }
        }

        private TvDocumentElement GetRootElementByElementId(string templateId, string elementId, List<TvDocumentElement> elements = null)
        {
            if (elements == null) elements = _docElementRepertory.GetAll().Where(m => m.TemplateId.Equals(templateId)).ToList();

            var element = elements.FirstOrDefault(m => m.Id.Equals(elementId));

            if (element == null) return null;

            return string.IsNullOrWhiteSpace(element.ParentId) ? element : GetRootElementByElementId(templateId, element.ParentId, elements);
        }

        private void GetChildElements(TvDocumentElement element, List<TvDocumentElement> result, List<TvDocumentElement> elements = null)
        {
            if (elements == null) elements = _docElementRepertory.GetAll().Where(m => m.TemplateId.Equals(element.TemplateId)).ToList();
            var childs = elements.Where(m => !string.IsNullOrWhiteSpace(m.ParentId) && m.ParentId.Equals(element.Id)).ToList();

            if (childs.Count <= 0) return;

            result.AddRange(childs);
            childs.ForEach(m => GetChildElements(m, result, elements));
        }

        public void RemoveDocumentRootElementCacheByElementName(string templateId, string elementName)
        {
            _redisCacheService.Remove(GetCacheKey(templateId, elementName));
        }

        public void AddDocumentRootElementCache(string templateId, string elementId)
        {
            var element = GetRootElementByElementId(templateId, elementId);
            if (element != null)
            {
                var result = new List<TvDocumentElement> { element };

                GetChildElements(element, result);

                _redisCacheService.Add(GetCacheKey(templateId, element.Name), result);
            }
        }

        public void RemoveTemplateRootNodeCache(string templateId)
        {
            var keys = _redisCacheService.GetAllKeys();
            var prefix = string.Format("{0}_{1}", RedisKey.TemplatesRootKey, templateId).ToLower();
            keys.ForEach(m =>
            {
                if (m.ToLower().StartsWith(prefix))
                    _redisCacheService.Remove(m);
            });
        }

        public void RemoveRootNodeCache(string templateId, string elementId)
        {
            var element = GetRootElementByElementId(templateId, elementId);
            if (element != null)
                _redisCacheService.Remove(GetCacheKey(templateId, element.Name));
        }

        public void RemoveRootNodeCacheByTemplateTypeAttributeId(string templateTypeAttributeId)
        {
            var attribute = _typeAttributeRepertory.FindByKey(templateTypeAttributeId);
            if (attribute != null)
            {
                var element = _typeElementRepertory.FindByKey(attribute.ElementId);
                if (element != null)
                {
                    UpdateCacheByTvTemplateElement(element);
                }
            }
        }

        public void RemoveRootNodeCacheByTemplateTypeElementId(string templateTypeElementId)
        {
            var element = _typeElementRepertory.FindByKey(templateTypeElementId);
            UpdateCacheByTvTemplateElement(element);
        }

        public List<TvDocumentElement> GetRootNodeFromCache(string templateId, string name)
        {
            var key = GetCacheKey(templateId, name);
            if (_redisCacheService.ContainsKey(key))
                return _redisCacheService.Get<List<TvDocumentElement>>(key);

            var element = _docElementRepertory.GetAll().FirstOrDefault(m => m.TemplateId.Equals(templateId) && m.Name.ToLower().Equals(name.ToLower()) && string.IsNullOrWhiteSpace(m.ParentId));
            if (element != null)
                AddDocumentRootElementCache(templateId, element.Id);
            return _redisCacheService.Get<List<TvDocumentElement>>(key);
        }

        public void SetTemplateRootElementCache(string templateId)
        {
            if (string.IsNullOrWhiteSpace(templateId)) return;
            var elements = _docElementRepertory.GetAll().Where(m => m.TemplateId.ToLower().Equals(templateId.ToLower()) && string.IsNullOrWhiteSpace(m.ParentId)).ToList();
            elements.AsParallel().ForAll(m =>AddDocumentRootElementCache(templateId, m.Id));
        }
    }
}