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
        /// ����Ԫ��IDɾ��Ԫ�ؼ��Ӽ�Ԫ�� ������
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