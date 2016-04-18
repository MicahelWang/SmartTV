using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.ManagerTest
{
    [TestClass]
    public class DeviceAppsMonitorManagerTest
    {
        private Mock<IDeviceAppsMonitorRepertory> mockDeviceAppsMonitorRepertory;
        private Mock<IAppLibraryManager> mockAppLibraryManager;
        private IDeviceAppsMonitorManager deviceAppsMonitorManager;

        [TestInitialize]
        public void Setup()
        {
            mockDeviceAppsMonitorRepertory = new Mock<IDeviceAppsMonitorRepertory>();
            mockAppLibraryManager = new Mock<IAppLibraryManager>();

            deviceAppsMonitorManager = new DeviceAppsMonitorManager(mockDeviceAppsMonitorRepertory.Object, mockAppLibraryManager.Object);
        }

        [TestMethod]
        public void AddDeviceAppsMonitor_ShouldAddDeviceAppsMonitor_WhenGiveDeviceAppsMonitor()
        {
            // Arrange
            var exceptedAppId = "TestAppId";

            var exceptedDeviceAppsMonitor = new DeviceAppsMonitor
            {
                Active = true,
                Id = exceptedAppId,
                DeviceSeries = "TestDeviceSeries"
            };

            var actual = new List<DeviceAppsMonitor>();

            mockDeviceAppsMonitorRepertory.Setup(m => m.Insert(exceptedDeviceAppsMonitor))
                .Callback(() =>
                {
                    actual.Add(exceptedDeviceAppsMonitor);
                });

            // Act 
            deviceAppsMonitorManager.Add(exceptedDeviceAppsMonitor);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any(a => a.Id.Equals(exceptedAppId)));
        }

        [TestMethod]
        public void AddDeviceAppsMonitors_ShouldAddDeviceAppsMonitors_WhenGiveDeviceAppsMonitor()
        {
            // Arrange
            var exceptedAppIds = new string[] { "TestAppId1", "TestAppId2" };

            var exceptedDeviceAppsMonitors = new List<DeviceAppsMonitor>
            {
                new DeviceAppsMonitor 
                {
                    Active = true,
                    Id = exceptedAppIds[0],
                    DeviceSeries = "TestDeviceSeries"
                },
                new DeviceAppsMonitor 
                {
                    Active = true,
                    Id = exceptedAppIds[1],
                    DeviceSeries = "TestDeviceSeries"
                }

            };

            var actual = new List<DeviceAppsMonitor>();

            mockDeviceAppsMonitorRepertory.Setup(m => m.Insert(exceptedDeviceAppsMonitors))
                .Callback(() =>
                {
                    actual.AddRange(exceptedDeviceAppsMonitors);
                });

            // Act 
            deviceAppsMonitorManager.Add(exceptedDeviceAppsMonitors);

            // Assert
            Assert.IsNotNull(actual);
            exceptedAppIds.ToList().ForEach(e =>
            {
                Assert.IsTrue(actual.Select(a => a.Id).Contains(e));
            });

        }

        [TestMethod]
        public void SearchDeviceAppsMonitorResponse_ShouldReturnDeviceAppsMonitorResponses_WhenGiveAppListRequestModels()
        {
            // Arrange
            var mockHeader = new RequestHeader
            {
                DEVNO = "TestDEVNO",
            };

            var mockAppListRequestModels = GetMockAppListRequestModels(10);

            var exceptedDeviceAppsMonitorResponses = GetMockDeviceAppsMonitorResponses(20);

            var exceptedDeviceAppsMonitors = mockAppListRequestModels.Select(a => new DeviceAppsMonitor
            {
                PackageName = a.PackageName,
                VersionCode = a.VersionCode,
                Active = true,
                DeviceSeries = mockHeader.DEVNO,
                UpdateTime = DateTime.Now,
                Action = AppActionType.Install.ToString()
            });

            var exceptedCount = exceptedDeviceAppsMonitorResponses.Count - mockAppListRequestModels.Count;

            var actualDeviceAppsMonitors = new List<DeviceAppsMonitor>();

            mockDeviceAppsMonitorRepertory.Setup(m => m.SearchDeviceAppsMonitorResponse(mockHeader.DEVNO,mockAppListRequestModels))
                .Returns(exceptedDeviceAppsMonitorResponses);

            mockDeviceAppsMonitorRepertory.Setup(m => m.Insert(It.IsAny<List<DeviceAppsMonitor>>()))
                .Callback(() =>
                {
                    actualDeviceAppsMonitors.AddRange(exceptedDeviceAppsMonitors);
                });

            mockAppLibraryManager.Setup(m => m.SearchAppPublishs(It.IsAny<AppPublishCriteria>())).Returns(new List<AppPublish>());

            // Act 
            var actual = deviceAppsMonitorManager.SearchDeviceAppsMonitorResponse(mockHeader, mockAppListRequestModels);
            Thread.Sleep(TimeSpan.FromSeconds(5));

            // Assert
            Assert.AreEqual(0, actual.Count);
            Assert.IsTrue(actualDeviceAppsMonitors != null && !actualDeviceAppsMonitors.Any());
        }

        [TestMethod]
        public void SearchDeviceAppsMonitorResponse_ShouldReturnDeviceAppsMonitorResponses_WhenGiveExitAppListRequestModels()
        {
            // Arrange
            var mockHeader = new RequestHeader
            {
                DEVNO = "TestDEVNO",
            };

            var mockAppListRequestModels = GetMockAppListRequestModels(10);

            var exceptedDeviceAppsMonitorResponses = GetMockDeviceAppsMonitorResponses(10);

            var exceptedDeviceAppsMonitors = mockAppListRequestModels.Select(a => new DeviceAppsMonitor
            {
                PackageName = a.PackageName,
                VersionCode = a.VersionCode,
                Active = true,
                DeviceSeries = mockHeader.DEVNO,
                UpdateTime = DateTime.Now,
                Action = AppActionType.Install.ToString()
            });

            var exceptedCount = exceptedDeviceAppsMonitorResponses.Count;

            var actualDeviceAppsMonitors = new List<DeviceAppsMonitor>();

            mockDeviceAppsMonitorRepertory.Setup(m => m.SearchDeviceAppsMonitorResponse(mockHeader.DEVNO,mockAppListRequestModels))
                .Returns(exceptedDeviceAppsMonitorResponses);

            mockDeviceAppsMonitorRepertory.Setup(m => m.Insert(It.IsAny<List<DeviceAppsMonitor>>()))
                .Callback(() =>
                {
                    actualDeviceAppsMonitors.AddRange(exceptedDeviceAppsMonitors);
                });

            mockAppLibraryManager.Setup(m => m.SearchAppPublishs(It.IsAny<AppPublishCriteria>())).Returns(new List<AppPublish>());

            // Act 
            var actual = deviceAppsMonitorManager.SearchDeviceAppsMonitorResponse(mockHeader, mockAppListRequestModels);
            Thread.Sleep(TimeSpan.FromSeconds(5)); 
            
            // Assert
            Assert.AreEqual(0, actual.Count);
            Assert.IsTrue(actualDeviceAppsMonitors != null && !actualDeviceAppsMonitors.Any());
        }

        [TestMethod]
        public void SearchDeviceAppsMonitorResponse_ShouldReturnNull_WhenGiveNotExitAppListRequestModels()
        {
            //Arrange
            var mockHeader = new RequestHeader
            {
                DEVNO = "TestDEVNO",
            };

            var mockAppListRequestModels = GetMockAppListRequestModels(20);

            var exceptedDeviceAppsMonitorResponses = GetMockDeviceAppsMonitorResponses(10);

            var exceptedDeviceAppsMonitors = mockAppListRequestModels.Select(a => new DeviceAppsMonitor
            {
                PackageName = a.PackageName,
                VersionCode = a.VersionCode,
                Active = true,
                DeviceSeries = mockHeader.DEVNO,
                UpdateTime = DateTime.Now,
                Action = AppActionType.Install.ToString()
            });
            var exceptedCount = mockAppListRequestModels.Count - exceptedDeviceAppsMonitorResponses.Count;

            var actualDeviceAppsMonitors = new List<DeviceAppsMonitor>();

            mockDeviceAppsMonitorRepertory.Setup(m => m.SearchDeviceAppsMonitorResponse(mockHeader.DEVNO,mockAppListRequestModels))
                .Returns(exceptedDeviceAppsMonitorResponses);

            mockDeviceAppsMonitorRepertory.Setup(m => m.Insert(It.IsAny<List<DeviceAppsMonitor>>()))
                .Callback(() =>
                {
                    actualDeviceAppsMonitors.AddRange(exceptedDeviceAppsMonitors);
                });

            mockAppLibraryManager.Setup(m => m.SearchAppPublishs(It.IsAny<AppPublishCriteria>())).Returns(new List<AppPublish>());
            //Act
            var actual = deviceAppsMonitorManager.SearchDeviceAppsMonitorResponse(mockHeader, mockAppListRequestModels);
            Thread.Sleep(TimeSpan.FromSeconds(5));

            //Assert
            Assert.AreEqual(0, actual.Count);
            Assert.IsTrue(actualDeviceAppsMonitors != null && !actualDeviceAppsMonitors.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void SearchDeviceAppsMonitorResponse_ShouldThrowCommonFrameworkManagerException_WhenAnyExceptionHappend()
        {
            // Arrange

            var mockHeader = new RequestHeader
            {
                DEVNO = "TestDEVNO",
            };

            var mockAppListRequestModels = GetMockAppListRequestModels(10);

            var exceptedDeviceAppsMonitorResponses = GetMockDeviceAppsMonitorResponses(20);

            mockDeviceAppsMonitorRepertory.Setup(m => m.SearchDeviceAppsMonitorResponse(mockHeader.DEVNO,mockAppListRequestModels))
               .Callback(() =>
               {
                   throw new CommonFrameworkManagerException("设置APP版本失败", null);
               });

            mockDeviceAppsMonitorRepertory.Setup(m => m.Insert(It.IsAny<List<DeviceAppsMonitor>>()))
               .Callback(() =>
               {
                   throw new CommonFrameworkManagerException("设置APP版本失败", null);
               });

            // Act 

            deviceAppsMonitorManager.SearchDeviceAppsMonitorResponse(mockHeader, mockAppListRequestModels);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        private List<AppListRequestModel> GetMockAppListRequestModels(int count)
        {
            var appListRequestModels = new List<AppListRequestModel>();

            for (int i = 0; i < count; i++)
            {
                appListRequestModels.Add(
                new AppListRequestModel
                {
                    PackageName = "TestPackageName" + i,
                    VersionCode =  i,
                });
            };

            return appListRequestModels;
        }

        private List<DeviceAppsMonitoApiMode> GetMockDeviceAppsMonitorResponses(int count)
        {
            var deviceAppsMonitorResponses = new List<DeviceAppsMonitoApiMode>();

            for (int i = 0; i < count; i++)
            {
                deviceAppsMonitorResponses.Add(
                new DeviceAppsMonitoApiMode
                {
                    PackageName = "TestPackageName" + i,
                    VersionCode =i,
                    DeviceSeries = "TestDeviceSeries" + i
                });
            };

            return deviceAppsMonitorResponses;
        }
    }
}
