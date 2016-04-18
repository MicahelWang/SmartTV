using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahCenter.Infrastructure
{
    public interface ITvTemplateElementManager
    {
        void UpdateTemplateElements();
        TvTemplateElement GetById(string id);

        List<TvTemplateElement> GetAll();

        void Delete(string id);        
        /// <summary>
        /// 根据元素ID删除元素及子级元素 及属性
        /// </summary>
        /// <param name="id"></param>
        void DeleteWithChilds(string id);

        string Add(TvTemplateElement entity);

        void Update(TvTemplateElement entity);
        bool ElementNameIsExist(TvTemplateElement entity);
        List<ElementDto> GeElementNodes(int templateType);
        TvTemplateElement GetChildFrameByTemplateIdId(int templateTypeId);

    }
}