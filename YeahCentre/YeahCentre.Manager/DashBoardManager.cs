using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;


namespace YeahCentre.Manager
{
    public class DashBoardManager : IDashBoardManager
    {
        private readonly IBackupDeviceManager _backupDeviceManager;
        private readonly IDeviceTraceLibraryManager _deviceTraceLibraryManager;
        private readonly ICityManager _cityManager;

        public DashBoardManager(IBackupDeviceManager backupDeviceManager
            , IDeviceTraceLibraryManager _deviceTraceLibraryManager
            , ICityManager cityManager)
        {

            this._deviceTraceLibraryManager = _deviceTraceLibraryManager;
            this._backupDeviceManager = backupDeviceManager;
            _cityManager = cityManager;

        }
        public List<HotelInfoStatistics> GetStatisticsHotelList(List<CoreSysHotel> hotelList)
        {
            var list = hotelList.Select(n => n.Id).ToList();
            var backupList = _backupDeviceManager.GetBackupDeviceStatistics(list);
            var deviceTraceList = _deviceTraceLibraryManager.GetDeviceTraceStatistics(list);
            var result = from a in hotelList
                         join b in backupList.DefaultIfEmpty() on a.Id equals IsNullReturnString(b, "HotelId") into backTemp
                         from back in backTemp.DefaultIfEmpty()
                         join c in deviceTraceList on a.Id equals IsNullReturnString(c, "HotelId") into deviceTemp
                         from device in deviceTemp.DefaultIfEmpty()
                         select new HotelInfoStatistics()
                         {
                             HotelId = a.Id,
                             HotelName = a.HotelName,
                             City = IsNullReturnString(_cityManager.GetById(a.City), "Name"),
                             BackUpDeviceSeriesCount = IsNullReturnInt(back, "BackUpDeviceSeriesCount"),
                             DeviceTraceSeriesCount = IsNullReturnInt(device, "DeviceTraceSeriesCount"),
                             DeviceSeriesTotal = IsNullReturnInt(back, "BackUpDeviceSeriesCount") + IsNullReturnInt(device, "DeviceTraceSeriesCount")
                         };
            return result.ToList();
        }
        private int IsNullReturnInt(object o, string field)
        {
            if (o == null)
                return 0;
            Type t = o.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            var proper = property.First().GetValue(o, null) ?? 0;
            return int.Parse(proper.ToString());
        }
        private string IsNullReturnString(object o, string field)
        {
            if (o == null)
                return "";
            Type t = o.GetType();
            IEnumerable<System.Reflection.PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            var proper = property.First().GetValue(o, null) ?? "";
            return proper.ToString();
        }
    }



}
