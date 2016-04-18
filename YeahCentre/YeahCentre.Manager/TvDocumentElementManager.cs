using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class TvDocumentElementManager : ITvDocumentElementManager
    {
        private readonly ITvDocumentElementRepertory _elementRepertory;
        private readonly ITvDocumentAttributeRepertory _attributeRepertory;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITvTemplateAttributeManager _templateAttributeManager;
        private readonly ITvDocumentElementCacheManager _tvDocumentElementCacheManager;

        public TvDocumentElementManager(ITvDocumentElementRepertory elementRepertory, IRedisCacheService redisCacheService, ITvDocumentAttributeRepertory attributeRepertory,
            ITvTemplateAttributeManager templateAttributeManager, ITvDocumentElementCacheManager tvDocumentElementCacheManager)
        {
            _elementRepertory = elementRepertory;
            _redisCacheService = redisCacheService;
            _attributeRepertory = attributeRepertory;
            _templateAttributeManager = templateAttributeManager;
            _tvDocumentElementCacheManager = tvDocumentElementCacheManager;
        }

        #region Redis

        private IEnumerable<TvDocumentElement> GetDocumentElements()
        {
            if (_redisCacheService.IsSet(RedisKey.DocumentElementsKey))
                return _redisCacheService.GetAllItemsFromSet<TvDocumentElement>(RedisKey.DocumentElementsKey);

            var elements = _elementRepertory.GetAll().OrderBy(m => m.Id).ToList();

            _redisCacheService.AddRangeToSet(RedisKey.DocumentElementsKey, elements, _elementRepertory.GetAll);
            return _redisCacheService.GetAllItemsFromSet<TvDocumentElement>(RedisKey.DocumentElementsKey);
        }
        private IEnumerable<TvDocumentElement> GetOnlyDocumentElements()
        {
            string key = RedisKey.DocumentOnlyElementsKey;
            if (_redisCacheService.IsSet(key))
                return _redisCacheService.Get<List<TvDocumentElement>>(key);

            var elements = _elementRepertory.GetElements().ToList();

            _redisCacheService.Add(key, elements);
            return _redisCacheService.Get<List<TvDocumentElement>>(key);
        }

        public void UpdateDocumentElements()
        {
            _redisCacheService.Remove(RedisKey.DocumentOnlyElementsKey);
            _redisCacheService.Remove(RedisKey.DocumentAttributesKey);
            _redisCacheService.Remove(RedisKey.DocumentElementsKey);
        }

        public void AddDocumentRootElementCache(string templateId, string elementId)
        {
            _tvDocumentElementCacheManager.AddDocumentRootElementCache(templateId, elementId);
        }

        #endregion

        public TvDocumentElement GetById(string id)
        {
            return GetDocumentElements().FirstOrDefault(m => m.Id == id);
        }
        /// <summary>
        /// 获取对象，不关联其他对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TvDocumentElement GetOnlyElemnt(string id)
        {
            return GetOnlyDocumentElements().FirstOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// 根据元素ID删除元素及子级元素 及属性
        /// </summary>
        /// <param name="id"></param>
        public void Delete(string id)
        {
            //TODO:使用事物级联删除
            var elementList = new List<TvDocumentElement>();
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
                UpdateDocumentElements();

                if (string.IsNullOrWhiteSpace(element.ParentId))
                    _tvDocumentElementCacheManager.RemoveDocumentRootElementCacheByElementName(element.TemplateId,
                        element.Name);
                else
                {
                    _tvDocumentElementCacheManager.RemoveRootNodeCache(element.TemplateId, element.ParentId);
                    _tvDocumentElementCacheManager.AddDocumentRootElementCache(element.TemplateId, element.ParentId);
                }
            }
        }
        /// <summary>
        /// 根据元素ID找到所有子元素
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="elementsList"></param>
        /// <returns></returns>
        private List<TvDocumentElement> DeleteChilds(string elementId, List<TvDocumentElement> elementsList)
        {
            if (elementsList == null)
                elementsList = new List<TvDocumentElement>();
            var childs = _elementRepertory.GetAll().Where(e => e.ParentId == elementId).ToList();
            if (childs.Count != 0)
            {
                elementsList.AddRange(childs);
                childs.ForEach(c => DeleteChilds(c.Id, elementsList));
            }
            return elementsList;
        }
        /// <summary>
        /// 获取所有子元算
        /// </summary>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public List<TvDocumentElement> GetChildElements(string elementId)
        {
            return _elementRepertory.GetAll().Where(e => e.ParentId == elementId).ToList();
        }

        public List<TvDocumentElement> GetElementsByTemplateId(string templateId)
        {
            return GetDocumentElements().Where(e => e.TemplateId.Equals(templateId)).ToList();
        }

        public string Add(TvDocumentElement entity)
        {
            //TODO:需要在事务外调用AddDocumentRootElementCache方法更新缓存

            _elementRepertory.Insert(entity);
            UpdateDocumentElements();
            return entity.Id;
        }
        /// <summary>
        /// 添加子元素即 相关属性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        [UnitOfWork]
        public string Add(TvDocumentElement entity, ICollection<TvTemplateAttribute> attributes)
        {
            //TODO:需要在事务外调用AddDocumentRootElementCache方法更新缓存

            _elementRepertory.Insert(entity);

            if (attributes != null && attributes.Count > 0)
            {
                var attributeList = new List<TvDocumentAttribute>();
                attributes.ToList().ForEach(a =>
                {
                    attributeList.Add(new TvDocumentAttribute()
                    {
                        ElementId = entity.Id,
                        Id = Guid.NewGuid().ToString("N"),
                        TemplateAttributeId = a.Id,
                        Text = a.Text,
                        Value = a.Value
                    });
                });
                _attributeRepertory.Insert(attributeList);
            }

            UpdateDocumentElements();
            return entity.Id;
        }

        public void Update(TvDocumentElement entity)
        {
            var dto = _elementRepertory.FindByKey(entity.Id);
            string oldName = dto.Name;
            dto.Name = entity.Name;
            dto.ParentId = entity.ParentId;
            dto.Orders = entity.Orders;
            _elementRepertory.Update(dto);
            UpdateDocumentElements();

            if (string.IsNullOrWhiteSpace(dto.ParentId))
            {
                if (oldName.ToLower().Equals(dto.Name.ToLower()))
                    _tvDocumentElementCacheManager.RemoveRootNodeCache(dto.TemplateId, dto.Id);
                else
                    _tvDocumentElementCacheManager.RemoveDocumentRootElementCacheByElementName(dto.TemplateId, oldName);

                _tvDocumentElementCacheManager.AddDocumentRootElementCache(dto.TemplateId, dto.Id);
            }

        }
        public bool DocumentNameIsExist(TvDocumentElement dto)
        {
            var existElements = GetOnlyDocumentElements().Where(e =>
                (
                e.TemplateId == dto.TemplateId
                && (string.IsNullOrWhiteSpace(e.Name) ? false : (e.Name.Trim().ToLower() == dto.Name.Trim().ToLower()))
                && (string.IsNullOrWhiteSpace(dto.ParentId) ? string.IsNullOrWhiteSpace(e.ParentId) : e.ParentId == dto.ParentId)
                ));
            return string.IsNullOrWhiteSpace(dto.Id) ? existElements.Count() > 0 : existElements.Where(e => e.Id != dto.Id).Count() > 0;
        }
        public List<TvDocumentElement> GetAll()
        {
            return GetOnlyDocumentElements().ToList();
        }

        public List<ElementDto> GeElementNodes(string templateId)
        {
            return GetDocumentElements().Where(m => m.TemplateId == templateId).Select(m => new ElementDto()
            {
                id = m.Id.ToString(),
                pId = m.ParentId.ToString(),
                name = m.Name,
                Attributes = m.Attributes.Select(attr => new AttributeDto()
                                            {
                                                Key = attr.Text,
                                                Value = attr.Value
                                            })
                                            .ToList()
            }).ToList();
        }
        public List<DocumentElementDto> GeDocumentElementNodes(string templateId)
        {
            return GetDocumentElements().Where(m => m.TemplateId == templateId).Select(m => new DocumentElementDto()
            {
                id = m.Id.ToString(),
                pId = m.ParentId != null ? m.ParentId.ToString() : "",
                name = m.Name,
                isallowchild = m.TemplateElement == null ? false : m.TemplateElement.IsAllowChild,
                editable = m.TemplateElement == null ? false : m.TemplateElement.Editable,
                Attributes = m.Attributes.Select(attr => new AttributeDto()
                {
                    Key = attr.Text,
                    Value = attr.Value
                })
                                            .ToList()
            }).ToList();
        }

        public List<TvDocumentElement> GeDocumentElementByTemplateId(string templateId)
        {
            return GetDocumentElements().Where(m => m.TemplateId == templateId).ToList();
        }
    }
}
