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

    public class HotelTVChannelRepertory : BaseRepertory<HotelTVChannel, string>, IHotelTVChannelRepertory
    {
        public override List<HotelTVChannel> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HotelTVChannelCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Category))
                query = query.Where(q => q.Category.Equals(criteria.Category));

            if (!string.IsNullOrEmpty(criteria.CategoryEn))
                query = query.Where(q => q.CategoryEn.Equals(criteria.CategoryEn));

            if (!string.IsNullOrEmpty(criteria.ChannelCode))
                query = query.Where(q => q.ChannelCode.Equals(criteria.ChannelCode));

            if (!string.IsNullOrEmpty(criteria.ChannelId))
                query = query.Where(q => q.ChannelId.Equals(criteria.ChannelId));

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(q => q.Name.Equals(criteria.Name));

            if (!string.IsNullOrEmpty(criteria.NameEn))
                query = query.Where(q => q.NameEn.Equals(criteria.NameEn));

            if (criteria.SortFiled.Equals("Id"))
                criteria.SortFiled = "LastUpdateTime";

            return query.ToPageList(criteria);
        }

        public List<string> SearchOnlyHotelId(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HotelTVChannelCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Category))
                query = query.Where(q => q.Category.Equals(criteria.Category));

            if (!string.IsNullOrEmpty(criteria.CategoryEn))
                query = query.Where(q => q.CategoryEn.Equals(criteria.CategoryEn));

            if (!string.IsNullOrEmpty(criteria.ChannelCode))
                query = query.Where(q => q.ChannelCode.Equals(criteria.ChannelCode));

            if (!string.IsNullOrEmpty(criteria.ChannelId))
                query = query.Where(q => q.ChannelId.Equals(criteria.ChannelId));

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(q => q.Name.Equals(criteria.Name));

            if (!string.IsNullOrEmpty(criteria.NameEn))
                query = query.Where(q => q.NameEn.Equals(criteria.NameEn));

            //if (criteria.SortFiled.Equals("Id"))
            //    criteria.SortFiled = "LastUpdateTime";

            var newQuery = query.Select(m => m.HotelId).Distinct().AsQueryable().OrderBy(m => m);

            searchCriteria.TotalCount = newQuery.FutureCount();

            return searchCriteria.NeedPaging ? newQuery.Page(searchCriteria.PageSize, searchCriteria.Page).ToList() : newQuery.ToList();

        }
    }
}
