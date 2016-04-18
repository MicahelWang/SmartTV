using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;


namespace YeahTVApiLibrary.Infrastructure
{
    public interface IBackupDeviceManager :IBaseManager<BackupDevice,BackupDeviceCriteria>
    {
        BackupDevice GetEntity(int id);

        void UpdateBackupDeviceBySeries(string deviceSeries);
        List<HotelInfoStatistics> GetBackupDeviceStatistics(List<string> hotelList);
    }
}
