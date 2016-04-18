using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.ManagerTest
{
    [TestClass]
    public class CacheManagerTest
    {
        private Mock<IAppsLibraryRepertory> mockAppsRepertory;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private Mock<ILogManager> mockLogManager;
        private Mock<IRequestApiService> mockRequestApiService;
        private Mock<ISystemConfigManager> mockSystemConfigManager;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private ICacheManager cacheManager;

        [TestInitialize]
        public void Setup()
        {
            mockAppsRepertory = new Mock<IAppsLibraryRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            mockLogManager = new Mock<ILogManager>();
            mockRequestApiService = new Mock<IRequestApiService>();
            mockSystemConfigManager = new Mock<ISystemConfigManager>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();

            cacheManager = new CacheManager(mockRedisCacheService.Object,
                mockAppsRepertory.Object,
                mockLogManager.Object,
                mockRequestApiService.Object,
                mockSystemConfigManager.Object,
                mockConstantSystemConfigManager.Object);

            mockConstantSystemConfigManager.Setup(m => m.AppCenterUrl).Returns("TestAppCenterUrl");
            mockLogManager.Setup(m => m.SaveError(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<AppType>(), It.IsAny<string>()))
                .Callback(() => 
            {
            
            });
            mockLogManager.Setup(m => m.SaveInfo(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<AppType>(), It.IsAny<string>()))
               .Callback(() =>
               {

               });
        }

        [TestMethod]
        public void SetWeather_ShouldSetWeaherSueccess_WhenGiveExitCitys()
        {
            //Arrange
            var mockCities = new List<CoreSysCity>
            {
              new CoreSysCity { Id = 73, Name = "上海" }
            };

            var mockHotels = new List<HotelEntity>
            {
                new HotelEntity{ City = 73}
            };

            var mockWeathers = new List<WeatherData> 
            {
                new WeatherData
                {
                    Date = "TestDate",
                    DayPictureUrl = "TestDayPictureUrl"
                }
            };

            var expected = new List<WeatherData>();

            mockRequestApiService.Setup(m => m.Get(mockConstantSystemConfigManager.Object.AppCenterUrl + Constant.GetCitiesUrl))
                .Returns(mockCities.ToJsonString());

            mockRequestApiService.Setup(m => m.HttpRequest(mockConstantSystemConfigManager.Object.AppCenterUrl + Constant.GetHotelApiUrl, "GetAll"))
              .Returns(mockHotels.ToJsonString());

            mockRedisCacheService.Setup(m => m.Set(Constant.SystemWeatherKey + mockCities.FirstOrDefault().Id.ToString(), It.IsAny<List<WeatherData>>()))
                .Callback(() => 
                {
                    expected = mockWeathers;
                });

            //Act
            cacheManager.SetWeather();

            //Assert
            Assert.AreEqual(expected, mockWeathers);
        }

        [TestMethod]
        public void SetAppsList_ShouldSetAppsListSueccess_WhenGiveExitApps()
        {
            //Arrange
            var mockApps = new List<Apps>
            {
                new Apps
                {
                    Id = "TestId",
                    AppKey = "TestAppKey",
                    Description = "TestDescription",
                }
            };

            var expected =  new Dictionary<string, Apps>();
            mockApps.ForEach(a =>
            {
                expected.Add(a.Id, a);
            });

            var actual = new Dictionary<string, Apps>();

            mockAppsRepertory.Setup(m => m.Search(It.IsAny<AppsCriteria>())).Returns(mockApps);
            mockRedisCacheService.Setup(m => m.Set(Constant.AppsListKey, expected)).Callback(() => 
            {
                actual = expected;
            });

            //Act
            cacheManager.SetAppsList();
            
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
