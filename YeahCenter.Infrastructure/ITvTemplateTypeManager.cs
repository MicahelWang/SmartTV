using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;

namespace YeahCenter.Infrastructure
{
    public interface ITvTemplateTypeManager
    {

        List<TvTemplateType> GetAll();
        TvTemplateType GetById(int id);

        TvTemplateType GetByName(string name);

        void Delete(int id);

        int Add(TvTemplateType entity);
        int AddWithBaseNode(TvTemplateType entity);
        void Update(TvTemplateType entity);

        void BatchDelete(int[] ids);
        /// <summary>
        /// 批量删除模板结构，及子元素
        /// </summary>
        /// <param name="ids"></param>
        void BatchDeleteWithChilds(int[] ids);

        IPagedList<TvTemplateType> PagedList(int pageIndex, int pageSize, string keyword);
    }


}
