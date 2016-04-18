namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel;

    public class SystemLogRepertory : BaseRepertory<SystemLog, int>, ISystemLogRepertory
    {
        public override List<SystemLog> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as LogCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));

            if (!string.IsNullOrEmpty(criteria.AppId))
                query = query.Where(q => q.AppType.Equals(criteria.AppId));

            if (!string.IsNullOrEmpty(criteria.LogInfo))
                query = query.Where(q => q.MessageInfo.Contains(criteria.LogInfo));

            if (!string.IsNullOrEmpty(criteria.LogInfoEx))
                query = query.Where(q => q.MessageInfoEx.Contains(criteria.LogInfoEx));

            if (!string.IsNullOrEmpty(criteria.LogType))
                query = query.Where(q => q.MessageType.Equals(criteria.LogType));

            if (!string.IsNullOrEmpty(criteria.AppType))
                query = query.Where(q => q.AppType.Equals(criteria.AppType));

            return query.ToPageList(searchCriteria);
        }
    }
}
