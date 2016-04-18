namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using EntityFramework.Extensions;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SystemConfigRepertory : BaseRepertory<SystemConfig, int>, ISystemConfigRepertory
    {
        public override List<SystemConfig> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as SystemConfigCriteria;

            var query = criteria.NeedNoTracking ?
                base.Entities.AsNoTracking().AsQueryable() :
                base.Entities.AsNoTracking();

            if (criteria.Id.HasValue)
                query = query.Where(q => q.Id == criteria.Id.Value);

            if (!string.IsNullOrEmpty(criteria.AppType))
                query = query.Where(q => q.ConfigType == criteria.AppType);

            if (!string.IsNullOrEmpty(criteria.ConfigName))
                query = query.Where(q => q.ConfigName.Contains(criteria.ConfigName));

            if (!string.IsNullOrEmpty(criteria.ConfigValue))
                query = query.Where(q => q.ConfigValue.Contains(criteria.ConfigValue));

            if (criteria.Enable.HasValue)
                query = query.Where(q => q.Active == criteria.Enable.Value);
            
            return query.ToPageList(criteria);
        }
    }
}
