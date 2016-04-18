namespace YeahTVApi.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Infrastructure;
    using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
    using System.Collections.Generic;
    using System.Linq;

    public class TVAppsRepertory : AppsRepertory, ITVAppsRepertory
    {
        public override List<Apps> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as AppsCriteria;

            var entity = criteria.NeedVersion
                ? base.Entities.Include("AppVresions")
                : base.Entities;

            var query = entity.AsQueryable();

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));
            
            if (!string.IsNullOrEmpty(criteria.Platform))
                query = query.Where(q => q.Platfrom.Equals(criteria.Platform));
            
            if (criteria.ShowInStroe.HasValue)
                query = query.Where(q => q.ShowInStroe.Value.Equals(criteria.ShowInStroe.Value));

            return query.ToList();
        }
    }
}
