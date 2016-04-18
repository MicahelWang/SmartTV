namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel;
    using EntityFramework.Extensions;

    public class LocalizeResourceRepertory : BaseRepertory<LocalizeResource, string>, ILocalizeResourceRepertory
    {
        public override List<LocalizeResource> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as LocalizeResourceCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));

            if (!string.IsNullOrEmpty(criteria.Lang))
                query = query.Where(q => q.Lang.Equals(criteria.Lang));

            if (!string.IsNullOrEmpty(criteria.Content))
                query = query.Where(q => q.Content.Equals(criteria.Content));
             
            return query.ToPageList(criteria);
        }
    }
}
