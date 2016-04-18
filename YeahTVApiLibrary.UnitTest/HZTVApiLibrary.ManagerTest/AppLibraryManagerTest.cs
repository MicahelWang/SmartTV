using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.ManagerTest
{
    [TestClass]
    public class AppLibraryManagerTest
    {
        private Mock<IDeviceTraceLibraryRepertory> mockTraceRepertory;
        private Mock<IBackupDeviceManager> mockBackupDeviceManager;
        private Mock<IAppsLibraryRepertory> mockAppsRepertory;
        private Mock<IAppVersionLibraryRepertory> mocAppVersionLibraryRepertory;
        private Mock<IRedisCacheManager> mockRedisCacheManager;
        private Mock<IAppPublishLibraryRepertory> mockAppPublishLibraryRepertory;
        private Mock<ICacheManager> mockCacheManager;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private IAppLibraryManager appLibraryManager;
        private List<Apps> mockApps;
        private List<AppVersion> mockAppVersions;
        private List<AppPublish> mockAppPublishs;

        [TestInitialize]
        public void Setup()
        {
            mockTraceRepertory = new Mock<IDeviceTraceLibraryRepertory>();
            mockAppsRepertory = new Mock<IAppsLibraryRepertory>();
            mockRedisCacheManager = new Mock<IRedisCacheManager>();
            mocAppVersionLibraryRepertory = new Mock<IAppVersionLibraryRepertory>();
            mockRedisCacheManager = new Mock<IRedisCacheManager>();
            mockAppPublishLibraryRepertory = new Mock<IAppPublishLibraryRepertory>();
            mockCacheManager = new Mock<ICacheManager>();
            mockBackupDeviceManager = new Mock<IBackupDeviceManager>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            mockApps = GetMockApps(10);
            mockAppVersions = GetMockAppVersions(10);
            mockAppPublishs = GetMockAppPublishs(10);


            appLibraryManager = new AppLibraryManager(mockAppsRepertory.Object,
                mockRedisCacheManager.Object,
                mockAppPublishLibraryRepertory.Object,
                mocAppVersionLibraryRepertory.Object, mockCacheManager.Object,
                mockBackupDeviceManager.Object,
                mockRedisCacheService.Object);
        }

        [TestMethod]
        public void GetAppByAppId_ShouldReturnApp_WhenGiveExitAppId()
        {
            // Arrange
            var exceptedAppId = "TestAppId";
            var mockApps = new List<Apps>
            {
                new Apps
                {
                    Active = true,
                    AppKey = "TestAppKey",
                    Id = exceptedAppId
                }
            };

            mockRedisCacheManager.Setup(m => m.IsSet(It.IsAny<string>())).Returns(true);
            mockRedisCacheManager.Setup(m => m.Get<Dictionary<string, Apps>>(It.IsAny<string>())).Returns(mockApps.ToDictionary(m => m.Id));
            mockAppsRepertory.Setup(m => m.Search(It.IsAny<AppsCriteria>())).Returns(mockApps);

            // Act 
            var actual = appLibraryManager.GetAppByAppId(exceptedAppId);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(exceptedAppId, actual.Id);
        }

        [TestMethod]
        public void GetAppByAppId_ShouldReturnNull_WhenGiveNotExitAppId()
        {
            // Arrange
            var exceptedAppId = "TestAppId";

            mockAppsRepertory.Setup(m => m.Search(It.IsAny<AppsCriteria>())).Returns(new List<Apps>());
            mockRedisCacheManager.Setup(m => m.IsSet(It.IsAny<string>())).Returns(true);
            mockRedisCacheManager.Setup(m => m.Get<Dictionary<string, Apps>>(It.IsAny<string>())).Returns(mockApps.ToDictionary(m => m.Id));

            // Act 
            var actual = appLibraryManager.GetAppByAppId(exceptedAppId);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void SearchApps_ShouldReturnApps_WhenGiveValidCriteria()
        {
            // Arrange
            var exceptedAppId = "TestAppId";
            var mockApps = new List<Apps>
            {
                new Apps
                {
                    Active = true,
                    AppKey = "TestAppKey",
                    Id = exceptedAppId
                }
            };

            var mockAppsCriteria = new AppsCriteria
            {
                Id = exceptedAppId,
            };

            mockAppsRepertory.Setup(m => m.Search(mockAppsCriteria)).Returns(mockApps);

            // Act 
            var actual = appLibraryManager.Search(mockAppsCriteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(exceptedAppId, actual.FirstOrDefault().Id);
        }

        [TestMethod]
        public void SearchApps_ShouldReturnNull_WhenGiveNotExitCriteria()
        {
            // Arrange
            var exceptedAppId = "TestAppIdxxxx";

            var mockAppsCriteria = new AppsCriteria
            {
                Id = exceptedAppId,
            };

            mockAppsRepertory.Setup(m => m.Search(mockAppsCriteria)).Returns(new List<Apps>());

            // Act 
            var actual = appLibraryManager.Search(mockAppsCriteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }


        [TestMethod]
        public void GetAppVersionList_ShouldReturnVersionList()
        {
            //Arrange
            var expected = new Dictionary<string, Apps>();
            var list = new List<Apps>
            {
                new Apps
                {
                    Id = "TestAppId",
                    Active = true,
                    
                }
            };
            list.ForEach(a =>
            {
                expected.Add(a.Id, a);
            });
            mockAppsRepertory.Setup(m => m.Search(It.IsAny<AppsCriteria>())).Returns(list);
            mockRedisCacheManager.Setup(m => m.IsSet(It.IsAny<string>())).Returns(true);
            mockRedisCacheManager.Setup(m => m.Get<Dictionary<string, Apps>>(It.IsAny<string>())).Returns(expected);

            //Act
            var actual = appLibraryManager.GetAppVersionList();
            //Assert
            Assert.IsNotNull(actual);

            foreach (var e in expected)
            {
                Assert.IsTrue(actual.Any(a => a.Key == e.Key));
            }
        }

        [TestMethod]
        public void QueryHotel_ShouldReturnHotel_WhenGiveValidHotelID()
        {
            //Arrange
            var HotelId = "HotelID";

            var expected = new Hotel()
            {
                hotelName = "TestHotelName",
                hotelStyle = "TestHotelStyle",
                hotelID = HotelId
            };

            mockRedisCacheManager.Setup(m => m.GetCache(Constant.HtotelCacheKey + HotelId)).Returns(expected.ToJsonString());

            //Act
            var actual = appLibraryManager.QueryHotel(HotelId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ToJsonString(), actual.ToJsonString());
        }

        [TestMethod]
        public void GetAppVersionList_ShouldEmptyList_WhenSearchReturnEmpty()
        {
            //Arrange
            var expected = new Dictionary<string, Apps>();

            mockAppsRepertory.Setup(m => m.Search(It.IsAny<AppsCriteria>())).Returns(new List<Apps>());
            mockRedisCacheManager.Setup(m => m.IsSet(It.IsAny<string>())).Returns(true);
            mockRedisCacheManager.Setup(m => m.Get<Dictionary<string, Apps>>(It.IsAny<string>())).Returns(expected);

            //Act
            var actual = appLibraryManager.GetAppVersionList();

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void SearchAppsFromCache_ShouldReturnApps_WhenGiveValidCriteria()
        {
            //Arrange
            var appName = "TestAppName3";
            var appVersion = "";
            var packageName = "TestPackageName3";
            var expectedList = mockApps.Where(m => m.Name == appName).ToList();

            mockRedisCacheManager.Setup(m => m.Get<Dictionary<string, Apps>>(Constant.AppsListKey)).Returns(mockApps.ToDictionary(a => a.Id));

            //Act
            var actual = appLibraryManager.SearchAppsFromCache(appName, appVersion, packageName);

            //Assert
            Assert.IsNotNull(actual);
            expectedList.ForEach(m =>
            {
                Assert.IsTrue(actual.Contains(m));
            });
            Assert.AreEqual(expectedList.Count, actual.Count);
        }

        [TestMethod]
        public void SearchAppsFromCache_ShouldReturnEmptyList_WhenGiveNotExistAppName()
        {
            //Arrange
            var appName = "TestAppName30";
            var appVersion = "";

            mockRedisCacheManager.Setup(m => m.Get<Dictionary<string, Apps>>(Constant.AppsListKey)).Returns(() => { return null; });

            //Act
            var actual = appLibraryManager.SearchAppsFromCache(appName, appVersion);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void QueryHotel_ShouldReturnNull_WhenGiveNotExistHotelID()
        {
            //Arrange
            var HotelId = "HotelID222";

            var expected = new Hotel()
            {
                hotelName = "TestHotelName",
                hotelStyle = "TestHotelStyle",
                hotelID = HotelId
            };

            mockRedisCacheManager.Setup(m => m.GetCache(Constant.HtotelCacheKey + HotelId)).Returns(() => { return null; });

            //Act
            var actual = appLibraryManager.QueryHotel(HotelId);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void AddApp_ShouldSuccess_WhenGiveValidApps()
        {
            //Arrange
            var expected = new Apps()
            {
                Id = "TestAppsIdNewId",
                Name = "TestAppsName"
            };

            mockAppsRepertory.Setup(m => m.Insert(It.IsAny<Apps>())).Callback<Apps>(m => mockApps.Add(m));
            mockAppsRepertory.Setup(m => m.GetAll()).Returns(new List<Apps>());

            //Act
            appLibraryManager.Add(expected);

            //Assert
            Assert.IsNotNull(mockApps.FirstOrDefault(m => m.Id == expected.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void AddApp_ShouldFail_WhenGiveErrorApps()
        {
            //Arrange
            var expected = new Apps();

            mockAppsRepertory.Setup(m => m.Insert(expected)).Callback(() =>
            {
                throw new CommonFrameworkManagerException("Apps error!", null);
            });

            //Act
            appLibraryManager.Add(expected);
        }

        [TestMethod]
        public void UpdateApp_ShouldSuccess_WhenGiveValidApps()
        {
            //Arrange
            var expected = new Apps()
            {
                Id = "TestAppId1",
                Name = "TestNewAppsName"
            };

            mockAppsRepertory.Setup(m => m.Update(It.IsAny<Apps>())).
                Callback<Apps>(m => mockApps.FirstOrDefault(a => a.Id == m.Id).Name = m.Name);
            mockAppsRepertory.Setup(m => m.GetAll()).Returns(new List<Apps>()); ;

            mockRedisCacheManager.Setup(m => m.IsSet(It.IsAny<string>())).Returns(true);
            mockRedisCacheManager.Setup(m => m.Get<Dictionary<string, Apps>>(It.IsAny<string>())).Returns(mockApps.ToDictionary(m => m.Id));

            //Act
            appLibraryManager.Update(expected);

            //Assert
            Assert.AreEqual(expected.Name, mockApps.FirstOrDefault(m => m.Id == expected.Id).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void UpdateApp_ShouldFail_WhenGiveErrorApps()
        {
            //Arrange
            var expected = new Apps() { Id = mockApps.First().Id };

            mockAppsRepertory.Setup(m => m.Update(expected)).Callback(() =>
            {
                throw new CommonFrameworkManagerException("Apps error!", null);
            });


            mockRedisCacheManager.Setup(m => m.IsSet(It.IsAny<string>())).Returns(true);
            mockRedisCacheManager.Setup(m => m.Get<Dictionary<string, Apps>>(It.IsAny<string>())).Returns(mockApps.ToDictionary(m => m.Id));

            //Act
            appLibraryManager.Update(expected);
        }

        [TestMethod]
        public void SearchAppVersions_ShouldReturnAppVersions_WhenGiveValidCriteria()
        {
            //Arrange
            var mockAppsCriteria = new AppsCriteria
            {
                AppName = "TestAppName3",
            };
            var expectedList = mockAppVersions.Where(m => m.App.Name == mockAppsCriteria.AppName).ToList();

            mocAppVersionLibraryRepertory.Setup(m => m.Search(It.IsAny<BaseSearchCriteria>())).
                Returns<AppsCriteria>(m => mockAppVersions.Where(a => a.App.Name == m.AppName).ToList());

            //Act
            var actual = appLibraryManager.SearchAppVersions(mockAppsCriteria);

            //Assert
            Assert.IsNotNull(actual);
            expectedList.ForEach(m =>
            {
                Assert.IsTrue(actual.Contains(m));
            });
            Assert.AreEqual(expectedList.Count, actual.Count);
        }

        [TestMethod]
        public void SearchAppVersions_ShouldReturnEmptyList_WhenGiveNotExistAppName()
        {
            //Arrange
            var mockAppsCriteria = new AppsCriteria
            {
                AppName = "TestAppName33333",
            };

            mocAppVersionLibraryRepertory.Setup(m => m.Search(It.IsAny<BaseSearchCriteria>())).
                Returns<AppsCriteria>(m => mockAppVersions.Where(a => a.App.Name == m.AppName).ToList());

            //Act
            var actual = appLibraryManager.SearchAppVersions(mockAppsCriteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void AddVersion_ShouldSuccess_WhenGiveValidAppVersion()
        {
            //Arrange
            var expected = new AppVersion()
            {
                Id = "TestAppVersionIdNewId",
                VersionName = "TestVersionName"
            };

            mocAppVersionLibraryRepertory.Setup(m => m.Insert(It.IsAny<AppVersion>())).
                Callback<AppVersion>(m => mockAppVersions.Add(m));
            mocAppVersionLibraryRepertory.Setup(m => m.GetAll()).Returns(new List<AppVersion>());
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<AppVersion>(), It.IsAny<Func<List<AppVersion>>>()));
            mocAppVersionLibraryRepertory.Setup(m => m.Search(It.IsAny<AppsCriteria>())).Returns(new List<AppVersion>() { expected });

            //Act
            appLibraryManager.AddVersion(expected);

            //Assert
            Assert.IsNotNull(mockAppVersions.FirstOrDefault(m => m.Id == expected.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void AddVersion_ShouldFail_WhenGiveErrorVersion()
        {
            //Arrange
            var expected = new AppVersion();

            mocAppVersionLibraryRepertory.Setup(m => m.Insert(expected)).
                Throws(new CommonFrameworkManagerException("AppVersion error!", null));

            //Act
            appLibraryManager.AddVersion(expected);
        }

        [TestMethod]
        public void UpdateAppVersion_ShouldSuccess_WhenGiveValidAppVersion()
        {
            //Arrange
            var expected = mockAppVersions.First();
            expected.VersionName = "TestVersionName";

            mocAppVersionLibraryRepertory.Setup(m => m.Search(It.IsAny<BaseSearchCriteria>())).
                Returns<AppsCriteria>(m => mockAppVersions.Where(a => a.Id == m.Id).ToList());
            mocAppVersionLibraryRepertory.Setup(m => m.Update(It.IsAny<AppVersion>())).
                Callback<AppVersion>(m => mockAppVersions.FirstOrDefault(a => a.Id == m.Id).VersionName = m.VersionName);

            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<AppVersion>>>()))
                .Returns(mockAppVersions);
            mockRedisCacheService.Setup(m => m.RemoveItemFromSet(It.IsAny<string>(), It.IsAny<object>()));
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<List<object>>>()));

            //Act
            appLibraryManager.UpdateAppVersion(expected);

            //Assert
            Assert.AreEqual(expected.VersionName, mockAppVersions.FirstOrDefault(m => m.Id == expected.Id).VersionName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateAppVersion_ShouldThorwArgumentNullException_WhenGiveNotExistAppVersionId()
        {
            //Arrange
            var expected = new AppVersion()
            {
                Id = "TestAppVersionIdNewId",
                VersionName = "TestVersionName"
            };

            mocAppVersionLibraryRepertory.Setup(m => m.Search(It.IsAny<BaseSearchCriteria>())).
                Returns<AppsCriteria>(m => { return null; });
            mocAppVersionLibraryRepertory.Setup(m => m.Update(It.IsAny<AppVersion>())).
                Callback<AppVersion>(m => mockAppVersions.FirstOrDefault(a => a.Id == m.Id).VersionName = m.VersionName);

            //Act
            appLibraryManager.UpdateAppVersion(expected);
        }

        [TestMethod]
        public void SearchAppPublishs_ShouldReturnAppPublishs_WhenGiveValidCriteria()
        {
            //Arrange
            var mockAppPublishCriteria = new AppPublishCriteria
            {
                Id = "TestAppPublishId3"
            };
            var expectedList = mockAppPublishs.Where(m => m.Id == mockAppPublishCriteria.Id).ToList();

            mockAppPublishLibraryRepertory.Setup(m => m.Search(It.IsAny<BaseSearchCriteria>())).
                Returns<AppPublishCriteria>(m => mockAppPublishs.Where(a => a.Id == m.Id).ToList());

            //Act
            var actual = appLibraryManager.SearchAppPublishs(mockAppPublishCriteria);

            //Assert
            Assert.IsNotNull(actual);
            expectedList.ForEach(m =>
            {
                Assert.IsTrue(actual.Contains(m));
            });
            Assert.AreEqual(expectedList.Count, actual.Count);
        }

        [TestMethod]
        public void SearchAppPublishs_ShouldReturnEmptyList_WhenGiveNotExistAppName()
        {
            //Arrange
            var mockAppPublishCriteria = new AppPublishCriteria
            {
                Id = "TestAppPublishId3222"
            };
            var expectedList = mockAppPublishs.Where(m => m.Id == mockAppPublishCriteria.Id).ToList();

            mockAppPublishLibraryRepertory.Setup(m => m.Search(It.IsAny<BaseSearchCriteria>())).
                Returns<AppPublishCriteria>(m => mockAppPublishs.Where(a => a.Id == m.Id).ToList());

            //Act
            var actual = appLibraryManager.SearchAppPublishs(mockAppPublishCriteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void AddPublish_ShouldSuccess_WhenGiveValidAddPublish()
        {
            //Arrange
            var expected = new AppPublish()
            {
                Id = "TestAppPublishNewId",
                VersionCode = 1,
                HotelId = "TestHotelId"
            };

            mockAppPublishLibraryRepertory.Setup(m => m.Insert(It.IsAny<AppPublish>())).
                Callback<AppPublish>(m => mockAppPublishs.Add(m));

            mockAppPublishLibraryRepertory.Setup(m => m.GetAll()).Returns(new List<AppPublish>());
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<AppPublish>(), It.IsAny<Func<List<AppPublish>>>()));
            mockAppPublishLibraryRepertory.Setup(m => m.Search(It.IsAny<AppPublishCriteria>())).Returns(new List<AppPublish> { expected });

            //Act
            appLibraryManager.AddPublish(expected);

            //Assert
            Assert.IsNotNull(mockAppPublishs.FirstOrDefault(m => m.Id == expected.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void AddPublish_ShouldFail_WhenGiveErrorVersion()
        {
            //Arrange
            var expected = new AppPublish();

            mockAppPublishLibraryRepertory.Setup(m => m.Insert(expected)).
                Throws(new CommonFrameworkManagerException("AppPublish error!", null));

            //Act
            appLibraryManager.AddPublish(expected);
        }

        [TestMethod]
        public void UpdateAppPublish_ShouldSuccess_WhenGiveValidAppPublish()
        {
            //Arrange
            var expected = mockAppPublishs.First();
            expected.VersionCode = 1;

            mockAppPublishLibraryRepertory.Setup(m => m.Search(It.IsAny<BaseSearchCriteria>())).
                Returns(mockAppPublishs.Where(a => a.Id == expected.Id).ToList());
            mockAppPublishLibraryRepertory.Setup(m => m.Update(It.IsAny<AppPublish>())).
                Callback<AppPublish>(m => mockAppPublishs.FirstOrDefault(a => a.Id == m.Id).VersionCode = m.VersionCode);
            mockAppPublishLibraryRepertory.Setup(m => m.GetAll()).Returns(new List<AppPublish>());

            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<AppPublish>>>()))
                .Returns(mockAppPublishs);
            mockRedisCacheService.Setup(m => m.RemoveItemFromSet(It.IsAny<string>(), It.IsAny<object>()));
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<List<object>>>()));

            //Act
            appLibraryManager.UpdateAppPublish(expected);

            //Assert
            Assert.AreEqual(expected.VersionCode, mockAppPublishs.FirstOrDefault(m => m.Id == expected.Id).VersionCode);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateAppPublish_ShouldThorwArgumentNullException_WhenGiveNotExistAppPublishId()
        {
            //Arrange

            mockAppPublishLibraryRepertory.Setup(m => m.Update(It.IsAny<AppPublish>())).
                Callback<AppPublish>(m => mockAppPublishs.FirstOrDefault(a => a.Id == m.Id).VersionCode = m.VersionCode);

            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<AppPublish>>>()))
                .Returns(mockAppPublishs);
            mockRedisCacheService.Setup(m => m.RemoveItemFromSet(It.IsAny<string>(), It.IsAny<object>()));
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<Func<List<object>>>()));


            //Act
            appLibraryManager.UpdateAppPublish(null);
        }

        [TestMethod]
        public void GetAppBackupDevice_ShouldReturnNull_WhenGiveNotExitRequestHeader()
        {
            // Arrange
            var mockHeader = new RequestHeaderBase
            {
                APP_ID = "appId",
                DEVNO = "DEVNO"
            };

            mockBackupDeviceManager.Setup(m => m.SearchFromCache(It.IsAny<BackupDeviceCriteria>())).Returns(new List<BackupDevice>() { new BackupDevice() { DeviceSeries = mockHeader.DEVNO } });

            // Act 
            var actual = appLibraryManager.GetAppBackupDevice(mockHeader);

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void GetAppBackupDevice_ShouldThrowCommonFrameworkManagerException_WhenGiveNotExitAppId()
        {
            // Arrange
            var mockHeader = new RequestHeaderBase
            {
                APP_ID = "",
                DEVNO = ""
            };

            mockBackupDeviceManager.Setup(m => m.SearchFromCache(It.IsAny<BackupDeviceCriteria>())).Returns(new List<BackupDevice>());

            // Act 
            appLibraryManager.GetAppBackupDevice(mockHeader);
        }

        [TestMethod]
        public void GetAppBackupDevice_ShouldReturnBackupDevice_WhenGiveValidRequestHeader()
        {
            // Arrange
            var excepted = new BackupDevice
            {
                DeviceSeries = "DEVNO"
            };

            var mockHeader = new RequestHeaderBase
            {
                APP_ID = "appId",
                DEVNO = "DEVNO"
            };


            mockBackupDeviceManager.Setup(m => m.SearchFromCache(It.IsAny<BackupDeviceCriteria>())).Returns(new List<BackupDevice>() { excepted });

            // Act 
            var actual = appLibraryManager.GetAppBackupDevice(mockHeader);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.DeviceSeries, actual.DeviceSeries);
        }

        private List<Apps> GetMockApps(int count)
        {
            var apps = new List<Apps>();
            for (int i = 0; i < count; i++)
            {
                apps.Add(new Apps
                {
                    Id = "TestAppId" + i,
                    AppKey = "TestAppKey" + i,
                    Name = "TestAppName" + i,
                    CreateTime = DateTime.Now,
                    PackageName = "TestPackageName" + i,
                    AppVresions = new List<AppVersion>() { 
                     new AppVersion(){
                      VersionCode=i
                     }                    
                    }
                });
            }
            return apps;
        }
        private List<AppVersion> GetMockAppVersions(int count)
        {
            var appVersions = new List<AppVersion>();
            for (int i = 0; i < count; i++)
            {
                appVersions.Add(new AppVersion
                {
                    Id = "TestAppVersionId" + i,
                    VersionName = "TestVersionName" + i,
                    VersionCode =  i,
                    App = new Apps()
                    {
                        Id = "TestAppId" + i,
                        AppKey = "TestAppKey" + i,
                        Name = "TestAppName" + i,
                        CreateTime = DateTime.Now,
                        PackageName = "TestPackageName" + i
                    }
                });
            }
            return appVersions;
        }
        private List<AppPublish> GetMockAppPublishs(int count)
        {
            var appPublishs = new List<AppPublish>();
            for (int i = 0; i < count; i++)
            {
                appPublishs.Add(new AppPublish
                {
                    Id = "TestAppPublishId" + i,
                    VersionCode =  i,
                    PublishDate = DateTime.Now,
                    HotelId = "TestHotelId" + i
                });
            }
            return appPublishs;
        }
    }
}
