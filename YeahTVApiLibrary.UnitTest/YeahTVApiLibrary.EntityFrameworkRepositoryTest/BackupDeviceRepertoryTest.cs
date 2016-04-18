using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.EntityFrameworkRepository;
using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.SearchCriteria;
using System.Linq;
using YeahTVApi.DomainModel;
using YeahTVApiLibrary.EntityFrameworkRepository.Models;

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.EntityFrameworkRepositoryTest
{
    [TestClass]
    public class BackupDeviceRepertoryTest : BaseRepertoryTest <BackupDevice, int>
    {
        public BackupDeviceRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockBackupDevices(10); };
            base.SetRepertory = () => { return new BackupDeviceRepertory(); };
        }

        [DeploymentItem(@"MySql.Data.dll")]
        [DeploymentItem(@"MySql.Data.Entity.EF6.dll")]
        [DeploymentItem(@"EntityFramework.SqlServer.dll")]
        [TestMethod]
        public void SearchBackupDevice_ShouldReturnNull_WhenBackupDeviceIsNotExit()
        {
            // Arrange
            var backupdevice = new BackupDeviceCriteria();
            backupdevice.HotelId = "aasdasda";
            backupdevice.Active = true;
            backupdevice.LastUpdatUser = "2015-04-24";
            backupdevice.DeviceSeries = "fsfds0";
            backupdevice.Id = "1";
            // Act 
            var actual = base.entityRepertory.Search(backupdevice);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }


        [DeploymentItem(@"MySql.Data.dll")]
        [DeploymentItem(@"MySql.Data.Entity.EF6.dll")]
        [DeploymentItem(@"EntityFramework.SqlServer.dll")]
        [TestMethod]
        public void SearchBackupDevice_ShouldReturnbackupdevice_WhenBackupDeviceIsHave()
        {
            // Arrange
            var backupdevice = new BackupDeviceCriteria();
            backupdevice.HotelId = "hotel1";

            var excepted = base.mockEntities.Where(m => m.HotelId.Equals(backupdevice.HotelId));
            // Act 
            var actual = base.entityRepertory.Search(backupdevice);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.Count(), actual.Count);
        }

        [TestMethod]
        public void TestUpdate()
        {
            // Arrange
            var mockDeviceSeries = new List<string> { "TestDeviceSeries1", "TestDeviceSeries2", "TestDeviceSeries3", "TestDeviceSeries4" };
            
            //Act
            var actual = base.entityRepertory.Update(e => mockDeviceSeries.Contains(e.DeviceSeries), e => new BackupDevice { DeviceSeries = "udapteDeviceSeries", DeviceType = "updateType" });
            
            // Assert
            var backupdevice = new BackupDeviceCriteria();
            backupdevice.DeviceSeries = "udapteDeviceSeries";
            var excepted = base.entityRepertory.Search(backupdevice);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual, excepted.Count);
        }

        private List<BackupDevice> GetMockBackupDevices(int p)
        {
            var backupdevice = new List<BackupDevice>();

            for (int i = 0; i < p; i++)
            {
                backupdevice.Add(new BackupDevice
                {
                    Active = true,
                    LastUpdateTime = DateTime.Now,
                    Id = i,
                    DeviceSeries = "TestDeviceSeries" + i,
                    HotelId = "TestHotelId" + i,
                    LastUpdatUser = "TestLastUpdatUser" + i,
                    Model = "TestModel" + i,
                    DeviceType = "TestDeviceType" + i
                });
            }

            return backupdevice;
        }

    }
}
