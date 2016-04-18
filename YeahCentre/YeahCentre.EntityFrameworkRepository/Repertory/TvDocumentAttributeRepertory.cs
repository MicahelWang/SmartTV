using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class TvDocumentAttributeRepertory : BaseRepertory<TvDocumentAttribute, string>, ITvDocumentAttributeRepertory
    {
        public override List<TvDocumentAttribute> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }
        public void BatchDelete(string[] ids)
        {
            var attributes = this.Entities.Where(attribute => ids.Contains(attribute.Id)).Cast<TvDocumentAttribute>().ToList();
            this.Entities.RemoveRange(attributes);
        }
        public List<TvDocumentAttribute> GetAllWithAttributes()
        {
            return this.Entities.Include("TemplateAttribute").ToList();
        }
    }
}