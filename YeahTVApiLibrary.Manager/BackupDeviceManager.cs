using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Manager
{
    public class BackupDeviceManager : BaseManager<BackupDevice,BackupDeviceCriteria>, IBackupDeviceManager
    {
        private readonly IRedisCacheService _redisCacheService;

        public BackupDeviceManager(IBackupDeviceRepertory backupDeviceRepertory, IRedisCacheService redisCacheService)
            : base(backupDeviceRepertory)
        {
            _redisCacheService = redisCacheService;
        }

        //用来做工程APP的新增
        public override void Add(BackupDevice backupDevice)
        {
            var list = base.ModelRepertory.Search(new BackupDeviceCriteria { DeviceSeries = backupDevice.DeviceSeries });

            if (!list.Any())
            {
                base.ModelRepertory.Insert(backupDevice);
            }
            else
            {
                throw new Exception("该设备已存在，不允许重复添加");
            }
        }

        public override void Update(BackupDevice backupDevice)
        {
            var backupDeviceDb = base.ModelRepertory.FindByKey(backupDevice.Id);
            backupDevice.CopyTo(backupDeviceDb, new string[] { "Id" , "DeviceSeries" });

            base.ModelRepertory.Update(backupDeviceDb);
        }

        public void UpdateBackupDeviceBySeries(string deviceSeries)
        {
            var backupDeviceInDb = base.ModelRepertory.Search(new BackupDeviceCriteria { DeviceSeries = deviceSeries}).FirstOrDefault();
            if (backupDeviceInDb != null)
            {
                backupDeviceInDb.Active = true;
                backupDeviceInDb.LastUpdateTime = DateTime.Now;
                base.ModelRepertory.Update(backupDeviceInDb);
            }
        }
        public BackupDevice GetEntity(int id)
        {
            return base.ModelRepertory.FindByKey(id); ;
        }

        public List<HotelInfoStatistics> GetBackupDeviceStatistics(List<string> hotelList)
        {
            var list = (base.ModelRepertory as IBackupDeviceRepertory).GetBackupDeviceStatistics(hotelList);

            var q = from p in list
                    group p by p.HotelId into g
                    select new HotelInfoStatistics() { HotelId = g.Key, BackUpDeviceSeriesCount = g.Count() };
            return q.ToList();
        }
    }
}
