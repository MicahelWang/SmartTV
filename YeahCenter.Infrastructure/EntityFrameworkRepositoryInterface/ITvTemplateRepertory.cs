using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCenter.Infrastructure
{
    public interface ITvTemplateRepertory : IBsaeRepertory<TvTemplate>
    {
        
    }

    public interface ITvTemplateElementRepertory : IBsaeRepertory<TvTemplateElement>
    {
        /// <summary>
        /// 批量删除元素及子元素 并且删除每个子元素的属性
        /// </summary>
        /// <param name="ids"></param>
        void BatchDelete(string[] ids);
    }

    public interface ITvTemplateAttributeRepertory : IBsaeRepertory<TvTemplateAttribute>
    {
        /// <summary>
        /// 批量删除元素及子元素 并且删除每个子元素的属性
        /// </summary>
        /// <param name="ids"></param>
        void BatchDelete(string[] ids);
    }
}