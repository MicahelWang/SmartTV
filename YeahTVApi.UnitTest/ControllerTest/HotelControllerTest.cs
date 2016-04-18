using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.Common;
using YeahTVApi.Controllers;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApi.UnitTests.ControllerTest
{
    [TestClass]
    public class HotelControllerTest
    {
        private Mock<IRedisCacheManager> mockRedisCacheManager;
        private Mock<IRequestApiService> mockRequestApiService;
        private Mock<IHttpContextService> mockHttpContextService;
        private Mock<ILogManager> mockLogManager;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private Mock<ITVHotelConfigManager> mockTVHotelConfigManager;
        private HotelController hotelController;

        [TestInitialize]
        public void Setup()
        {
            mockRedisCacheManager = new Mock<IRedisCacheManager>();
            mockRequestApiService = new Mock<IRequestApiService>();
            mockHttpContextService = new Mock<IHttpContextService>();
            mockLogManager = new Mock<ILogManager>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            mockTVHotelConfigManager = new Mock<ITVHotelConfigManager>();

            var mockHeader = new RequestHeader
            {
                HotelID = "TestHotelId"
            };
            mockHttpContextService.Setup(m => m.Current.Items[RequestParameter.Header]).Returns(mockHeader);

            hotelController = new HotelController(mockRequestApiService.Object,
                mockHttpContextService.Object,
                mockLogManager.Object, 
                mockConstantSystemConfigManager.Object,
                mockRedisCacheManager.Object,
                mockTVHotelConfigManager.Object);
        }

        [TestMethod]
        public void GetHotelDetail_ShouldReturnHotelDetail_WhenGiveExitHotelId()
        {
            // Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;
            var mockUrl = "TestConfigValue/";

            mockConstantSystemConfigManager.Setup(m => m.AppCenterUrl).Returns(mockUrl);

            var url = mockConstantSystemConfigManager.Object.AppCenterUrl + Constant.GetHotelApiUrl + mockHeader.HotelID;

            var exceptedHotelObject = new HotelObject
            {
                Brand = new CoreSysBrand
                {
                    BrandCode = "TestBrandCode"
                },
                Group = new CoreSysGroup
                {
                    Id = "TestCoreSysGroupId"
                },
                Hotel = new HotelEntity
                {
                    GroupId = "TestGroupId",
                    HotelId = mockHeader.HotelID
                }
            };

            mockRequestApiService.Setup(m => m.HttpRequest(url, "DETAIL"))
                .Returns(exceptedHotelObject.ToJsonString());

            // Act 
            var actual = hotelController.GetHotelDetail().obj;

            // Assert
            Assert.AreEqual(exceptedHotelObject.ToJsonString(), actual.ToJsonString());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetHotelDetail_ShouldThrowException_WhenGiveNotExitSystemConfigKey()
        {
            // Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;

            mockConstantSystemConfigManager.Setup(m => m.AppCenterUrl)
                .Callback(() =>
                {
                    throw new Exception();
                });

            // Act 
            var actual = hotelController.GetHotelDetail();
        }

        [TestMethod]
        public void GetHotelDetail_ShouldReturnNull_WhenGiveExitHotelId()
        {
            // Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;
            var mockUrl = "TestConfigValue/";

            mockConstantSystemConfigManager.Setup(m => m.AppCenterUrl).Returns(mockUrl);

            var url = mockConstantSystemConfigManager.Object.AppCenterUrl + Constant.GetHotelApiUrl + mockHeader.HotelID;

            mockRequestApiService.Setup(m => m.HttpRequest(url, "DETAIL"))
                .Returns(string.Empty);

            // Act 
            var actual = hotelController.GetHotelDetail().obj;

            // Assert
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void GetWeather_ShouldReturnGetWeather_WhenGiveCityName()
        {
            // Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;
            var mockWeatherDate = new List<WeatherData>
            {
                new WeatherData
                {
                    Date = "TestDate",
                    DayPictureUrl = "TestDayPictureUrl"
                }
            };

            var mockUrl = "TestConfigValue/";
            mockConstantSystemConfigManager.Setup(m => m.AppCenterUrl).Returns(mockUrl);

            var url = mockConstantSystemConfigManager.Object.AppCenterUrl + Constant.GetHotelApiUrl + mockHeader.HotelID;

            var mockHotel = new HotelEntity
            {
                City = 0,
            };

            mockRequestApiService.Setup(m => m.Get(url))
               .Returns(mockHotel.ToJsonString());

            mockRedisCacheManager.Setup(m => m.Get<List<WeatherData>>(Constant.SystemWeatherKey + mockHotel.City.ToString())).Returns(mockWeatherDate);
            // Act 
            var actual = hotelController.GetWeather().obj;

            // Assert
            Assert.AreEqual(mockWeatherDate.ToJsonString(), actual.ToJsonString());
        }

        [TestMethod]
        public void GetWeather_ShouldReturnNull_WhenGiveCityName()
        {
            // Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;

            var mockUrl = "TestConfigValue/";
            mockConstantSystemConfigManager.Setup(m => m.AppCenterUrl).Returns(mockUrl);

            var url = mockConstantSystemConfigManager.Object.AppCenterUrl + Constant.GetHotelApiUrl + mockHeader.HotelID;

            var mockHotel = new HotelEntity
            {
                City = 0,
            };

            mockRequestApiService.Setup(m => m.Get(url))
               .Returns(mockHotel.ToJsonString());

            mockRedisCacheManager.Setup(m => m.Get<List<WeatherData>>(Constant.SystemWeatherKey + mockHotel.City.ToString())).Returns(new List<WeatherData>());
           
            // Act 
            var actual = hotelController.GetWeather().obj;


            // Assert
            Assert.IsFalse((actual as List<WeatherData>).Any());
        }

        [TestMethod]
        public void GetGetBaseData_ShouldReturnGetGetBaseData_ByHotelId()
        {
            mockConstantSystemConfigManager.Setup(m => m.AppCenterUrl).Returns("http://10.4.26.95:16528/");
            var abc =hotelController.GetBaseData("0f710609748e40adba345078d6369f8b");
            Assert.AreEqual(0, 0);
        }
    }
}