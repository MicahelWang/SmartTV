using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class TvTemplateManager : ITvTemplateManager
    {
        private readonly ITvDocumentElementRepertory _elementRepertory;
        private readonly ITvDocumentAttributeRepertory _attributeRepertory;
        private readonly ITvTemplateRepertory _templateRepertory;

        private readonly ITvTemplateElementRepertory _templateElementRepertory;
        private readonly ITvTemplateAttributeRepertory _templateAttributeRepertory;

        private readonly ITvDocumentElementManager _documentElementManager;

        private readonly IRedisCacheService _redisCacheService;
        private readonly ITvDocumentElementCacheManager _tvDocumentElementCacheManager;

        public TvTemplateManager(ITvDocumentElementRepertory elementRepertory, ITvTemplateRepertory templateRepertory, IRedisCacheService redisCacheService,
            ITvDocumentAttributeRepertory attributeRepertory, ITvTemplateAttributeRepertory templateAttributeRepertory, ITvTemplateElementRepertory templateElementRepertory
             , ITvDocumentElementManager documentElementManager
            , ITvDocumentElementCacheManager tvDocumentElementCacheManager)
        {
            _elementRepertory = elementRepertory;
            _templateRepertory = templateRepertory;
            _redisCacheService = redisCacheService;
            _attributeRepertory = attributeRepertory;
            _templateAttributeRepertory = templateAttributeRepertory;
            _templateElementRepertory = templateElementRepertory;
            _documentElementManager = documentElementManager;
            _tvDocumentElementCacheManager = tvDocumentElementCacheManager;
        }

        #region Redis

        private List<TvTemplate> GetTemplates()
        {
            if (_redisCacheService.IsSet(RedisKey.TemplatesKey))
                return _redisCacheService.Get<List<TvTemplate>>(RedisKey.TemplatesKey);
            var types = _templateRepertory.GetAll().OrderByDescending(m => m.CreateDate).ToList();
            _redisCacheService.Add(RedisKey.TemplatesKey, types);
            return _redisCacheService.Get<List<TvTemplate>>(RedisKey.TemplatesKey);
        }
        public void UpdateTemplates()
        {
            var types = _templateRepertory.GetAll().OrderBy(m => m.Id).ToList();
            if (_redisCacheService.IsSet(RedisKey.TemplatesKey))
                _redisCacheService.Set(RedisKey.TemplatesKey, types);
            else
            {
                _redisCacheService.Add(RedisKey.TemplatesKey, types);
            }
        }
        public void SetTemplateRootElementCache(string templateId)
        {
            _tvDocumentElementCacheManager.SetTemplateRootElementCache(templateId);
        }

        #endregion

        public List<TvTemplate> GetAll()
        {
            return GetTemplates();
        }
        public TvTemplate GetTvTemplateById(string id)
        {
            return GetTemplates().Find(t => t.Id == id);
        }

        public TvTemplate GetElementByName(string name)
        {
            return GetTemplates().Find(t => t.Name == name);
        }
        public object GetById(string id, string templateRootName)
        {
            return GetObjects(id, templateRootName);
        }

        private Dictionary<string, object> GetObjects(string id, string templateRootName)
        {
            var template = new Dictionary<string, object>();
            var entity = GetTvTemplateById(id);

            var elements = _tvDocumentElementCacheManager.GetRootNodeFromCache(id, templateRootName);
            if (elements != null)
            {
                template.Add("templateType", entity.TemplateTypeId);
                template.Add("elements", BuildNodes(elements, templateRootName));
            }
            return template;
        }

        /// <summary>
        /// 根据模板ID删除模板
        /// </summary>
        /// <param name="ids"></param>
        public void BatchDelete(string[] ids)
        {
            ids.AsParallel().ForAll(id => DeleteElements(id));
            //批量删除模板
            _templateRepertory.Delete(m => ids.Contains(m.Id));

            ids.AsParallel().ForAll(id => _tvDocumentElementCacheManager.RemoveTemplateRootNodeCache(id));
            UpdateTemplates();
        }
        /// <summary>
        /// 根据模板ID 删除元素及每个元素的属性
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public void DeleteElements(string templateId)
        {
            var elements = _elementRepertory.GetByTemplateId(templateId);
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

        #region Copy

        [UnitOfWork]
        public string Copy(ViewCopyTemplate entity)
        {
            //TODO:外部调用SetTemplateRootElementCache设置节点缓存
            var sourceTemplate = GetTvTemplateById(entity.SourceTemplateId);

            TvTemplate newTemplate = new TvTemplate();
            sourceTemplate.CopyTo(newTemplate, new[] { "Id" });
            newTemplate.CreateDate = DateTime.Now;
            newTemplate.ModifyDate = DateTime.Now;
            newTemplate.CreateUser = entity.CreateUser;
            newTemplate.ModifyUser = entity.CreateUser;
            newTemplate.Name = entity.Name;
            newTemplate.TemplateType = null;
            _templateRepertory.Insert(newTemplate);

            var templateElements = _elementRepertory.GetAll().Where(e => e.TemplateId == sourceTemplate.Id).ToList();
            var templateAttributeQuery = _attributeRepertory.GetAll();
            var attributeList = new ConcurrentBag<TvDocumentAttribute>();
            var elementList = new ConcurrentBag<TvDocumentElement>();

            GetChildElements("", "", newTemplate.Id, templateElements, attributeList, elementList, templateAttributeQuery);

            _elementRepertory.Insert(elementList);
            _attributeRepertory.Insert(attributeList);

            _documentElementManager.UpdateDocumentElements();
            _redisCacheService.Remove(RedisKey.TemplatesKey);

            return newTemplate.Id;
        }


        /// <summary>
        /// 循环添加子元素、属性
        /// </summary>
        /// <param name="oldParentId"></param>
        /// <param name="parentId"></param>
        /// <param name="templateId"></param>
        /// <param name="templateElements"></param>
        public void GetChildElements(string oldParentId, string parentId, string templateId, List<TvDocumentElement> templateElements, ConcurrentBag<TvDocumentAttribute> attributeList,
            ConcurrentBag<TvDocumentElement> elementList, List<TvDocumentAttribute> templateAttributeQuery)
        {
            List<TvDocumentElement> templateElementList = templateElements.Where(e =>
            {
                if (string.IsNullOrWhiteSpace(oldParentId))
                    return string.IsNullOrWhiteSpace(e.ParentId);
                else
                    return e.ParentId == oldParentId;

            }).ToList();

            templateElementList.AsParallel().ForAll(t =>
            {
                var tempDocElement = new TvDocumentElement()
                {
                    Name = t.Name,
                    Orders = t.Orders,
                    ParentId = parentId,
                    Id = Guid.NewGuid().ToString("N"),
                    TemplateElementId = t.TemplateElementId.ToString(),
                    TemplateId = templateId
                };
                elementList.Add(tempDocElement);

                templateAttributeQuery.Where(a => a.ElementId == t.Id).ToList().AsParallel().ForAll(a =>
                {
                    attributeList.Add(new TvDocumentAttribute()
                    {
                        ElementId = tempDocElement.Id,
                        Id = Guid.NewGuid().ToString("N"),
                        TemplateAttributeId = a.TemplateAttributeId,
                        Text = a.Text,
                        Value = a.Value
                    });
                });


                GetChildElements(t.Id, tempDocElement.Id, templateId, templateElements, attributeList, elementList, templateAttributeQuery);
            });
        }

        #endregion

        [UnitOfWork]
        public string AddWithElements(TvTemplate entity)
        {
            //TODO:外部调用SetTemplateRootElementCache设置节点缓存

            _templateRepertory.Insert(entity);

            // throw new Exception("test");

            var templateElements = _templateElementRepertory.GetAll().Where(e => e.TemplateType == entity.TemplateTypeId).ToList();
            var templateAttributeQuery = _templateAttributeRepertory.GetAll();
            var attributeList = new ConcurrentBag<TvDocumentAttribute>();
            var elementList = new ConcurrentBag<TvDocumentElement>();

            GetChildElements("", "", entity.Id, templateElements, attributeList, elementList, templateAttributeQuery);

            _elementRepertory.Insert(elementList);
            _attributeRepertory.Insert(attributeList);

            _documentElementManager.UpdateDocumentElements();
            //UpdateTemplates(); 事务中不能直接更新缓存
            _redisCacheService.Remove(RedisKey.TemplatesKey);

            return entity.Id;
        }

        /// <summary>
        /// 循环添加子元素、属性
        /// </summary>
        /// <param name="oldParentId"></param>
        /// <param name="parentId"></param>
        /// <param name="templateId"></param>
        /// <param name="templateElements"></param>
        public void GetChildElements(string oldParentId, string parentId, string templateId, List<TvTemplateElement> templateElements, ConcurrentBag<TvDocumentAttribute> attributeList,
            ConcurrentBag<TvDocumentElement> elementList, List<TvTemplateAttribute> templateAttributeQuery)
        {
            List<TvTemplateElement> templateElementList = templateElements.Where(e =>
            {
                if (string.IsNullOrWhiteSpace(oldParentId))
                    return string.IsNullOrWhiteSpace(e.ParentId);
                else
                    return e.ParentId == oldParentId;

            }).ToList();

            templateElementList.AsParallel().ForAll(t =>
            {
                var tempDocElement = new TvDocumentElement()
                {
                    Name = t.Name,
                    Orders = t.Orders,
                    ParentId = parentId,
                    Id = Guid.NewGuid().ToString("N"),
                    TemplateElementId = t.Id.ToString(),
                    TemplateId = templateId
                };
                elementList.Add(tempDocElement);

                GetChildAttribute("", "", tempDocElement.Id, attributeList, templateAttributeQuery.Where(m => m.ElementId.Equals(t.Id)).ToList());

                GetChildElements(t.Id, tempDocElement.Id, templateId, templateElements, attributeList, elementList, templateAttributeQuery);
            });
        }

        private void GetChildAttribute(string oldParentId, string parentId, string elementId,
            ConcurrentBag<TvDocumentAttribute> attributeList, List<TvTemplateAttribute> templateAttributeQuery)
        {

            var templateAttributes = templateAttributeQuery.Where(e =>
            {
                if (string.IsNullOrWhiteSpace(oldParentId))
                    return string.IsNullOrWhiteSpace(e.ParentId);
                return e.ParentId == oldParentId;
            }).ToList();

            templateAttributes.AsParallel().ForAll(t =>
                {
                    var newDocumentAttribute = new TvDocumentAttribute()
                        {
                            ElementId = elementId,
                            Id = Guid.NewGuid().ToString("N"),
                            TemplateAttributeId = t.Id,
                            Text = t.Text,
                            Value = t.Value,
                            ParentId = parentId
                        };
                    attributeList.Add(newDocumentAttribute);

                    GetChildAttribute(t.Id, newDocumentAttribute.Id, elementId, attributeList, templateAttributeQuery);
                });
        }


        public void Update(TvTemplate entity)
        {
            var type = _templateRepertory.FindByKey(entity.Id);
            type.Name = entity.Name;
            type.Description = entity.Description;
            type.ModifyDate = entity.ModifyDate;
            type.ModifyUser = entity.ModifyUser;
            _templateRepertory.Update(type);
            UpdateTemplates();

        }



        public List<Dictionary<string, object>> BuildNodes(List<TvDocumentElement> elements, string rootName, string parentId = "")
        {

            var context = new List<Dictionary<string, object>>();

            var currentNodes = elements.Where(
                m => (string.IsNullOrWhiteSpace(parentId) ?
                string.IsNullOrWhiteSpace(m.ParentId)
                : (m.ParentId != null && m.ParentId.Equals(parentId))))
                .OrderBy(m => m.Orders).ToList();

            foreach (var elementDto in currentNodes)
            {
                var element = elementDto.Attributes.ToDictionary<TvDocumentAttribute, string, object>(attribute => attribute.Text, attribute => attribute.Value);
                element.Add("Name", elementDto.Name);
                if (elements.Any(m => m.ParentId == elementDto.Id))
                {
                    element.Add("Nodes", BuildNodes(elements, null, elementDto.Id));
                }
                context.Add(element);
            }
            return context;
        }

        public IPagedList<TvTemplate> PagedList(int pageIndex, int pageSize, string keyword)
        {
            var pageList = new PagedList<TvTemplate>(GetTemplates().OrderByDescending(m => m.ModifyDate).ToList(), pageIndex, pageSize);
            return pageList;
        }
    }
}
