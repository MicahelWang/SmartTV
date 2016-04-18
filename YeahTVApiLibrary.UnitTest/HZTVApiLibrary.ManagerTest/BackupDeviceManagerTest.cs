using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApi.Manager;
using YeahTVApiLibrary.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.DomainModel.Mapping;
using YeahTVLibrary.Manager;
using YeahTVApiLibrary.Manager;

namespace YeahTVApiLibrary.UnitTests.HZTVApiLibrary.ManagerTest
{
    [TestClass]
    public class BackupDeviceManagerTest
    {
        private Mock<IBackupDeviceRepertory> mockBackupDeviceRepertory;
        private IBackupDeviceManager backupDeviceManager;
        private List<BackupDevice> mockTraces;
        private Mock<IRedisCacheService> mockRedisCacheService;
        //初始化
        [TestInitialize]
        public void Initial()
        {
            mockTraces = GetMockDeviceTraces(10);
            mockBackupDeviceRepertory = new Mock<IBackupDeviceRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            this.backupDeviceManager = new BackupDeviceManager(mockBackupDeviceRepertory.Object, mockRedisCacheService.Object);
        }


        [TestMethod]
        public void UpdateBackupDevice_ShouldReturnSueccess_WhenBackupDeviceIsNotNull()
        {
            //Arrange
            var backupDevice = new BackupDevice()
            {
                Active = true,
                DeviceSeries = "BackupDevice1",
                HotelId = "sdsdfsfsdfsdfsdfs",
                Id = 1,
                LastUpdateTime = DateTime.Now,
                LastUpdatUser = "admin"
            };

            var mockTraceInDb = mockTraces.FirstOrDefault();
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<BackupDevice>>>())).Returns(new List<BackupDevice>() { mockTraceInDb });
            mockBackupDeviceRepertory.Setup(m => m.Update(backupDevice)).Callback(() =>
             {
                 mockTraces.SingleOrDefault(m => m.Id == 1).DeviceSeries = backupDevice.DeviceSeries;
             });
            mockBackupDeviceRepertory.Setup(m => m.Search(It.IsAny<BackupDeviceCriteria>())).Returns(new List<BackupDevice> { mockTraceInDb });
            mockRedisCacheService.Setup(
                  m => m.UpdateItemFromSet(It.IsAny<string>(), It.IsAny<BackupDevice>(), It.IsAny<BackupDevice>()));

            //Act
            backupDeviceManager.Update(mockTraceInDb);

            //Assert
            Assert.IsTrue(mockTraces.Any(m => m.DeviceSeries.Equals(backupDevice.DeviceSeries)));
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void UpdateBackupDevice_ShouldFail_WhenRepertoryHasException()
        {
            //Arrange
            var backupdevice = new BackupDevice()
            {
                Active = true,
                DeviceSeries = "BackupDevice1",
                HotelId = "sdsdfsfsdfsdfsdfs",
                Id = 1,
                LastUpdateTime = DateTime.Now,
                LastUpdatUser = "admin"
            };
            mockBackupDeviceRepertory.Setup(m => m.Update(backupdevice)).Callback(() =>
            {
                throw new CommonFrameworkManagerException("UpdateMovieTraceManager Error!", null);
            });
            mockBackupDeviceRepertory.Setup(m => m.Search(It.IsAny<BackupDeviceCriteria>())).Returns(new List<BackupDevice> { backupdevice });
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<BackupDevice>>>())).Returns(new List<BackupDevice>() { backupdevice });
            mockRedisCacheService.Setup(
                m => m.UpdateItemFromSet(It.IsAny<string>(), It.IsAny<BackupDevice>(), It.IsAny<BackupDevice>()));

            //Act
            backupDeviceManager.Update(backupdevice);
        }

        [TestMethod]
        public void AddBackupDevice_whenDeviceSeriesIsnotHave_ShouldreturnSuccess()
        {
            //Arrange

            BackupDevice backupDevice = new BackupDevice()
            {
                Active = true,
                DeviceSeries = "sdfsdfs",
                DeviceType = "AIO",
                HotelId = "sdfgdhwsfer",
                Id = 1,
                LastUpdateTime = DateTime.Now,
                LastUpdatUser = "admin",
                Model = "AIO"
            };

            mockBackupDeviceRepertory.Setup(m => m.Search(It.IsAny<BackupDeviceCriteria>()))
                .Returns(new List<BackupDevice>());

            mockBackupDeviceRepertory.Setup(m => m.Insert(backupDevice))
                .Callback<BackupDevice>((f) =>
                {
                    mockTraces.Add(f);
                });
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<BackupDevice>>>())).Returns(new List<BackupDevice>());
            //Act
            backupDeviceManager.Add(backupDevice);

            //Assert
            Assert.IsTrue(mockTraces.Any(m => m.DeviceSeries.Equals(backupDevice.DeviceSeries)));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddBackupDevice_whenDeviceSeriesIsnotHave_ShouldreturnFail()
        {
            //Arrange

            BackupDevice backupDevice = new BackupDevice()
            {
                Active = true,
                DeviceSeries = "BackupDevice1",
                DeviceType = "AIO",
                HotelId = "sdfgdhwsfer",
                Id = 1,
                LastUpdateTime = DateTime.Now,
                LastUpdatUser = "admin",
                Model = "AIO"
            };
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<BackupDevice>>>())).Returns(new List<BackupDevice>() { backupDevice });
            mockBackupDeviceRepertory.Setup(m => m.Search(It.IsAny<BackupDeviceCriteria>()))
                .Returns(mockTraces);
            //Act
            backupDeviceManager.Add(backupDevice);

        }
        [TestMethod]
        public void GetEfById_whenIdexist_ShouldReturnEf()
        {
            var tracesId = 1;
            var traces = mockTraces.FirstOrDefault(m => m.Id == tracesId);
            mockBackupDeviceRepertory.Setup(m => m.FindByKey(It.IsAny<object>())).Returns<int>((id) => mockTraces.FirstOrDefault(m => m.Id == id));
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<BackupDevice>>>())).Returns(new List<BackupDevice>() { traces });
            //Act
            var actual = backupDeviceManager.GetEntity(tracesId);
            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(traces, actual);
        }
        [TestMethod]
        public void GetEfById_whenIdexist_ShouldReturnNull()
        {
            var tracesId = 11;
            mockBackupDeviceRepertory.Setup(m => m.FindByKey(It.IsAny<object>())).Returns((int id) => mockTraces.FirstOrDefault(m => m.Id == id));
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<BackupDevice>>>())).Returns(new List<BackupDevice>() { });
            //Act
            var actual = backupDeviceManager.GetEntity(tracesId);
            //Assert
            Assert.IsNull(actual);
        }

        private List<BackupDevice> GetMockDeviceTraces(int count)
        {
            var traces = new List<BackupDevice>();

            for (int i = 0; i < count; i++)
            {
                traces.Add(new BackupDevice
                {

                    HotelId = "TestHotelId",
                    DeviceSeries = "BackupDevice" + i,
                    Active = true,
                    Id = i,
                    LastUpdatUser = "admin" + i,
                    LastUpdateTime = DateTime.Now
                });
            }

            return traces;
        }
    }
}
