namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;

    public class BehaviorLogRepertory : BaseRepertory<BehaviorLog, string>, IBehaviorLogRepertory
    {
        public override List<BehaviorLog> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as LogCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));

            if (!string.IsNullOrEmpty(criteria.LogInfo))
                query = query.Where(q => q.BehaviorInfo.Contains(criteria.LogInfo));

            if (criteria.BehaviorType.HasValue)
                query = query.Where(q => q.BehaviorType.ToLower().Equals(criteria.BehaviorType.Value.ToString().ToLower()));


            if (criteria.CompleteBeginTime != null)
                query = query.Where(q => q.CreateTime >= criteria.CompleteBeginTime);

            if (criteria.CompleteEndTime != null)
                query = query.Where(q => q.CreateTime <= criteria.CompleteEndTime);

            return searchCriteria.NeedPaging ? query.ToPageList(searchCriteria) : query.ToList();
        }
    }
}
