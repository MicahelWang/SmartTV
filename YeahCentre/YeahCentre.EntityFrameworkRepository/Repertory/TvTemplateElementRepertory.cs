using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class TvTemplateElementRepertory : BaseRepertory<TvTemplateElement, string>, ITvTemplateElementRepertory
    {
        public override List<TvTemplateElement> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public new List<TvTemplateElement> GetAll()
        {
           return Entities.Include("Attributes").ToList();
        }
        public void BatchDelete(string[] ids)
        {
            var elements = this.Entities.Where(element => ids.Contains(element.Id)).Cast<TvTemplateElement>().ToList();
            this.Entities.RemoveRange(elements);
        }
    }
}