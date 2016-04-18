using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class TvDocumentElementRepertory : BaseRepertory<TvDocumentElement, string>, ITvDocumentElementRepertory
    {
        public override List<TvDocumentElement> Search(BaseSearchCriteria searchCriteria)
        {
            throw new System.NotImplementedException();
        }
        public void BatchDelete(string[] ids)
        {
            var elements=this.Entities.Where(element => ids.Contains(element.Id)).Cast<TvDocumentElement>().ToList();
            this.Entities.RemoveRange(elements);
        }
        public List<TvDocumentElement> GetByTemplateId(string templateId)
        {
           return this.Entities.Include("Attributes").Where(m => m.TemplateId == templateId).ToList();
        }
        public new List<TvDocumentElement> GetAll()
        {
            return Entities.Include("Attributes.TemplateAttribute").Include("TemplateElement").ToList();
        }
        /// <summary>
        /// 获取元素集合，不关联其他对象
        /// </summary>
        /// <returns></returns>
        public List<TvDocumentElement> GetElements()
        {
            return Entities.ToList();
        }
    }
}