using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.DomainModel;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class BackupDeviceRepertory : BaseRepertory<BackupDevice, int>, IBackupDeviceRepertory
    {

        public override List<BackupDevice> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as BackupDeviceCriteria;

            var query = base.Entities.AsQueryable();
            int sid;
             if (!string.IsNullOrEmpty(criteria.Id) && int.TryParse( criteria.Id,out sid))
                 query = query.Where(q => q.Id.Equals(sid));

            int entityId;
            if (!string.IsNullOrEmpty(criteria.Id) && int.TryParse(criteria.Id, out entityId))
                query = query.Where(q => q.Id == entityId);

            if (!string.IsNullOrEmpty(criteria.DeviceSeries))
                query = query.Where(q => q.DeviceSeries.Contains(criteria.DeviceSeries));

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));

            if (!string.IsNullOrEmpty(criteria.LastUpdatUser))
                query = query.Where(q => q.LastUpdatUser.Equals(criteria.LastUpdatUser));

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));

            return query.ToPageList(searchCriteria);
        }

        public BackupDevice GetSingle(BaseSearchCriteria searchCriteria)
        {
            var deviceSearchCriteria = searchCriteria as BackupDeviceCriteria;
            var query = base.Entities.AsQueryable();

            if (deviceSearchCriteria != null && !string.IsNullOrEmpty(deviceSearchCriteria.DeviceSeries))
                query = query.Where(q => q.DeviceSeries.Equals(deviceSearchCriteria.DeviceSeries));

            if (!string.IsNullOrEmpty(deviceSearchCriteria.HotelId))
                query = query.Where(q => q.HotelId.Contains(deviceSearchCriteria.HotelId));

            return query.SingleOrDefault();
        }


        public List<BackupDevice> GetBackupDeviceStatistics(List<string> hotelList)
        {
            return base.Entities.Where(m => hotelList.Contains(m.HotelId) && !m.IsUsed).ToList();
        }
    }
}
