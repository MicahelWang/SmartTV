using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahCentre.Manager;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahCenter.Infrastructure;

namespace YeahCentre.UnitTest.ManagerTest
{
    [TestClass]
    public class CoreSysHotelManagerTest
    {
        private Mock<ISysHotelSencondRepertory> mockSysHotelSencondRepertory;
        private Mock<ISysBrandRepertory> mockSysBrandRepertory;
        private Mock<ISysGroupRepertory> mockSysGroupRepertory;
        private Mock<ISysHotelRepertory> mockSysHotelRepertory;

        private Mock<IRedisCacheService> mockRedisCacheService;
        private Mock<IHotelMovieTraceNoTemplateWrapperFacade> mockHotelMovieTraceNoTemplateWrapperFacade;
        private IHotelManager hotelManager;
        private List<CoreSysHotel> mockCoreSysHotels;

        [TestInitialize]
        public void Setup()
        {
            mockCoreSysHotels = GetMockCoreSysHotels(10);
            mockSysHotelSencondRepertory = new Mock<ISysHotelSencondRepertory>();
            mockSysGroupRepertory = new Mock<ISysGroupRepertory>();
            mockSysHotelRepertory = new Mock<ISysHotelRepertory>();
            mockSysBrandRepertory = new Mock<ISysBrandRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            mockHotelMovieTraceNoTemplateWrapperFacade = new Mock<IHotelMovieTraceNoTemplateWrapperFacade>();

            //hotelManager = new HotelManager(mockSysHotelRepertory.Object,
            //    mockSysBrandRepertory.Object,
            //    mockSysGroupRepertory.Object,
            //    mockRedisCacheService.Object,
            //    mockSysHotelSencondRepertory.Object, 
            //    mockHotelMovieTraceNoTemplateWrapperFacade.Object);
        }

        [TestMethod]
        public void AddCoreSysHotel_ShouldAddCoreSysHotel_WhenGiveValidCoreSysHotel()
        {
            //Arrange
            var expectedHotelId = "TestHotelId";
            var expectedCoreSysHotel = new CoreSysHotel
            {
                Id = expectedHotelId,
                HotelName = "TestHotelName",
                BrandId = "TestBrandId",
                GroupId = "TestGroupId",
                CoreSysHotelSencond = new CoreSysHotelSencond()
                {
                    Id = expectedHotelId,
                    WelcomeWord = "TestWelcomeWord"
                }
            };

            var actual = new List<CoreSysHotel>();

            mockSysHotelRepertory.Setup(m => m.Insert(expectedCoreSysHotel))
                .Callback(() =>
                {
                    actual.Add(expectedCoreSysHotel);
                });

            //Act
            var actHotelId = hotelManager.Add(expectedCoreSysHotel);

            //Assert
            Assert.AreEqual(expectedHotelId, actHotelId);
            Assert.IsTrue(actual.Any(a =>
                    a.Id.Equals(actHotelId) && a.CoreSysHotelSencond.Id.Equals(actHotelId)));
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddCoreSysHotel_ShouldThorwNullReferenceException_WhenSencondInfoIsNull()
        {
            //Arrange
            var expectedCoreSysHotel = new CoreSysHotel()
            {
                Id = "TestHotelId",
                HotelName = "TestHotelName",
                CoreSysHotelSencond = null
            };

            //Act
            hotelManager.Add(expectedCoreSysHotel);
        }

        [TestMethod]
        public void UpdateCoreSysHotel_ShouldReturnSuccess_WhenCoreSysHotelIsNotNull()
        {
            //Arrange
            var expectedHotelId = "TestHotelId2";
            var expectedCoreSysHotel = new CoreSysHotel
            {
                Id = expectedHotelId,
                HotelName = "TestHotelName modify",
                BrandId = "TestBrandId",
                GroupId = "TestGroupId",
                CoreSysHotelSencond = new CoreSysHotelSencond()
                {
                    Id = expectedHotelId,
                    WelcomeWord = "TestWelcomeWord modify"
                }
            };

            mockSysHotelRepertory.Setup(m => m.GetAll()).Returns(mockCoreSysHotels);
            mockSysHotelRepertory.Setup(m => m.Update(expectedCoreSysHotel)).Callback(() =>
            {
                mockCoreSysHotels.SingleOrDefault(m => m.Id == expectedCoreSysHotel.Id).HotelName =
                    expectedCoreSysHotel.HotelName;
            });

            mockSysHotelSencondRepertory.Setup(m => m.Update(expectedCoreSysHotel.CoreSysHotelSencond))
                .Callback(() =>
                {
                    mockCoreSysHotels.SingleOrDefault(m =>
                        m.CoreSysHotelSencond.Id == expectedCoreSysHotel.CoreSysHotelSencond.Id).
                        CoreSysHotelSencond.WelcomeWord = expectedCoreSysHotel.CoreSysHotelSencond.WelcomeWord;
                });

            mockRedisCacheService.Setup(m => m.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey)).Returns(mockCoreSysHotels);

            //Act
            hotelManager.Update(expectedCoreSysHotel);

            //Assert
            Assert.IsTrue(mockCoreSysHotels.Any(m =>
                m.Id == expectedCoreSysHotel.Id && m.HotelName == expectedCoreSysHotel.HotelName
                && m.CoreSysHotelSencond.WelcomeWord == m.CoreSysHotelSencond.WelcomeWord));
        }
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateCoreSysHotel_ShouldThorwNullReferenceException_WhenGiveNotExistCoreSysHotel()
        {
            //Arrange
            var expectedHotelId = "TestHotelId20";
            var expectedCoreSysHotel = new CoreSysHotel
            {
                Id = expectedHotelId,
                HotelName = "TestHotelName modify",
                BrandId = "TestBrandId",
                GroupId = "TestGroupId",
                CoreSysHotelSencond = null
            };

            mockSysHotelRepertory.Setup(m => m.GetAll()).Returns(mockCoreSysHotels);
            mockRedisCacheService.Setup(m => m.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey)).Returns(mockCoreSysHotels);

            //Act
            hotelManager.Update(expectedCoreSysHotel);
        }

        [TestMethod]
        public void BatchDeleteHotel_ShouldReturnSuccess_WhenGiveValidHotelIds()
        {
            //Arrange
            var expectedDeletedHotels = new string[] { "TestHotelId2", "TestHotelId3" };

            mockSysHotelRepertory.Setup(m => m.GetAll()).Returns(mockCoreSysHotels);
            mockRedisCacheService.Setup(m => m.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey)).Returns(mockCoreSysHotels);
            mockSysHotelRepertory.Setup(m => m.Update(It.IsAny<CoreSysHotel>())).Callback(() =>
            {
                mockCoreSysHotels.ForEach(m =>
                {
                    if (expectedDeletedHotels.Contains(m.Id))
                    {
                        m.IsDelete = true;
                    }
                });
            });

            //Act
            hotelManager.BatchDelete(expectedDeletedHotels);

            //Assert
            Assert.IsTrue(mockCoreSysHotels.Where(m => expectedDeletedHotels.Contains(m.Id)).All(m => m.IsDelete));
        }

        /// <summary>
        /// 测试事务 引发异常后 断言是否生效
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void BatchDeleteHotel_ShouldRollBack_WhenAnyExceptionHappend()
        {
            //Arrange
            var expectedDeletedHotels = new string[] { "TestHotelId2", "TestHotelId3" };
            var updateRunCount = 0;
            mockSysHotelRepertory.Setup(m => m.GetAll()).Returns(mockCoreSysHotels);
            mockRedisCacheService.Setup(m => m.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey)).Returns(mockCoreSysHotels);
            mockSysHotelRepertory.Setup(m => m.Update(It.IsAny<CoreSysHotel>())).
                Callback<CoreSysHotel>(m =>
                {
                    m.IsDelete = true;
                    updateRunCount++;
                });
            mockSysHotelRepertory.Setup(m => m.Update(It.Is<CoreSysHotel>(c => updateRunCount > 0))).Throws(new Exception("any exception happend"));

            //Act
            hotelManager.BatchDelete(expectedDeletedHotels);

            //Assert
            Assert.IsTrue(mockCoreSysHotels.All(m => !m.IsDelete));
        }

        [TestMethod]
        public void GetHotelListByGroupId_ShouldReturnHotels_WhenGiveValidGroupId()
        {
            //Arrange
            var expectedGroupId = "TestGroupId2";
            var expectedHotels = mockCoreSysHotels.Where(m => m.GroupId == expectedGroupId).ToList();

            mockSysHotelRepertory.Setup(m => m.GetAll()).Returns(mockCoreSysHotels);
            mockRedisCacheService.Setup(m => m.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey)).Returns(mockCoreSysHotels);

            //Act
            var actual = hotelManager.GetByGroup(expectedHotels.First().GroupId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.All(m => m.GroupId == expectedGroupId));
            expectedHotels.ForEach(m =>
            {
                Assert.IsTrue(actual.Contains(m));
            });
            Assert.AreEqual(expectedHotels.Count, actual.Count);
        }

        [TestMethod]
        public void GetAllCoreSysHotels_ShouldReturnAllHotels()
        {
            //Arrange
            var expectedHotels = mockCoreSysHotels;

            mockSysHotelRepertory.Setup(m => m.GetAll()).Returns(mockCoreSysHotels);
            mockRedisCacheService.Setup(m => m.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey)).Returns(mockCoreSysHotels);

            //Act
            var actual = hotelManager.GetAllCoreSysHotels();

            //Assert
            Assert.IsNotNull(actual);
            expectedHotels.ForEach(m =>
            {
                Assert.IsTrue(actual.Contains(m));
            });
            Assert.AreEqual(expectedHotels.Count, actual.Count);
        }

        [TestMethod]
        public void GetCoreSysHotelById_ShouldReturnHotel_WhenGiveExistHotelId()
        {
            //Arrange
            var expectedHotelId = "TestHotelId2";
            var expectedHotel = mockCoreSysHotels.Where(m => m.Id == expectedHotelId).FirstOrDefault();

            mockSysHotelRepertory.Setup(m => m.GetAll()).Returns(mockCoreSysHotels);
            mockRedisCacheService.Setup(m => m.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey)).Returns(mockCoreSysHotels);

            //Act
            var actual = hotelManager.GetCoreSysHotelById(expectedHotelId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreSame(expectedHotel, actual);
        }

        [TestMethod]
        public void GetCoreSysHotelById_ShouldReturnNull_WhenGiveNotExistHotelId()
        {
            //Arrange
            var expectedHotelId = "TestHotelId20";

            mockSysHotelRepertory.Setup(m => m.GetAll()).Returns(mockCoreSysHotels);
            mockRedisCacheService.Setup(m => m.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey)).Returns(mockCoreSysHotels);

            //Act
            var actual = hotelManager.GetCoreSysHotelById(expectedHotelId);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PagedList_ShouldReturnHotels_WhenGiveValidPageInfo()
        {
            //Arrange
            var expected = mockCoreSysHotels.Count / 2;

            mockSysHotelRepertory.Setup(m => m.GetAll()).Returns(mockCoreSysHotels);
            mockRedisCacheService.Setup(m => m.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey)).Returns(mockCoreSysHotels);

            //Act
            var actual = hotelManager.PagedList(0, expected, "");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.Count());
        }

        [TestMethod]
        public void SearchCoreSysHotels_ShouldReturnHotels_WhenGiveValidCriteria()
        {
            //Arrange
            var expectedHotelId = "TestHotelId3";
            var expectedHotels = mockCoreSysHotels.Where(m => m.Id == expectedHotelId).ToList();

            var mockCoreSysHotelCriteria = new CoreSysHotelCriteria()
            {
                Id = expectedHotelId
            };
            mockSysHotelRepertory.Setup(m => m.Search(mockCoreSysHotelCriteria)).Returns<BaseSearchCriteria>((s) =>
                mockCoreSysHotels.Where(m => m.Id == s.Id).ToList());

            //Act
            var actual = hotelManager.Search(mockCoreSysHotelCriteria);

            //Assert
            Assert.IsNotNull(actual);
            expectedHotels.ForEach(m =>
            {
                Assert.IsTrue(actual.Contains(m));
            });
            Assert.AreEqual(expectedHotels.Count, actual.Count);
        }

        [TestMethod]
        public void SearchCoreSysHotels_ShouldReturnEmpty_WhenGiveNotExistCriteria()
        {
            //Arrange
            var expectedHotelId = "TestHotelId30";

            var mockCoreSysHotelCriteria = new CoreSysHotelCriteria()
            {
                Id = expectedHotelId
            };
            mockSysHotelRepertory.Setup(m => m.Search(mockCoreSysHotelCriteria)).Returns<BaseSearchCriteria>((s) =>
                mockCoreSysHotels.Where(m => m.Id == s.Id).ToList());

            //Act
            var actual = hotelManager.Search(mockCoreSysHotelCriteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GetSameBrandHotelCount_ShouldReturnCount_WhenGiveExistBrandId()
        {
            //Arrange
            var exceptedBrandId = "TestBrandId1";
            var exceptedCount = mockCoreSysHotels.Where(m => m.BrandId == exceptedBrandId).Count();

            mockSysHotelRepertory.Setup(m => m.GetSameBrandHotelCount(It.IsAny<string>())).Returns<string>(s =>
            {
                return mockCoreSysHotels.Where(m => m.BrandId == s).Count();
            });

            //Act
            var actual = hotelManager.GetSameBrandHotelCount(exceptedBrandId);

            //Assert
            Assert.AreEqual(exceptedCount, actual);
        }

        [TestMethod]
        public void GetSameBrandHotelCount_ShouldReturnZero_WhenGiveNotExistBrandId()
        {
            //Arrange
            var exceptedBrandId = "TestBrandId1111";

            mockSysHotelRepertory.Setup(m => m.GetSameBrandHotelCount(It.IsAny<string>())).Returns<string>(s =>
            {
                return mockCoreSysHotels.Where(m => m.BrandId == s).Count();
            });

            //Act
            var actual = hotelManager.GetSameBrandHotelCount(exceptedBrandId);

            //Assert
            Assert.AreEqual(0, actual);
        }

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
    }
}
