using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class TvTemplateRepertory : BaseRepertory<TvTemplate, string>, ITvTemplateRepertory
    {
        public override List<TvTemplate> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }
        public new List<TvTemplate> GetAll()
        {
            return Entities.Include("TemplateType").ToList();
        }
    }

    public class TvTemplateAttributeRepertory : BaseRepertory<TvTemplateAttribute, string>, ITvTemplateAttributeRepertory
    {
        public override List<TvTemplateAttribute> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }
        public void BatchDelete(string[] ids)
        {
            var elements = this.Entities.Where(element => ids.Contains(element.Id)).Cast<TvTemplateAttribute>().ToList();
            this.Entities.RemoveRange(elements);
        }
    }
}
