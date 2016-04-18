namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel;

    public class AppPublishRepertory : BaseRepertory<AppPublish, string>, IAppPublishLibraryRepertory
    {
        public override List<AppPublish> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as AppPublishCriteria;

            var query = base.Entities.Include("AppVersion.App").AsQueryable();

            if (!string.IsNullOrEmpty(criteria.AppId))
                query = query.Where(q => q.Id.Equals(criteria.AppId));

            if (criteria.VersionCode.HasValue)
                query = query.Where(q => q.VersionCode.Equals(criteria.VersionCode.Value));

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId) || q.HotelId.ToLower().Equals(Constant.CommonAllPbulishApp.ToLower()));

            if(criteria.PublishTime.HasValue)
                query = query.Where(q => q.PublishDate <= criteria.PublishTime.Value);
                
            if(criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));

            return query.ToPageList(searchCriteria);
        }

        public new List<AppPublish> GetAll()
        {
            return Entities.Include("AppVersion.App").ToList();
        }
    }
}
