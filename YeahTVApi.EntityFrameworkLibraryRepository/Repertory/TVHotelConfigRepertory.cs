using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;
using EntityFramework.Extensions;


namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class TVHotelConfigRepertory : BaseRepertory<TVHotelConfig, int>, ITVHotelConfigRepertory 
    {

        public override List<TVHotelConfig> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HotelConfigCriteria ;

            var query = base.Entities.AsQueryable();
            int entityId;
            if (!string.IsNullOrEmpty(criteria.Id) && int.TryParse(criteria.Id, out entityId))
                query = query.Where(q => q.Id == entityId);

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Value.Equals(criteria.Active.Value));

            if (!string.IsNullOrEmpty(criteria.ConfigCodes))
            {
                var codes = criteria.ConfigCodes.Split(',');
                query = query.Where(q => codes.Contains(q.ConfigCode));
            }
   
            return query.ToPageList(searchCriteria);
        }
        public List<string> SearchOnlyHotelId(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HotelConfigCriteria;
           

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));

            if (!string.IsNullOrEmpty(criteria.ConfigCodes))
                query = query.Where(q => q.ConfigCode.Equals(criteria.ConfigCodes));

            var newQuery = query.Select(m => m.HotelId).Distinct().AsQueryable().OrderBy(m=>m);

            searchCriteria.TotalCount = newQuery.FutureCount();
      
            return searchCriteria.NeedPaging?newQuery.Page(searchCriteria.PageSize, searchCriteria.Page).ToList():newQuery.ToList();

        }
    }
}

