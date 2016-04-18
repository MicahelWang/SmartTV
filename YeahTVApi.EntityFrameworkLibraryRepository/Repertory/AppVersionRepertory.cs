namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;

    public class AppVersionRepertory : BaseRepertory<AppVersion, string>, IAppVersionLibraryRepertory
    {
        public override List<AppVersion> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as AppsCriteria;

            var entity = base.Entities.Include("App").AsQueryable();

            var query = entity.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));

            if (!string.IsNullOrEmpty(criteria.AppName))
                query = query.Where(q => q.App.Name.Equals(criteria.AppName));

            if (criteria.AppVersion.HasValue)
                query = query.Where(q => q.VersionCode.Equals(criteria.AppVersion.Value));

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));

            if (criteria.SortFiled.Equals("Id"))
                criteria.SortFiled = "LastUpdateTime";

            return query.ToPageList(criteria);
        }
        public new List<AppVersion> GetAll()
        {
            return Entities.Include("App").ToList();
        }
    }
}
