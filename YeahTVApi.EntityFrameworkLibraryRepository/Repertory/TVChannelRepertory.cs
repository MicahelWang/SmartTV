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

    public class TVChannelRepertory : BaseRepertory<TVChannel, string>, ITVChannelRepertory
    {
        public override List<TVChannel> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as TVChannelCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Category))
                query = query.Where(q => q.Category.Equals(criteria.Category));

            if (!string.IsNullOrEmpty(criteria.CategoryEn))
                query = query.Where(q => q.CategoryEn.Equals(criteria.CategoryEn));

            if (!string.IsNullOrEmpty(criteria.DefaultCode))
                query = query.Where(q => q.DefaultCode.Equals(criteria.DefaultCode));

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(q => q.Name.Equals(criteria.Name));

            if (!string.IsNullOrEmpty(criteria.NameEn))
                query = query.Where(q => q.NameEn.Equals(criteria.NameEn));

            return query.ToPageList(criteria);
        }

        public List<TVChannel> SearchChannelsByIds(List<string> keys)
        {
            return base.Entities.Where(m => keys.Contains(m.Id)).ToList();
        }
    }
}
