using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using System.Linq;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysGroupRepertory : BaseRepertory<CoreSysGroup, string>, ISysGroupRepertory
    {
        public override List<CoreSysGroup> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as GroupCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.GroupName))
                query = query.Where(q => q.GroupName.Contains(criteria.GroupName));
            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));
            return query.ToPageList(searchCriteria);
        }
    }
}
