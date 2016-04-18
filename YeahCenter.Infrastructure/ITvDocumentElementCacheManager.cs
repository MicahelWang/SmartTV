using System.Collections.Generic;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahCenter.Infrastructure
{
    public interface ITvDocumentElementCacheManager
    {
        void AddDocumentRootElementCache(string templateId, string elementId);
        void SetTemplateRootElementCache(string templateId);
        void RemoveTemplateRootNodeCache(string templateId);
        void RemoveRootNodeCache(string templateId, string elementId);
        void RemoveDocumentRootElementCacheByElementName(string templateId, string elementName);
        void RemoveRootNodeCacheByTemplateTypeAttributeId(string templateTypeAttributeId);
        void RemoveRootNodeCacheByTemplateTypeElementId(string templateTypeElementId);
        List<TvDocumentElement> GetRootNodeFromCache(string templateId, string name);
    }
}