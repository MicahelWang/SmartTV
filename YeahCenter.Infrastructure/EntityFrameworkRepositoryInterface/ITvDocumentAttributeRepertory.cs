using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCenter.Infrastructure
{
    public interface ITvDocumentAttributeRepertory : IBsaeRepertory<TvDocumentAttribute>
    {
        /// <summary>
        /// 删除本身及子节点
        /// </summary>
        /// <param name="ids"></param>
        void BatchDelete(string[] ids);

        List<TvDocumentAttribute> GetAllWithAttributes();
    }
}