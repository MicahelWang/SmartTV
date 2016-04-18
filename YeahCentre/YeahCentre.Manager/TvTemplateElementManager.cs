using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class TvTemplateElementManager : ITvTemplateElementManager
    {
        private readonly ITvTemplateElementRepertory _elementRepertory;
        private readonly ITvTemplateAttributeRepertory _attributeRepertory;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITvDocumentElementCacheManager _tvDocumentElementCacheManager;

        public TvTemplateElementManager(ITvTemplateElementRepertory elementRepertory, IRedisCacheService redisCacheService, ITvTemplateAttributeRepertory attributeRepertory
             , ITvDocumentElementCacheManager tvDocumentElementCacheManager)
        {
            _elementRepertory = elementRepertory;
            _redisCacheService = redisCacheService;
            _attributeRepertory = attributeRepertory;
            _tvDocumentElementCacheManager = tvDocumentElementCacheManager;
        }

        #region Redis

        private IEnumerable<TvTemplateElement> GetTemplateElements()
        {
            const string key = RedisKey.TemplateElementsKey;
            if (_redisCacheService.IsSet(key))
                return _redisCacheService.Get<List<TvTemplateElement>>(key);
            var elements = _elementRepertory.GetAll().OrderBy(m => m.Id).ToList();
            _redisCacheService.Add(key, elements);
            return _redisCacheService.Get<List<TvTemplateElement>>(key);
        }

        public void UpdateTemplateElements()
        {
            const string key = RedisKey.TemplateElementsKey;
            var elements = _elementRepertory.GetAll().OrderBy(m => m.Id).ToList();
            if (_redisCacheService.IsSet(key))
                _redisCacheService.Set(key, elements);
            else
            {
                _redisCacheService.Add(key, elements);
            }
            _redisCacheService.Remove(RedisKey.DocumentAttributesKey);
            _redisCacheService.Remove(RedisKey.DocumentElementsKey);
            _redisCacheService.Remove(RedisKey.TemplateAttributesKey);
        }
        #endregion

        public TvTemplateElement GetById(string id)
        {
            return GetTemplateElements().FirstOrDefault(m => m.Id == id);
        }

        public List<TvTemplateElement> GetAll()
        {
            return GetTemplateElements().ToList();
        }
        /// <summary>
        /// 根据结构ID查找子框架
        /// </summary>
        /// <param name="templateTypeId"></param>
        /// <returns></returns>
        public TvTemplateElement GetChildFrameByTemplateIdId(int templateTypeId)
        {
            return GetTemplateElements().Where(e => e.TemplateType == templateTypeId && e.IsChildFrame).FirstOrDefault();
        }

        public void Delete(string id)
        {
            _elementRepertory.Delete(m => m.Id == id);
            UpdateTemplateElements();

            var element = _elementRepertory.FindByKey(id);
            if (element != null)
                _tvDocumentElementCacheManager.RemoveRootNodeCacheByTemplateTypeElementId(id);
        }
        /// <summary>
        /// 根据元素ID删除元素及子级元素 及属性
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWithChilds(string id)
        {
            //TODO:使用事物级联删除
            var elementList = new List<TvTemplateElement>();
            var element = _elementRepertory.FindByKey(id);
            elementList.Add(element);
            //获取所有子节点元素
            elementList.AddRange(DeleteChilds(id, null));

            if (elementList.Count > 0)
            {
                //根据元素ID删除所有属性
                var attributes = _attributeRepertory.GetAll().Select(a => new { a.Id, a.ElementId }).Where(a => elementList.Select(e => e.Id).Contains(a.ElementId)).ToList();
                _attributeRepertory.BatchDelete(attributes.Select(a => a.Id).ToArray());

                //删除所有元素
                _elementRepertory.BatchDelete(elementList.Select(e => e.Id).ToArray());

                //更新缓存
                UpdateTemplateElements();

                _tvDocumentElementCacheManager.RemoveRootNodeCacheByTemplateTypeElementId(id);
            }
        }
        /// <summary>
        /// 根据元素ID找到所有子元素
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="elementsList"></param>
        /// <returns></returns>
        private List<TvTemplateElement> DeleteChilds(string elementId, List<TvTemplateElement> elementsList)
        {
            if (elementsList == null)
                elementsList = new List<TvTemplateElement>();
            var childs = _elementRepertory.GetAll().Where(e => e.ParentId == elementId).ToList();
            if (childs.Count != 0)
            {
                elementsList.AddRange(childs);
                childs.ForEach(c => DeleteChilds(c.Id, elementsList));
            }
            return elementsList;
        }

        public string Add(TvTemplateElement entity)
        {
            entity.Id = Guid.NewGuid().ToString("N");
            if (entity.ParentId == null)
            {
                entity.ParentId = "";
            }
            _elementRepertory.Insert(entity);
            UpdateTemplateElements();
            return entity.Id;
        }

        public void Update(TvTemplateElement entity)
        {
            var dto = _elementRepertory.FindByKey(entity.Id);
            dto.Name = entity.Name;
            dto.ParentId = entity.ParentId;
            dto.Orders = entity.Orders;
            dto.Editable = entity.Editable;
            dto.IsAllowChild = entity.IsAllowChild;
            dto.IsChildFrame = entity.IsChildFrame;
            _elementRepertory.Update(dto);
            UpdateTemplateElements();

            _tvDocumentElementCacheManager.RemoveRootNodeCacheByTemplateTypeElementId(dto.Id);
        }
        public bool ElementNameIsExist(TvTemplateElement entity)
        {
            var existElements = GetTemplateElements().Where(e =>
                (
                e.TemplateType == entity.TemplateType
                && (string.IsNullOrWhiteSpace(e.Name) ? false : (e.Name.Trim().ToLower() == entity.Name.Trim().ToLower()))
                && (string.IsNullOrWhiteSpace(entity.ParentId) ? string.IsNullOrWhiteSpace(e.ParentId) : e.ParentId == entity.ParentId)
                ));
            return string.IsNullOrWhiteSpace(entity.Id) ? existElements.Count() > 0 : existElements.Where(e => e.Id != entity.Id).Count() > 0;
        }

        public List<ElementDto> GeElementNodes(int templateType)
        {

            return GetTemplateElements().Where(m => m.TemplateType == templateType).Select(m => new ElementDto()
            {
                id = m.Id,
                pId = m.ParentId,
                name = m.Name,
                Attributes = m.Attributes == null ? null : m.Attributes.Select(attr => new AttributeDto()
                                            {
                                                Key = attr.Text,
                                                Value = attr.Value
                                            }).ToList()
            }).ToList();
        }
    }
}
