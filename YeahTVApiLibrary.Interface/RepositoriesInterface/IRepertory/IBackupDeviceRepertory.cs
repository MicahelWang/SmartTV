using System.Collections.Generic;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IBackupDeviceRepertory : IBsaeRepertory<BackupDevice>
    {
        BackupDevice GetSingle(BaseSearchCriteria searchCriteria);

        List<BackupDevice> GetBackupDeviceStatistics(List<string> hotelList);
    }
}
