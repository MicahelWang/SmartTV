using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;

namespace YeahCenter.Infrastructure
{
    public interface ITvDocumentAttributeManager
    {
        TvDocumentAttribute GetById(string id);

        void Delete(string id);

        string Add(TvDocumentAttribute entity);

        void Update(TvDocumentAttribute entity);

        void BatchDelete(string[] ids);

        List<TvDocumentAttribute> GetAttributes(string elementId);
        /// <summary>
        /// 获取带有结构对象的属性
        /// </summary>
        /// <returns></returns>
        List<TvDocumentAttribute> GetAllWithAttributes();


        List<TvDocumentAttribute>  GetAttributesByPrentId(string elementId, string parrentId);

    }
}