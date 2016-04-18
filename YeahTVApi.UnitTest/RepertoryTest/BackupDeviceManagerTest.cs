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

        //初始化
        [TestInitialize]
        public void Initial()
        {
            mockTraces = GetMockDeviceTraces(10);
            mockBackupDeviceRepertory = new Mock<IBackupDeviceRepertory>();
            this.backupDeviceManager = new BackupDeviceManager(mockBackupDeviceRepertory.Object);
        }


        [TestMethod]
        public void UpdateBackupDevice_ShouldReturnSueccess_WhenBackupDeviceIsNotNull()
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
                 mockTraces.SingleOrDefault(m => m.Id == 1).DeviceSeries = backupdevice.DeviceSeries;
             });
            mockBackupDeviceRepertory.Setup(m => m.FindByKey(It.IsAny<string>())).Returns(mockTraces.FirstOrDefault());

         
             //Act
             backupDeviceManager.UpdateBackupDevice(backupdevice);

             //Assert
             Assert.IsTrue(mockTraces.Any(m => m.DeviceSeries == backupdevice.DeviceSeries)); 

            
        }

        [TestMethod]
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
            backupDeviceManager.UpdateBackupDevice(backupdevice);
        }


        private List<BackupDevice> GetMockDeviceTraces(int count)
        {
            var traces = new List<BackupDevice>();

            for (int i = 0; i < count; i++)
            {
                traces.Add(new BackupDevice
                {

                    HotelId = "TestHotelId",
                    DeviceSeries = "backupDevice" + i,
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
