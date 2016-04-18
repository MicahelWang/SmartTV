namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;

    public class AppsRepertory : BaseRepertory<Apps, string>, IAppsLibraryRepertory
    {
        public override List<Apps> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as AppsCriteria;

            var entity = criteria.NeedVersion
                 ? base.Entities.Include("AppVresions.AppPublishs").AsQueryable()
                 : base.Entities.AsQueryable();

            var query = entity.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.AppName))
                query = query.Where(q => q.Name.Equals(criteria.AppName));

            if (!string.IsNullOrEmpty(criteria.PackageName))
                query = query.Where(q => q.PackageName.Equals(criteria.PackageName));

            if (criteria.AppVersion.HasValue)
                query = query.Where(q => q.AppVresions.Any(a => a.VersionCode.Equals(criteria.AppVersion.Value)));

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));

            if (criteria.ShowInStroe.HasValue)
                query = query.Where(q => q.ShowInStroe.Value.Equals(criteria.ShowInStroe.Value));

            if (criteria.AppVersionActive.HasValue)
                query = query.Where(q => q.AppVresions.Any(v=>v.Active.Equals(criteria.AppVersionActive.Value)));

            return criteria.NeedPaging ? query.ToPageList(searchCriteria) : query.ToList();
        }

        public new List<Apps> GetAll()
        {
            return Entities.Include("AppVresions.AppPublishs").AsQueryable().ToList();
        }
    }
}
