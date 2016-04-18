using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.Mapping;
using YeahTVApi.DomainModel.Models.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Infrastructure;
using Moq;
using YeahTVApi.Controllers;
using YeahTVApi.Entity;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApiLibrary.Service;
using YeahTVApi.DomainModel.Models;


namespace YeahTVApi.UnitTests.ControllerTest
{
    [TestClass]
    public class AuthForAppTest
    {
        private Mock<IAppLibraryManager> mockApplibrarymanager;
        private Mock<IHttpContextService> mockHttpContext;
        private Mock<ITraceManager> mockTraceManager;
        private Mock<ILogManager> mockLogManager;
        private Mock<IRedisCacheManager> mockRedisCacheManager;
        private Mock<IDeviceAppsMonitorManager> mockDeviceAppsMonitorManager;
        private Mock<ITVHotelConfigManager> mockTVHotelConfigManager;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private Mock<IHttpContextService> mockHttpContextService;
        private Mock<IRequestApiService> mockRequestApiService;

        private AppController appController;
        [TestInitialize]
        public void Setup()
        {
            mockApplibrarymanager = new Mock<IAppLibraryManager>();
            mockHttpContext = new Mock<IHttpContextService>();
            mockTraceManager = new Mock<ITraceManager>();
            mockLogManager = new Mock<ILogManager>();
            mockRedisCacheManager = new Mock<IRedisCacheManager>();
            mockRequestApiService = new Mock<IRequestApiService>();
            mockDeviceAppsMonitorManager = new Mock<IDeviceAppsMonitorManager>();
            mockTVHotelConfigManager = new Mock<ITVHotelConfigManager>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            mockHttpContextService = new Mock<IHttpContextService>();
            var mockHeader = new RequestHeader
            {
                HotelID = "TestHotelId",
                DEVNO = "Test"
            };
            mockHttpContextService.Setup(m => m.Current.Items[RequestParameter.Header]).Returns(mockHeader);

            appController = new AppController(mockTraceManager.Object, mockHttpContext.Object
                , mockLogManager.Object, mockRedisCacheManager.Object, mockRequestApiService.Object, mockDeviceAppsMonitorManager.Object,
                mockTVHotelConfigManager.Object, mockApplibrarymanager.Object, mockConstantSystemConfigManager.Object);

            appController.HttpContextService = mockHttpContextService.Object;
        }

        [TestMethod]
        public void TestAuthForApp()
        {
            // Arrange
            var expectedMsgResult = new MsgResult()
            {
                Msg = "TestMsg",
                Data = "TestData"
            };
            var password = "testPassword";
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;
            mockConstantSystemConfigManager.Setup(m => m.OpenApiAddress).Returns("http://testcenter.yeah-info.com:8088/");

            mockRequestApiService.Setup(m => m.HttpRequest(It.IsAny<string>(), It.Is<string>(s => s == "Post"))).Returns(expectedMsgResult.ToJsonString());

            //Act
            var actual = appController.GetAuthForApp(password);
            var actualApiResult = actual as ApiObjectResult<object>;

            // Assert
            Assert.IsNotNull(actualApiResult);
            Assert.AreEqual(expectedMsgResult.Data, actualApiResult.obj);
        }

        [TestMethod]
        public void GetallHotel()
        {
            // Arrange
            var expectedLatitude = 31.170651m;
            var expectedLongitude = 121.401264m;
            var ip = "http://10.4.26.95:16528/";

            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;
            var hotelList = new List<HotelEntity>() {
                    new HotelEntity(){
                         HotelId="TestHotelId",
                         HotelName="TestHotelName",
                          Latitude=31.170651m,
                          Longitude=121.401264m
                    }                
                };
            var brand = new CoreSysBrand()
            {
                Id = "TestBrandId",
                BrandName = "TestBrandName"
            };
            mockConstantSystemConfigManager.Setup(m => m.AppCenterUrl).Returns(ip);
            mockConstantSystemConfigManager.Setup(m => m.ResourceSiteAddress).Returns(ip);
            mockLogManager.Setup(m => m.SaveInfo(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<DomainModel.Enum.AppType>(), It.IsAny<string>()));
            mockRequestApiService.Setup(m => m.HttpRequest(It.IsAny<string>(), It.Is<string>(s => s == "GetAll"))).Returns(hotelList.ToJsonString());
            mockRequestApiService.Setup(m => m.Get(It.Is<string>(s => s.Contains("api/Brand/")))).Returns(brand.ToJsonString());


            //Act
            var actual = appController.GetHotelsByLocation(expectedLatitude.ToString(), expectedLongitude.ToString(), "500");
            var actualList = actual as ApiListResult<SimpleHotelEntity>;

            //Assert
            Assert.IsNotNull(actualList);
            Assert.AreEqual(hotelList.Count, actualList.list.Count);
        }


    }
}
