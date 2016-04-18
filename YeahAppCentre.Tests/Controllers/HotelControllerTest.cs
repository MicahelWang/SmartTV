using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YeahAppCentre.Controllers;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Tests.Controllers
{
    [TestClass]
    public class HotelControllerTest
    {
        private HotelController hotelController;
        private Mock<IHotelManager> mockManager;
        private Mock<ITvTemplateManager> mockTvTemplateManager;
        private Mock<IBrandManager> mockBrandManager;
        private Mock<IGroupManager> mockGroupManager;
        private Mock<IProvinceManager> mockProvinceManager;
        private Mock<ICityManager> mockCityManager;
        private Mock<ICountyManager> mockCountyManager;
        private Mock<IHttpContextService> mockHttpContextService;
        private List<CoreSysHotel> mockCoreSysHotels;
        private List<CoreSysGroup> mockCoreSysGroups;
        private List<CoreSysBrand> mockCoreSysBrands;

        [TestInitialize]
        public void Setup()
        {
            mockCoreSysHotels = GetMockCoreSysHotels(30);
            mockCoreSysGroups = GetMockCoreSysGroups(30);
            mockCoreSysBrands = GetMockCoreSysBrands(30);
            mockManager = new Mock<IHotelManager>();
            mockTvTemplateManager = new Mock<ITvTemplateManager>();
            mockBrandManager = new Mock<IBrandManager>();
            mockGroupManager = new Mock<IGroupManager>();
            mockProvinceManager = new Mock<IProvinceManager>();
            mockCityManager = new Mock<ICityManager>();
            mockCountyManager = new Mock<ICountyManager>();
            mockHttpContextService = new Mock<IHttpContextService>();
            hotelController = new HotelController(
                mockManager.Object,
                mockTvTemplateManager.Object,
                mockBrandManager.Object,
                mockGroupManager.Object,
                mockProvinceManager.Object,
                mockCityManager.Object,
                mockCountyManager.Object,
                mockHttpContextService.Object);
        }

        [TestMethod]
        public void Index_ShouldReturnList_WhenGiveValidCriteria()
        {
            //Arrange
            var expectedHotels = mockCoreSysHotels.OrderBy(m => m.Id).Take(15).ToList();

            var hotelCriteria = new CoreSysHotelCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 0,
                OrderAsc = true,
                SortFiled = "Id"
            };
            mockManager.Setup(m => m.Search(hotelCriteria)).Returns(expectedHotels);
            mockGroupManager.Setup(m => m.GetAll()).Returns(mockCoreSysGroups);
            //mockHttpContextService.Setup(m => m.Current).Returns(new Mock<HttpContextBase>().Object);
            //mockHttpContextService.Setup(m => m.Current.Session[Constant.SessionKey.CurrentUser]).Returns(
            //  new CurrentUser() {
            //            Account = "testAccount1"
            //        });

            //Act
            var actual = hotelController.Index(hotelCriteria) as ViewResult;
            var hotelList = actual.ViewBag.List as PagedViewList<CoreSysHotel>;

            //Assert
            Assert.IsNotNull(actual.ViewBag.HotelCriteria);
            Assert.IsNotNull(actual.ViewBag.Groups);
            Assert.IsNotNull(actual.ViewBag.DefaultSelect);
            Assert.IsNotNull(actual.ViewBag.List);
            expectedHotels.ForEach(m =>
            {
                Assert.IsTrue(hotelList.Source.Contains(m));
            });
            Assert.AreEqual(expectedHotels.Count, hotelList.Source.Count);
        }

        [TestMethod]
        public void Index_ShouldReturnEmptyList_WhenGiveErrorPage()
        {
            //Arrange
            var expectedHotels = mockCoreSysHotels.OrderBy(m => m.Id).Take(15).ToList();
            var hotelCriteria = new CoreSysHotelCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 10,
                OrderAsc = true,
                SortFiled = "Id"
            };

            mockManager.Setup(m => m.Search(hotelCriteria)).Returns(() => new List<CoreSysHotel>());
            mockGroupManager.Setup(m => m.GetAll()).Returns(mockCoreSysGroups);

            //Act
            var actual = hotelController.Index(hotelCriteria) as ViewResult;
            var hotelList = actual.ViewBag.List as PagedViewList<CoreSysHotel>;

            //Assert
            Assert.IsNotNull(actual.ViewBag.HotelCriteria);
            Assert.IsNotNull(actual.ViewBag.Groups);
            Assert.IsNotNull(actual.ViewBag.DefaultSelect);
            Assert.AreEqual(0, hotelList.Source.Count);
        }

        [TestMethod]
        public void GetCoreSysHotelList_ShouldReturnHotels_WhenGiveValidCriteria()
        {
            //Arrange
            var expectedHotels = mockCoreSysHotels.OrderBy(m => m.Id).Take(15).ToList();
            var hotelCriteria = new CoreSysHotelCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 0,
                OrderAsc = true,
                SortFiled = "Id"
            };

            mockManager.Setup(m => m.Search(hotelCriteria)).Returns(() => expectedHotels);
            mockGroupManager.Setup(m => m.GetAll()).Returns(mockCoreSysGroups);

            //Act
            var actual = hotelController.List(hotelCriteria) as PartialViewResult;
            var hotelList = actual.Model as PagedViewList<CoreSysHotel>;

            //Assert            
            expectedHotels.ForEach(m =>
            {
                Assert.IsTrue(hotelList.Source.Contains(m));
            });
            Assert.AreEqual(expectedHotels.Count, hotelList.Source.Count);
        }

        [TestMethod]
        public void GetCoreSysHotelList_ShouldReturnEmptyList_WhenGiveErrorPage()
        {
            //Arrange
            var expectedHotels = mockCoreSysHotels.OrderBy(m => m.Id).Take(15).ToList();
            var hotelCriteria = new CoreSysHotelCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 10,
                OrderAsc = true,
                SortFiled = "Id"
            };

            mockManager.Setup(m => m.Search(hotelCriteria)).Returns(() => new List<CoreSysHotel>());
            mockGroupManager.Setup(m => m.GetAll()).Returns(mockCoreSysGroups);

            //Act
            var actual = hotelController.List(hotelCriteria) as PartialViewResult;
            var hotelList = actual.Model as PagedViewList<CoreSysHotel>;

            //Assert            
            Assert.AreEqual(0, hotelList.Source.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void GetCoreSysHotelList_ShouldThrowException_WhenGiveNotExistSortFiled()
        {
            //Arrange
            var expectedHotels = mockCoreSysHotels.OrderBy(m => m.Id).Take(15).ToList();
            var hotelCriteria = new CoreSysHotelCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 10,
                OrderAsc = true,
                SortFiled = "NoExistId"
            };

            mockManager.Setup(m => m.Search(hotelCriteria)).Throws(new Exception("SortFiled Error!"));
            mockGroupManager.Setup(m => m.GetAll()).Returns(mockCoreSysGroups);

            //Act
            var actual = hotelController.List(hotelCriteria) as PartialViewResult;
        }

        [TestMethod]
        public void HttpGetEdit_ShouldReturnHotel_WhenGiveValidAddType()
        {
            //Arrange
            var expectedHotel = new CoreSysHotel();

            mockTvTemplateManager.Setup(m => m.GetAll()).Returns(() => new List<TvTemplate>());
            mockCountyManager.Setup(m => m.GetAll()).Returns(() => new List<CoreSysCounty>());

            //Act
            var actual = hotelController.Edit(string.Empty, YeahTVApi.DomainModel.Enum.OpType.Add) as ViewResult;

            //Assert
            Assert.IsNotNull(actual.Model);
        }

        [TestMethod]
        public void HttpGetEdit_ShouldReturnHotel_WhenGiveExistHotelId()
        {
            //Arrange
            var expectedHotel = mockCoreSysHotels.First();

            mockTvTemplateManager.Setup(m => m.GetAll()).Returns(() => new List<TvTemplate>());
            mockCountyManager.Setup(m => m.GetAll()).Returns(() => new List<CoreSysCounty>());
            mockManager.Setup(m => m.GetCoreSysHotelById(expectedHotel.Id)).Returns(expectedHotel);

            //Act
            var actual = hotelController.Edit(expectedHotel.Id, YeahTVApi.DomainModel.Enum.OpType.Update) as ViewResult;
            var actualHotel = actual.Model as CoreSysHotel;

            //Assert
            Assert.IsNotNull(actual.ViewBag.CountyName);
            Assert.IsNotNull(actualHotel);
            Assert.AreSame(expectedHotel, actualHotel);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void HttpGetEdit_ShouldThorwNullReferenceException_WhenGiveNotExistHotelId()
        {
            //Arrange
            var expectedHotelId = "NotExistHotelId";

            mockTvTemplateManager.Setup(m => m.GetAll()).Returns(() => new List<TvTemplate>());
            mockCountyManager.Setup(m => m.GetAll()).Returns(() => new List<CoreSysCounty>());
            mockManager.Setup(m => m.GetCoreSysHotelById(It.IsAny<string>())).Returns(() => null);

            //Act
            var actual = hotelController.Edit(expectedHotelId, YeahTVApi.DomainModel.Enum.OpType.Update) as ViewResult;
            var actualHotel = actual.Model as CoreSysHotel;
        }


        [TestMethod]
        public void HttpPostEdit_ShouldReturnSuccess_WhenGiveValidCoreSysHotel()
        {
            //Arrange
            var expectedHotel = new CoreSysHotel()
            {
                HotelName = "TestHotelName99",
                BrandId = "TestBrandId1",
                Country = 1,
                CoreSysHotelSencond = new CoreSysHotelSencond()
                {
                    WelcomeWord = "TestWelcomWord"
                }
            };

            mockGroupManager.Setup(m => m.GetAll()).Returns(mockCoreSysGroups);
            mockBrandManager.Setup(m => m.GetBrand(It.IsAny<string>())).Returns(() =>
                    new CoreSysBrand()
                    {
                        Id = "TestBrandId1",
                        GroupId = "TestGroupId1"
                    });
            mockCountyManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(() =>
                new CoreSysCounty()
                {
                    Id = 1,
                    ParentId = 1
                });
            mockCityManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(() =>
                new CoreSysCity()
                {
                    Id = 1,
                    ParentId = 1
                });
            mockProvinceManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(() =>
                new CoreSysProvince()
                {
                    Id = 1,
                    ParentId = 0
                });

            mockManager.Setup(m => m.Add(expectedHotel)).Returns<CoreSysHotel>(m => m.Id);

            //Act
            var actual = hotelController.Edit(YeahTVApi.DomainModel.Enum.OpType.Add, expectedHotel) as ContentResult;

            //Assert
            Assert.AreEqual("Success", actual.Content);
            Assert.IsFalse(string.IsNullOrWhiteSpace(expectedHotel.Id));
        }

        [TestMethod]
        public void HttpPostEdit_ShouldThorwNullErrorMessage_WhenNotGiveCoreSysHotelSencond()
        {
            //Arrange
            var expectedHotel = new CoreSysHotel()
            {
                HotelName = "TestHotelName99",
                BrandId = "TestBrandId1",
                Country = 1,
                CoreSysHotelSencond = null
            };

            mockGroupManager.Setup(m => m.GetAll()).Returns(mockCoreSysGroups);
            mockBrandManager.Setup(m => m.GetBrand(It.IsAny<string>())).Returns(() =>
                    new CoreSysBrand()
                    {
                        Id = "TestBrandId1",
                        GroupId = "TestGroupId1"
                    });
            mockCountyManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(() =>
                new CoreSysCounty()
                {
                    Id = 1,
                    ParentId = 1
                });
            mockCityManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(() =>
                new CoreSysCity()
                {
                    Id = 1,
                    ParentId = 1
                });
            mockProvinceManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(() =>
                new CoreSysProvince()
                {
                    Id = 1,
                    ParentId = 0
                });

            mockManager.Setup(m => m.Add(expectedHotel)).Returns<CoreSysHotel>(m => m.Id);

            //Act
            var actual = hotelController.Edit(YeahTVApi.DomainModel.Enum.OpType.Add, expectedHotel) as ContentResult;

            //Assert
            Assert.AreEqual("扩展信息不能为空！", actual.Content);
        }


        [TestMethod]
        public void HttpPostEdit_ShouldUpdateSuccess_WhenGiveValidCoreSysHotel()
        {
            //Arrange
            var expectedHotelId = "TestHotelId2";
            var expectedHotel = new CoreSysHotel()
            {
                Id = expectedHotelId,
                HotelName = "TestHotelName99",
                BrandId = "TestBrandId1",
                Country = 1,
                CoreSysHotelSencond = new CoreSysHotelSencond()
                {
                    Id = expectedHotelId,
                    WelcomeWord = "Update Info"
                }
            };

            mockGroupManager.Setup(m => m.GetAll()).Returns(mockCoreSysGroups);
            mockBrandManager.Setup(m => m.GetBrand(It.IsAny<string>())).Returns(() =>
                    new CoreSysBrand()
                    {
                        Id = "TestBrandId1",
                        GroupId = "TestGroupId1"
                    });
            mockCountyManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(() =>
                new CoreSysCounty()
                {
                    Id = 1,
                    ParentId = 1
                });
            mockCityManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(() =>
                new CoreSysCity()
                {
                    Id = 1,
                    ParentId = 1
                });
            mockProvinceManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(() =>
                new CoreSysProvince()
                {
                    Id = 1,
                    ParentId = 0
                });

            mockManager.Setup(m => m.Update(expectedHotel)).Callback(() =>
            {

                mockCoreSysHotels.First(m => m.Id == expectedHotelId).CoreSysHotelSencond.
                    WelcomeWord = expectedHotel.CoreSysHotelSencond.WelcomeWord;
            });


            //Act
            var actual = hotelController.Edit(YeahTVApi.DomainModel.Enum.OpType.Update, expectedHotel) as ContentResult;

            //Assert
            Assert.AreEqual("Success", actual.Content);
            Assert.AreEqual(expectedHotel.CoreSysHotelSencond.WelcomeWord,
                mockCoreSysHotels.First(m => m.Id == expectedHotelId).CoreSysHotelSencond.WelcomeWord);
        }


        [TestMethod]
        public void BatchDeleteHotels_ShouldReturnSuccess_WhenGiveExistHotelIds()
        {
            //Arrange
            var expectedHotelIds = mockCoreSysHotels.Select(m => m.Id).ToArray();


            mockManager.Setup(m => m.BatchDelete(expectedHotelIds)).Callback((string[] ids) =>
            {
                mockCoreSysHotels.Where(m => ids.Contains(m.Id)).ToList().ForEach(m => mockCoreSysHotels.Remove(m));
            });

            //Act
            var actual = hotelController.BatchDelete(string.Join(",", expectedHotelIds)) as ContentResult;


            //Assert            
            Assert.AreEqual("Success", actual.Content);
            Assert.AreEqual(0, mockCoreSysHotels.Count);
        }

        [TestMethod]
        public void GetBrandsByGroup_ShouldReturnBrandsJson_WhenGiveExistGroupId()
        {
            //Arrange
            var expectedBrandId = "TestGroupId1";
            var expectedBrands = mockCoreSysBrands.Where(m => m.GroupId == expectedBrandId).ToJsonString();

            mockBrandManager.Setup(m => m.GetBrandsByGroup(expectedBrandId)).Returns<string>(id =>
                {
                    return mockCoreSysBrands.Where(m => m.GroupId == id).ToList();
                });

            //Act
            var actual = hotelController.GetBrandsByGroup(expectedBrandId) as JsonResult;

            //Assert            
            Assert.AreEqual(expectedBrands, actual.Data.ToJsonString());
        }

        [TestMethod]
        public void GetBrandsByGroup_ShouldReturnEmpty_WhenGiveNotExistGroupId()
        {
            //Arrange
            var expectedBrandId = "TestGroupId99";

            mockBrandManager.Setup(m => m.GetBrandsByGroup(expectedBrandId)).Returns<string>(id =>
            {
                return mockCoreSysBrands.Where(m => m.GroupId == id).ToList();
            });

            //Act
            var actual = hotelController.GetBrandsByGroup(expectedBrandId) as JsonResult;

            //Assert            
            Assert.AreEqual(0, ((List<CoreSysBrand>)actual.Data).Count);
        }


        [TestMethod]
        public void GetAreasJson_ShouldReturnAreasJson()
        {
            //Arrange
            mockCountyManager.Setup(m => m.GetAll()).Returns(() =>
              new List<CoreSysCounty>() 
              { 
                  new CoreSysCounty()
                  {
                       Id=1,
                       ParentId=1
                  }
              });
            mockCityManager.Setup(m => m.GetAll()).Returns(() =>
              new List<CoreSysCity>() 
              { 
                  new CoreSysCity()
                  {
                       Id=1,
                       ParentId=1
                  }
              });
            mockProvinceManager.Setup(m => m.GetAll()).Returns(() =>
              new List<CoreSysProvince>() 
              { 
                  new CoreSysProvince()
                  {
                       Id=1,
                       ParentId=0
                  }
              });


            //Act
            var actual = hotelController.GetAreasJson() as JsonResult;
            var actualTreeList = actual.Data as List<TreeNode>;

            //Assert            
            Assert.IsNotNull(actualTreeList);
            Assert.AreEqual(3, actualTreeList.Count);
        }


        [TestMethod]
        public void GetBrandsJson_ShouldReturnBrandsJson()
        {
            //Arrange
            mockBrandManager.Setup(m => m.GetAll()).Returns(() =>
              new List<CoreSysBrand>() 
              { 
                  new CoreSysBrand()
                  {
                       Id="TestBrandId1",
                       GroupId="TestGroupId1"
                  }
              });
            mockGroupManager.Setup(m => m.GetAll()).Returns(() =>
              new List<CoreSysGroup>() 
              { 
                  new CoreSysGroup()
                  {
                       Id="TestGroupId1"
                  }
              });

            //Act
            var actual = hotelController.GetBrandsJson() as JsonResult;
            var actualTreeList = actual.Data as List<TreeNode>;

            //Assert            
            Assert.IsNotNull(actualTreeList);
            Assert.AreEqual(2, actualTreeList.Count);
        }

        #region MockList
        private List<CoreSysHotel> GetMockCoreSysHotels(int count)
        {
            var hotels = new List<CoreSysHotel>();

            for (int i = 0; i < count; i++)
            {
                hotels.Add(new CoreSysHotel
                {
                    Id = "TestHotelId" + i,
                    HotelName = "TestHotelName" + i,
                    BrandId = "TestBrandId" + i,
                    GroupId = "TestGroupId" + i,
                    IsDelete = false,
                    CoreSysHotelSencond = new CoreSysHotelSencond()
                    {
                        Id = "TestHotelId" + i,
                        WelcomeWord = "TestWelcomeWord" + i
                    }
                });
            }

            return hotels;
        }


        private List<CoreSysGroup> GetMockCoreSysGroups(int count)
        {
            var groups = new List<CoreSysGroup>();

            for (int i = 0; i < count; i++)
            {
                groups.Add(new CoreSysGroup
                {
                    Id = "TestGroupId" + i,
                    GroupName = "TestGroupName" + i
                });
            }

            return groups;
        }
        private List<CoreSysBrand> GetMockCoreSysBrands(int count)
        {
            var brands = new List<CoreSysBrand>();

            for (int i = 0; i < count; i++)
            {
                brands.Add(new CoreSysBrand
                {
                    Id = "TestBrandId" + i,
                    BrandName = "TestBrandName" + i,
                    GroupId = "TestGroupId" + i
                });
            }

            return brands;
        }
        #endregion
    }
}
