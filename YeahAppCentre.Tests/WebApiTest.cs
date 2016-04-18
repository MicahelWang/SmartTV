using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Service;
using Moq;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Tests
{
    [TestClass]
    public class WebApiTest
    {
        private const string BaseUrl = "http://10.4.26.95:16528/";

        private Mock<IRequestApiService> mockRequestApiService;


        [TestInitialize]
        public void Setup()
        {
            mockRequestApiService = new Mock<IRequestApiService>();
        }

        [TestMethod]
        public void GroupGetTest()
        {
            //Arrange
            const string path = "api/group/";
            const string url = BaseUrl + path;
            var groupList = new List<CoreSysGroup>() {
                    new CoreSysGroup(){
                     Id="TestGroupId",
                      GroupName="TestGroupName"
                    }                
                };

            mockRequestApiService.Setup(m => m.Get(It.IsAny<string>())).Returns(() =>
            {
                return groupList.ToJsonString();
            });

            //Act
            var act = mockRequestApiService.Object.Get(url);
            var entity = act.JsonStringToObj<List<CoreSysGroup>>();

            //Assert
            Assert.IsNotNull(act);
            Assert.IsNotNull(entity);
            Assert.AreEqual(groupList.Count, entity.Count);

        }

        [TestMethod]
        public void BrandGetByGroupTest()
        {
            //Arrange
            const string groupId = "fc9e7ad9f2ea498f869adce5ad0652e0";
            string path = string.Format("api/brand/{0}", groupId);
            string url = BaseUrl + path;
            const string method = "ByGroup";
            var brandList=new List<CoreSysBrand>() {
                    new CoreSysBrand(){
                     Id="TestBrandId",
                      BrandName="TestBrandName"
                    }                
                };

            mockRequestApiService.Setup(m => m.HttpRequest(It.IsAny<string>(), method)).Returns(() =>
            {
                return brandList.ToJsonString();
            });

            //Act
            var act = mockRequestApiService.Object.HttpRequest(url, method);
            var entity = act.JsonStringToObj<List<CoreSysBrand>>();

            //Assert
            Assert.IsNotNull(act);
            Assert.IsNotNull(entity);
            Assert.AreEqual(brandList.Count, entity.Count);
        }


        [TestMethod]
        public void HotelGetByByBrandTest()
        {
            //Arrange
            const string brandId = "ad5fba9865db461c98a60094723a6925";
            string path = string.Format("api/Hotel/{0}", brandId);
            var service = new RequestApiService();
            string url = BaseUrl + path;
            const string method = "ByBrand";
            var hotelList = new List<HotelEntity>() {
                    new HotelEntity(){
                         HotelId="TestHotelId",
                         HotelName="TestHotelName"
                    }                
                };

            mockRequestApiService.Setup(m => m.HttpRequest(It.IsAny<string>(), method)).Returns(() =>
            {
                return hotelList.ToJsonString();
            });

            //Act
            var act = mockRequestApiService.Object.HttpRequest(url, method);
            var entity = act.JsonStringToObj<List<HotelEntity>>();

            //Assert
            Assert.IsNotNull(act);
            Assert.IsNotNull(entity);
            Assert.AreEqual(hotelList.Count, entity.Count);
        }
    }
}
