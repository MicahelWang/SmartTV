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
using YeahTVApi.DomainModel.Models.DataModel;
using System.Linq.Expressions;

namespace YeahTVApiLibrary.UnitTests.HZTVApiLibrary.ManagerTest
{
    [TestClass]
    public class HotelMovieTraceManagerTest
    {
        private Mock<IHotelMovieTraceRepertory> mockHotelMovieTraceRepertory;
        private Mock<IMovieTemplateRelationManager> mockMovieTemplateRelationManager;
        private Mock<IRequestApiService> mockRequestApiService;
        private Mock<ISysAttachmentManager> mockSysAttachmentManager;
        private Mock<ITVHotelConfigManager> mockTVHotelConfigManager;
        private Mock<IMovieTemplateManager> mockMovieTemplateManager;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private IHotelMovieTraceManager HotelMovieTraceManager;
        private List<HotelMovieTrace> mockTraces;

        [TestInitialize]
        public void Setup()
        {
            mockTraces = GetMockDeviceTraces(100);
            mockHotelMovieTraceRepertory = new Mock<IHotelMovieTraceRepertory>();
            mockMovieTemplateRelationManager = new Mock<IMovieTemplateRelationManager>();
            mockSysAttachmentManager = new Mock<ISysAttachmentManager>();
            mockTVHotelConfigManager = new Mock<ITVHotelConfigManager>();
            mockMovieTemplateManager = new Mock<IMovieTemplateManager>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            mockRequestApiService = new Mock<IRequestApiService>();
            HotelMovieTraceManager = new HotelMovieTraceManager(mockHotelMovieTraceRepertory.Object,
                mockMovieTemplateRelationManager.Object,
                mockSysAttachmentManager.Object,
                mockTVHotelConfigManager.Object,
                mockMovieTemplateManager.Object,
                mockConstantSystemConfigManager.Object,
                mockRedisCacheService.Object,
                mockRequestApiService.Object);
        }

        [TestMethod]
        public void SearchTraces_ShouldReturnTraces_WhenGiveValidCriteria()
        {
            //Arrange
            var criteria = new HotelMovieTraceCriteria();
            criteria.HotelId = "TestHotelId1";
            criteria.MovieId = "TestMovieId1";

            var excepted = mockTraces.Where(m => m.HotelId.Contains(criteria.HotelId)
                && m.MovieId.Contains(criteria.MovieId)).ToList();
            mockHotelMovieTraceRepertory.Setup(m => m.Search(criteria)).Returns(excepted);

            //Act
            var actual = HotelMovieTraceManager.Search(criteria);


            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.Count, actual.Count);

        }

        [TestMethod]
        public void SearchTraces_ShouldReturnTwentyTraces_WhenGiveValidCriteria()
        {
            //Arrange
            var criteria = new HotelMovieTraceCriteria();
            criteria.HotelId = "TestHotelId1";
            criteria.MovieId = "TestMovieId1";

            var excepted = mockTraces.Where(m => m.HotelId.Contains(criteria.HotelId)
            && m.MovieId.Contains(criteria.MovieId)).Page(criteria.PageSize, criteria.Page).ToList();
            mockHotelMovieTraceRepertory.Setup(m => m.Search(criteria)).Returns(excepted);

            //Act         
            var actual = HotelMovieTraceManager.Search(criteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void SearchTraces_ShouldReturnNull_WhenTraceNotExit()
        {
            //Arrange
            var criteria = new HotelMovieTraceCriteria();
            criteria.HotelId = "TestHotelId1";
            criteria.MovieId = "MovieId1";
            mockHotelMovieTraceRepertory.Setup(m => m.Search(criteria)).Returns(new List<HotelMovieTrace>());

            //Act
            var actual = HotelMovieTraceManager.Search(criteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void SearchTraces_ShouldFail_WhenRepertoryHasException()
        {
            //Arrange
            var criteria = new HotelMovieTraceCriteria();
            criteria.HotelId = "TestHotelId1";
            criteria.MovieId = "TestMovieId1";
            mockHotelMovieTraceRepertory.Setup(m => m.Search(criteria)).Callback(() =>
            {
                throw new CommonFrameworkManagerException("Serach error!", null);
            });

            //Act
            HotelMovieTraceManager.Search(criteria);

        }

        [TestMethod]
        public void AddMovieTraceManager_ShouldSuccess_WhenAddMovieTraceManager()
        {
            //Arrange
            var mockHotelCount = 0.0;
            var mockMovieId = "TestMovieId";

            var mockTrace = new HotelMovieTrace
            {
                MoiveTemplateId = "TestMoiveTemplateId",
                HotelId = "TestHotelIdInsert",
                MovieId = "TestMovieIdInsert",
            };

            var mockMovieTemplate = new MovieTemplate
            {
                HotelCount = mockHotelCount,
                Id = "TestMoiveTemplateId"
            };

            var mockMovieInTemplates = new List<MovieTemplateRelation>
            {
                new MovieTemplateRelation
                {
                    MovieId = mockMovieId,
                    MovieTemplateId= "TestMoiveTemplateId"
                }
            };

            mockRedisCacheService.Setup(m => m.Remove(It.IsAny<string>()));
            mockMovieTemplateManager.Setup(m => m.GetAllFromCache()).Returns(new List<MovieTemplate> { mockMovieTemplate });
            mockMovieTemplateManager.Setup(m => m.Update(It.IsAny<MovieTemplate>()));
            mockMovieTemplateRelationManager.Setup(m => m.GetAllFromCache()).Returns(mockMovieInTemplates);

            mockHotelMovieTraceRepertory.Setup(m => m.Insert(It.Is<List<HotelMovieTrace>>(i => i.Any(a => a.MovieId.Equals(mockMovieId))))).Callback(() =>
            {
                mockTraces.Add(mockTrace);
            });

            //Act
            HotelMovieTraceManager.AddMovieTraceManager(mockTrace);

            //Assert
            Assert.IsTrue(mockTraces.Any(m => m.MovieId == mockTrace.MovieId));
            Assert.AreEqual(++mockHotelCount, mockMovieTemplate.HotelCount);
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void AddMovieTraceManager_ShouldFail_WhenRepertoryHasException()
        {
            //Arrange
            var mockTrace = new HotelMovieTrace
            {
                HotelId = "TestHotelIdInsert",
                MovieId = "TestMovieIdInsert",
            };
            mockHotelMovieTraceRepertory.Setup(m => m.Insert(mockTrace)).Callback(() =>
            {
                throw new CommonFrameworkManagerException("AddMovieTraceManager error!", null);
            });
            //Act
            HotelMovieTraceManager.AddMovieTraceManager(mockTrace);
        }

        [TestMethod]
        public void UpdateMovieTraceManager_ShouldSuccess_WhenUpdateMovieTraceManager()
        {
            //Arrange
            var mockMovieId = "TestMovieId";
            var mockTrace = new HotelMovieTrace
            {
                HotelId = "TestHotelIdInsert",
                MovieId = "TestMovieIdInsert",
                MoiveTemplateId = "TestMoiveTemplateId"
            };
            var mockMovieTemplate = new MovieTemplate
            {
                Id = "TestMoiveTemplateId"
            };

            mockHotelMovieTraceRepertory.Setup(m => m.Search(It.IsAny<HotelMovieTraceCriteria>())).Returns(new List<HotelMovieTrace> { mockTrace });
            mockHotelMovieTraceRepertory.Setup(m => m.Update(mockTrace)).Callback(() =>
            {
                mockTraces.SingleOrDefault(m => m.MovieId == "TestMovieId").VideoServerAddress = mockTrace.VideoServerAddress;
            });

            mockHotelMovieTraceRepertory.Setup(m => m.FindByKey(It.IsAny<string>())).Returns(mockTraces.FirstOrDefault());
            mockHotelMovieTraceRepertory.Setup(m => m.Delete(h => h.HotelId.Equals(mockTrace.HotelId)));

            var mockMovieInTemplates = new List<MovieTemplateRelation>
            {
                new MovieTemplateRelation
                {
                    MovieId = mockMovieId
                }
            };

            mockMovieTemplateManager.Setup(m => m.GetAllFromCache()).Returns(new List<MovieTemplate> { mockMovieTemplate });
            mockMovieTemplateRelationManager.Setup(m => m.GetAllFromCache()).Returns(mockMovieInTemplates);

            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<HotelMovieTrace>>>())).Returns(new List<HotelMovieTrace> { mockTrace });
            mockRedisCacheService.Setup(m => m.RemoveItemFromSet(It.IsAny<string>(), It.IsAny<object>()));

            //Act
            HotelMovieTraceManager.UpdateMovieTraceManager(mockTrace);

            //Assert
            Assert.IsTrue(mockTraces.Any(m => m.VideoServerAddress == mockTrace.VideoServerAddress));
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void UpdateMovieTraceManager_ShouldFail_WhenRepertoryHasException()
        {
            //Arrange
            var mockTrace = new HotelMovieTrace
            {
                HotelId = "TestHotelIdInsert",
                MovieId = "TestMovieIdInsert",
            };
            mockHotelMovieTraceRepertory.Setup(m => m.FindByKey(It.IsAny<string>())).Callback(() =>
            {
                throw new CommonFrameworkManagerException("UpdateMovieTraceManager Error!", null);
            });
            //Act
            HotelMovieTraceManager.UpdateMovieTraceManager(mockTrace);
        }

        [TestMethod]
        public void SearchMoviesForApi_ShouldReturnSearchMoviesForApi_WhenGiveExitRequestHeader()
        {
            //Arrange
            var mockHeader = new RequestHeader
            {
                HotelID = "TestHotelId"
            };

            var mockDeviceTraces = GetMockDeviceTraces(10);
            mockConstantSystemConfigManager.Setup(m => m.ResourceSiteAddress).Returns("TestResourceSiteAddress/");

            mockTVHotelConfigManager.Setup(m => m.SearchFromCache(new HotelConfigCriteria() { }))
                .Returns(new List<TVHotelConfig>
                {
                    new TVHotelConfig
                    { 
                        ConfigName = "TestConfigName",
                        ConfigValue = "TestConfigValue",
                        ConfigCode="VodAddress",
                        HotelId="TestHotelId"
                    } 
                });

            var excepted = mockDeviceTraces.Select(h => new MovieApiModel
            {
                CoverAddress = h.Movie.CoverAddress,
                IsFree = h.MovieTemplate.MovieTemplateRelations.FirstOrDefault().IsFree,
                LastViewTime = h.LastViewTime,
                MovieReview = h.Movie.MovieReview,
                MovieReviewEn = h.Movie.MovieReviewEn,
                Name = h.Movie.Name,
                NameEn = h.Movie.NameEn,
                PosterAddress = new List<string> { h.Movie.PosterAddress },
                Price = h.MovieTemplate.MovieTemplateRelations.FirstOrDefault().Price,
                ViewCount = h.ViewCount,
                MovieId = h.MovieId
            }).ToList();

            mockHotelMovieTraceRepertory.Setup(m => m.Search(It.IsAny<HotelMovieTraceCriteria>()))
                .Returns(mockDeviceTraces);

            mockSysAttachmentManager.Setup(m => m.GetById(It.IsAny<int>())).Returns(new CoreSysAttachment { FilePath = "TestFilePath" });


            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<HotelMovieTrace>>>())).Returns(mockDeviceTraces);
            mockRedisCacheService.Setup(m => m.RemoveItemFromSet(It.IsAny<string>(), It.IsAny<object>()));

            //Act
            var actual = HotelMovieTraceManager.SearchMoviesForApi(mockHeader);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.Count, actual.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(CommonFrameworkManagerException))]
        public void SearchMoviesForApi_ShouldFail_WhenRepertoryHasException()
        {
            //Arrange
            var mockHeader = new RequestHeader
            {
                HotelID = "TestHotelId"
            };
            mockHotelMovieTraceRepertory.Setup(m => m.Search(It.IsAny<HotelMovieTraceCriteria>())).Callback(() =>
            {
                throw new CommonFrameworkManagerException("SearchMoviesForApi error!", null);
            });
            //Act
            HotelMovieTraceManager.SearchMoviesForApi(mockHeader);
        }

        private List<HotelMovieTrace> GetMockDeviceTraces(int count)
        {
            var traces = new List<HotelMovieTrace>();

            for (int i = 0; i < count; i++)
            {
                traces.Add(new HotelMovieTrace
                {

                    HotelId = "TestHotelId",
                    MovieId = "TestMovieId" + i,
                    Active = true,
                    IsDownload = true,
                    MovieTemplate = new MovieTemplate
                    {
                        MovieTemplateRelations = new List<MovieTemplateRelation> { new MovieTemplateRelation { MovieId = "TestMovieId" + i, IsFree = true, Price = 0 } },
                    },

                    Movie = new Movie
                    {
                        Id = "TestMovieId" + i,
                        CoverAddress = "0",
                        PosterAddress = "0",
                    },
                    ViewCount = 0,
                });
            }

            return traces;
        }
    }
}
