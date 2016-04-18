using EntityFramework.Extensions;
using YeahTVApi.Common;

namespace YeahTVApi.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Infrastructure;
    using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
    using System.Collections.Generic;
    using System.Linq;

    public class TVTraceRepertory : DeviceTraceRepertory, ITVTraceRepertory
    {
        public override List<DeviceTrace> Search(BaseSearchCriteria searchCriteria)
        { 
            var tVTraceModelCriteria = searchCriteria as TraceCriteria;
            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(tVTraceModelCriteria.HotelId))
                query = query.Where(q => q.HotelId.Contains(tVTraceModelCriteria.HotelId));

            if (!string.IsNullOrEmpty(tVTraceModelCriteria.DeviceSeries))
                query = query.Where(q => q.DeviceSeries.Equals(tVTraceModelCriteria.DeviceSeries));

            if (!string.IsNullOrEmpty(tVTraceModelCriteria.Platfrom))
                query = query.Where(q => q.Platfrom.Contains(tVTraceModelCriteria.Platfrom));

            if (!string.IsNullOrEmpty(tVTraceModelCriteria.RoomNo))
                query = query.Where(q => q.RoomNo.Contains(tVTraceModelCriteria.RoomNo));

            if (!tVTraceModelCriteria.IsTVTrace.HasValue || tVTraceModelCriteria.IsTVTrace.Value)
                query = query.Where(q => string.IsNullOrEmpty(q.Token));
            else
                query = query.Where(q => !string.IsNullOrEmpty(q.Token) && q.Token.Contains(tVTraceModelCriteria.Token));

            if (tVTraceModelCriteria.NeedPaging)
            {
                searchCriteria.TotalCount = query.FutureCount();
                query = query.OrderBy(q=>q.RoomNo).Page(tVTraceModelCriteria.PageSize, tVTraceModelCriteria.Page);
                
            }

            return query.ToList();
        }

        public List<string> GetTraceHotelIds()
        {
            return base.Entities.Where(e => string.IsNullOrEmpty(e.Token))
                .Select(e => e.HotelId).Distinct().ToList();
        }
    }
}
