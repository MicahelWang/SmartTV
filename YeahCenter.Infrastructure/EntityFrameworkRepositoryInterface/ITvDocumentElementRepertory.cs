using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCenter.Infrastructure
{
    public interface ITvDocumentElementRepertory : IBsaeRepertory<TvDocumentElement>
    {

        List<TvDocumentElement> GetByTemplateId(string templateId);
        /// <summary>
        /// 批量删除元素及子元素 并且删除每个子元素的属性
        /// </summary>
        /// <param name="ids"></param>
        void BatchDelete(string[] ids);
        /// <summary>
        /// 获取元素集合，不关联其他对象
        /// </summary>
        /// <returns></returns>
        List<TvDocumentElement> GetElements();
    }
}