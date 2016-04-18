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

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.ManagerTest
{
    [TestClass]
    public class DeviceTraceManagerTest
    {
        private Mock<IDeviceTraceLibraryRepertory> mockTraceRepertory;
        private Mock<IBackupDeviceManager> mockBackupDeviceManager;
        private Mock<IAppLibraryManager> mockAppsManager;
        private Mock<IAppPublishLibraryRepertory> mockAppPublishRepertory;
        private IDeviceTraceLibraryManager deviceTraceLibraryManager;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private List<DeviceTrace> mockTraces;
        private Mock<IMongoDeviceTraceRepository> mockMongoDeviceTraceRepository;

        [TestInitialize]
        public void Setup()
        {
            mockTraces = GetMockDeviceTraces(100);
            mockTraceRepertory = new Mock<IDeviceTraceLibraryRepertory>();
            mockAppsManager = new Mock<IAppLibraryManager>();
            mockAppPublishRepertory = new Mock<IAppPublishLibraryRepertory>();
            mockBackupDeviceManager = new Mock<IBackupDeviceManager>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            mockMongoDeviceTraceRepository = new Mock<IMongoDeviceTraceRepository>();
            deviceTraceLibraryManager = new DeviceTraceManager(mockTraceRepertory.Object,
                mockAppsManager.Object, mockAppPublishRepertory.Object,
                mockBackupDeviceManager.Object, mockRedisCacheService.Object,
                mockMongoDeviceTraceRepository.Object);
        }

        [TestMethod]
        public void SearchTraces_ShouldReturnTraces_WhenGiveValidCriteria()
        {
            // Arrange
            var criteria = new TraceCriteria();
            criteria.DeviceSeries = "TestDeviceSeries1";
            criteria.Platfrom = "TestPlatfrom1";
            criteria.HotelId = "Test1";

            var excepted = mockTraces.Where(m => m.DeviceSeries.Contains(criteria.DeviceSeries)
                && m.Platfrom.Contains(criteria.Platfrom)
                && m.HotelId.Contains(criteria.HotelId)).ToList();

            mockTraceRepertory.Setup(m => m.Search(criteria)).Returns(excepted);

            // Act 
            var actual = deviceTraceLibraryManager.Search(criteria);


        }

        [TestMethod]
        public void SearchTraces_ShouldReturnTwentyTraces_WhenGiveValidCriteriaNeedPageing()
        {
            // Arrange
            var criteria = new TraceCriteria();
            criteria.DeviceSeries = "TestDeviceSeries";
            criteria.Platfrom = "TestPlatfrom";
            criteria.HotelId = "Test";
            criteria.NeedPaging = true;

            var excepted = mockTraces.Where(m => m.DeviceSeries.Contains(criteria.DeviceSeries)
                && m.Platfrom.Contains(criteria.Platfrom)
                && m.HotelId.Contains(criteria.HotelId))
                .Page(criteria.PageSize, criteria.Page).ToList();

            mockTraceRepertory.Setup(m => m.Search(criteria)).Returns(excepted);

            // Act 
            var actual = deviceTraceLibraryManager.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.Count, actual.Count);
        }

        [TestMethod]
        public void SearchTraces_ShouldReturnNull_WhenTraceIsNotExit()
        {
            // Arrange
            var criteria = new TraceCriteria();
            criteria.DeviceSeries = "TestDeviceSeries1ss";
            criteria.Platfrom = "TestPlatfrom1sss";
            criteria.HotelId = "Test1sss";

            mockTraceRepertory.Setup(m => m.Search(criteria)).Returns(new List<DeviceTrace>());

            // Act 
            var actual = deviceTraceLibraryManager.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void AddTrace_ShouldSuccess_WhenAddDeviceTrace()
        {
            // Arrange
            var mockTrace = new DeviceTrace
            {
                Active = true,
                Brand = "TestBrandInsert",
                DeviceKey = "TestDeviceKeyInsert",
                DeviceSeries = "TestDeviceSeriesInsert",
                FirstVisitTime = DateTime.Now,
                GuestId = "TestGuestIdInsert",
                HotelId = "TestInsert",
                Ip = "",
                LastVisitTime = DateTime.Now,
                Manufacturer = "TestManufacturerInsert",
                Model = "TestModelInsert",
                Platfrom = "TestPlatfromInsert",
                ModelId = 0,
                OsVersion = "TestOsVersionInsert",
                Remark = "",
                RoomNo = "",
                Token = "",
            };

            mockTraceRepertory.Setup(m => m.Insert(mockTrace)).Callback(() =>
            {
                mockTraces.Add(mockTrace);
            });
            mockTraceRepertory.Setup(m => m.GetAll()).Returns(new List<DeviceTrace>());
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<DeviceTrace>(), It.IsAny<Func<List<DeviceTrace>>>()));
            mockTraceRepertory.Setup(m => m.Search(It.IsAny<TraceCriteria>())).Returns(new List<DeviceTrace> { mockTrace });

            // Act 
            deviceTraceLibraryManager.Add(mockTrace);

            // Assert
            Assert.IsTrue(mockTraces.Any(m => m.DeviceSeries == mockTrace.DeviceSeries));
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void AddTrace_ShouldFail_WhenRepertoryHasException()
        {
            // Arrange
            var mockTrace = new DeviceTrace
            {
                Active = true,
                Brand = "TestBrandInsert",
                DeviceKey = "TestDeviceKeyInsert",
                DeviceSeries = "TestDeviceSeriesInsert",
                FirstVisitTime = DateTime.Now,
                GuestId = "TestGuestIdInsert",
                HotelId = "TestInsert",
                Ip = "",
                LastVisitTime = DateTime.Now,
                Manufacturer = "TestManufacturerInsert",
                Model = "TestModelInsert",
                Platfrom = "TestPlatfromInsert",
                ModelId = 0,
                OsVersion = "TestOsVersionInsert",
                Remark = "",
                RoomNo = "",
                Token = "",
            };

            mockTraceRepertory.Setup(m => m.Insert(mockTrace)).Callback(() =>
            {
                throw new CommonFrameworkManagerException("AddTrace error!", null);
            });

            // Act 
            deviceTraceLibraryManager.Add(mockTrace);
        }

        [TestMethod]
        public void UpdateTrace_ShouldSuccess_WhenUpdateDeviceTrace()
        {
            // Arrange
            var mockTrace = new DeviceTrace
            {
                Active = true,
                Brand = "TestBrandInsert",
                DeviceKey = "TestDeviceKeyInsert",
                DeviceSeries = "TestDeviceSeriesInsert",
                FirstVisitTime = DateTime.Now,
                GuestId = "TestGuestIdInsert",
                HotelId = "TestHotelIdInsert",
                Ip = "",
                LastVisitTime = DateTime.Now,
                Manufacturer = "TestManufacturerInsert",
                Model = "TestModelInsert",
                Platfrom = "TestPlatfromInsert",
                ModelId = 0,
                OsVersion = "TestOsVersionInsert",
                Remark = "TestRemarkInsert",
                RoomNo = "TestRoomNoInsert",
                Token = "",
            };

            mockTraceRepertory.Setup(m => m.Update(mockTrace)).Callback(() =>
            {
                mockTraces.SingleOrDefault(m => m.DeviceSeries == "TestDeviceSeries1").RoomNo = mockTrace.RoomNo;
                mockTraces.SingleOrDefault(m => m.DeviceSeries == "TestDeviceSeries1").HotelId = mockTrace.HotelId;
                mockTraces.SingleOrDefault(m => m.DeviceSeries == "TestDeviceSeries1").Remark = mockTrace.Remark;
            });

            mockTraceRepertory.Setup(m => m.Search(It.IsAny<TraceCriteria>())).Returns(mockTraces);
            mockTraceRepertory.Setup(m => m.GetSingle(It.IsAny<TraceCriteria>())).Returns(mockTraces.FirstOrDefault());
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<DeviceTrace>>>())).Returns(new List<DeviceTrace>() { 
            mockTrace
            });
            // Act 
            deviceTraceLibraryManager.Update(mockTrace);

            // Assert
            Assert.IsTrue(mockTraces.Any(m => m.RoomNo == mockTrace.RoomNo));
            Assert.IsTrue(mockTraces.Any(m => m.HotelId == mockTrace.HotelId));
            Assert.IsTrue(mockTraces.Any(m => m.Remark == mockTrace.Remark));
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void UpdateTrace_ShouldFail_WhenRepertoryHasException()
        {
            // Arrange
            var mockTrace = new DeviceTrace
            {
                Active = true,
                Brand = "TestBrandInsert",
                DeviceKey = "TestDeviceKeyInsert",
                DeviceSeries = "TestDeviceSeriesInsert",
                FirstVisitTime = DateTime.Now,
                GuestId = "TestGuestIdInsert",
                HotelId = "TestInsert",
                Ip = "",
                LastVisitTime = DateTime.Now,
                Manufacturer = "TestManufacturerInsert",
                Model = "TestModelInsert",
                Platfrom = "TestPlatfromInsert",
                ModelId = 0,
                OsVersion = "TestOsVersionInsert",
                Remark = "",
                RoomNo = "",
                Token = "",
            };

            mockTraceRepertory.Setup(m => m.GetSingle(It.IsAny<TraceCriteria>())).Callback(() =>
            {
                throw new CommonFrameworkManagerException("UpdateTrace error!", null);
            });

            // Act 
            deviceTraceLibraryManager.Update(mockTrace);
        }


        [TestMethod]
        public void GetAppTrace_ShouldReturnTraces_WhenGiveValidRequestHeader()
        {
            // Arrange
            var excepted = new DeviceTrace
            {
                DeviceSeries = "DEVNO"
            };

            var mockHeader = new RequestHeaderBase
            {
                APP_ID = "appId",
                DEVNO = "DEVNO"
            };

            var mockAppsCriteria = new TraceCriteria { DeviceSeries = mockHeader.DEVNO, IsTVTrace = true };

            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<DeviceTrace>>>())).Returns(new List<DeviceTrace>() { excepted });
            mockTraceRepertory.Setup(m => m.GetSingle(It.IsAny<TraceCriteria>())).Returns(excepted);

            // Act 
            var actual = deviceTraceLibraryManager.GetAppTrace(mockHeader);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.DeviceSeries, actual.DeviceSeries);
        }

        [TestMethod]
        public void GetAppTrace_ShouldReturnNull_WhenGiveNotExitRequestHeader()
        {
            // Arrange
            var mockHeader = new RequestHeaderBase
            {
                APP_ID = "appId",
                DEVNO = "DEVNO"
            };

            var mockAppsCriteria = new TraceCriteria { DeviceSeries = mockHeader.DEVNO, IsTVTrace = true };

            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<DeviceTrace>>>())).Returns(new List<DeviceTrace>() );
            mockTraceRepertory.Setup(m => m.GetSingle(It.IsAny<TraceCriteria>())).Returns(new DeviceTrace());

            // Act 
            var actual = deviceTraceLibraryManager.GetAppTrace(mockHeader);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetAppKey_ShouldReturnAppKey_WhenGiveValidRequestHeader()
        {
            //Arrange
            var excepted = "TestDeviceKey";

            var mockDeviceTraces = new List<DeviceTrace>
            {
                new DeviceTrace
                {
                    DeviceKey = excepted,
                    Active = true,
                    DeviceSeries = "TestDevno"
                }
            };

            var mockHeader = new RequestHeaderBase
            {
                Ver = "TestVer",
                DEVNO = "TestDevno"
            };
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<DeviceTrace>>>())).Returns(mockDeviceTraces);
            mockTraceRepertory.Setup(m => m.Search(It.IsAny<TraceCriteria>())).Returns(mockDeviceTraces);

            //Act
            var actual = deviceTraceLibraryManager.GetAppKey(mockHeader);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void GetAppTrace_ShouldThrowCommonFrameworkManagerException_WhenGiveNotExitAppId()
        {
            // Arrange
            var mockHeader = new RequestHeaderBase
            {
                APP_ID = "",
                DEVNO = ""
            };

            var mockAppsCriteria = new TraceCriteria { DeviceSeries = mockHeader.DEVNO, IsTVTrace = true };

            mockTraceRepertory.Setup(m => m.GetSingle(It.IsAny<TraceCriteria>())).Returns(new DeviceTrace());

            // Act 
            var actual = deviceTraceLibraryManager.GetAppTrace(mockHeader);
        }

        [TestMethod]
        public void GetAppKey_ShouldReturnNull_WhenGiveNotExitRequestHeader()
        {
            //Arrange
            var mockDeviceTraces = new List<DeviceTrace>
            {
                new DeviceTrace
                {
                    Active = true,
                }
            };
            var mockHeader = new RequestHeaderBase
            {
                Ver = "TestVer",
                DEVNO = "TestDEVNO"
            }; 

            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<DeviceTrace>>>())).Returns(mockDeviceTraces);
            mockTraceRepertory.Setup(m => m.Search(It.IsAny<TraceCriteria>())).Returns(mockDeviceTraces);

            //Act

            var actual = deviceTraceLibraryManager.GetAppKey(mockHeader);

            //Assert
            Assert.IsNull(actual);
            Assert.AreEqual(null, actual);
        }
        private List<DeviceTrace> GetMockDeviceTraces(int count)
        {
            var traces = new List<DeviceTrace>();

            for (int i = 0; i < count; i++)
            {
                traces.Add(new DeviceTrace
                {
                    Active = true,
                    Brand = "TestBrand" + i,
                    DeviceKey = "TestDeviceKey" + i,
                    DeviceSeries = "TestDeviceSeries" + i,
                    FirstVisitTime = DateTime.Now,
                    GuestId = "TestGuestId" + i,
                    HotelId = "Test" + i,
                    Ip = "",
                    LastVisitTime = DateTime.Now,
                    Manufacturer = "TestManufacturer" + i,
                    Model = "TestModel" + i,
                    Platfrom = "TestPlatfrom" + i,
                    ModelId = 0,
                    OsVersion = "TestOsVersion" + i,
                    Remark = "",
                    RoomNo = "",
                    Token = "",
                });
            }

            return traces;
        }
    }
}
