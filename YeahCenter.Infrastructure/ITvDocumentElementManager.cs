using System.Collections.Generic;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahCenter.Infrastructure
{
    public interface ITvDocumentElementManager
    {
        TvDocumentElement GetById(string id);
        TvDocumentElement GetOnlyElemnt(string id);

        void Delete(string id);

        string Add(TvDocumentElement entity);
        [UnitOfWork]
        string Add(TvDocumentElement entity,ICollection<TvTemplateAttribute> attributes);

        void Update(TvDocumentElement entity);

        void UpdateDocumentElements();
        void AddDocumentRootElementCache(string templateId, string elementId);
        /// <summary>
        /// 检查同一个父节点下是否存在相同名称元素
        /// </summary>
        /// <returns></returns>
        bool DocumentNameIsExist(TvDocumentElement dto);
        List<TvDocumentElement> GetAll();
        List<TvDocumentElement> GetChildElements(string elementId);
        List<ElementDto> GeElementNodes(string templateId);
        List<DocumentElementDto> GeDocumentElementNodes(string templateId);
        List<TvDocumentElement> GeDocumentElementByTemplateId(string templateId);
        List<TvDocumentElement> GetElementsByTemplateId(string templateId);

    }
}