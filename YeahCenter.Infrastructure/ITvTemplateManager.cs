using System.Collections.Generic;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahCenter.Infrastructure
{
    public interface ITvTemplateManager
    {
        List<TvTemplate> GetAll();
        TvTemplate GetTvTemplateById(string id);
        object GetById(string id, string templateRootName);

        TvTemplate GetElementByName(string name);
        void BatchDelete(string[] ids);
        [UnitOfWork]
        string Copy(ViewCopyTemplate entity);
        [UnitOfWork]
        string AddWithElements(TvTemplate entity);
        void Update(TvTemplate entity);
        /// <summary>
        /// 更新缓存
        /// </summary>
        void UpdateTemplates();

        void SetTemplateRootElementCache(string templateId);

        IPagedList<TvTemplate> PagedList(int pageIndex, int pageSize, string keyword);
    }
}
