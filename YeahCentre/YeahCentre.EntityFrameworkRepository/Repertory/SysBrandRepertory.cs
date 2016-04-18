using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysBrandRepertory : BaseRepertory<CoreSysBrand, string>, ISysBrandRepertory
    {

        public override List<CoreSysBrand> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as CoreSysBrandCriteria;

            var entity =  base.Entities.AsQueryable().Where(m=>!m.IsDelete);

            var query = entity.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.GroupId))
                query = query.Where(q => q.GroupId.Equals(criteria.GroupId));

            if (!string.IsNullOrEmpty(criteria.BrandName))
                query = query.Where(q => q.BrandName.Contains(criteria.BrandName));

            return query.ToPageList(searchCriteria);
        }

        public List<CoreSysBrand> GetBrandsByGroup(string groupId)
        {
            var query = base.Entities.Where(m => m.GroupId.Equals(groupId)&& !m.IsDelete);
            return query.ToList();
        }
    }
}
