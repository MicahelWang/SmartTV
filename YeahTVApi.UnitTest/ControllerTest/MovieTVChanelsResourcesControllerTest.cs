using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Controllers;
using YeahTVApi.Entity;
using Moq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using System.Collections.Generic;
using YeahTVApi.DomainModel.Mapping;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Enum;
using System.Linq;

namespace YeahTVApi.UnitTests.ControllerTest
{
    [TestClass]
    public class MovieTVChanelsResourcesControllerTest
    {
        private Mock<IHotelTVChannelManager> mockHotelTVChannelManager;
        private Mock<IHotelMovieTraceManager> mockHotelMovieTraceManager;
        private Mock<IRedisCacheManager> mockRedisCacheManager;
        private Mock<IRequestApiService> mockRequestApiService;
        private Mock<IHttpContextService> mockHttpContextService;
        private Mock<ILogManager> mockLogManager;
        private Mock<ITagManager> mockTagManager;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private Mock<IHotelMovieTraceNoTemplateWrapperFacade> mockHotelMovieTraceWrapperFacade;
        private Mock<ITVHotelConfigManager> mocktvHotelConfigManager;

        private MovieTVChanelsResourcesController movieTVChanelsResourcesController;
        [TestInitialize]
        public void Setup()
        {
            mockHotelTVChannelManager = new Mock<IHotelTVChannelManager>();
            mockHotelMovieTraceManager = new Mock<IHotelMovieTraceManager>();
            mockRedisCacheManager = new Mock<IRedisCacheManager>();
            mockRequestApiService = new Mock<IRequestApiService>();
            mockHttpContextService = new Mock<IHttpContextService>();
            mockLogManager = new Mock<ILogManager>();
            mockTagManager = new Mock<ITagManager>();
            mocktvHotelConfigManager = new Mock<ITVHotelConfigManager>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            mockHotelMovieTraceWrapperFacade = new Mock<IHotelMovieTraceNoTemplateWrapperFacade>();

            var mockHeader = new RequestHeader
            {
                HotelID = "TestHotelId"
            };
            mockHttpContextService.Setup(m => m.Current.Items[RequestParameter.Header]).Returns(mockHeader);

            movieTVChanelsResourcesController = new MovieTVChanelsResourcesController(
                mockHotelTVChannelManager.Object,
                mockHotelMovieTraceManager.Object,
                mockRedisCacheManager.Object
                , mockRequestApiService.Object,
                mockHttpContextService.Object,
                mockLogManager.Object,
                mockConstantSystemConfigManager.Object,
                mockHotelMovieTraceWrapperFacade.Object, 
                mockTagManager.Object,
                mocktvHotelConfigManager.Object);
        }

        [TestMethod]
        public void GetHotelChanels_ShouldReturnHotelChanels_WhenGiveExitHotelId()
        {
            // Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;
            var mockTvChannels = GetMockHotelTvChannels(10, mockHeader);

            mockConstantSystemConfigManager.Setup(m => m.AppCenterUrl).Returns("TestConfigValue/");

            var url = mockConstantSystemConfigManager.Object.AppCenterUrl + Constant.GetHotelApiUrl + mockHeader.HotelID;

            var mockHotelEntity = new HotelEntity
           {
               AdUrl = "TestAdUrl"
           };

            var excepted = mockTvChannels.ToHotelTvChannelApiModel(mockHotelEntity.AdUrl);

            mockHotelTVChannelManager.Setup(m => m.SearchHotelTVChannels(mockHeader)).Returns(mockTvChannels);

            mockRequestApiService.Setup(m => m.Get(url))
                .Returns(mockHotelEntity.ToJsonString());

            // Act 
            var actual = movieTVChanelsResourcesController.GetHotelChanels().obj;

            // Assert
            Assert.AreEqual(excepted.ToJsonString(), actual.ToJsonString());
        }

        [TestMethod]
        public void GetHotelChanels_ShouldThrowException_WhenNotGiveSystemConfigKey()
        {
             // Arrange

            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;

            mockHotelTVChannelManager.Setup(m=>m.SearchHotelTVChannels(mockHeader))
                    .Callback(() =>
                    {
                        throw new System.Exception();
                    });

            mockLogManager.Setup(m => m.SaveError(It.IsAny<Exception>(), It.IsAny<object>(), It.IsAny<AppType>(),It.IsAny<string>()));
            
            //Act
            var actual = movieTVChanelsResourcesController.GetHotelChanels().obj as List<HotelTVChannelApiModel>;

            // Assert
            //Assert.IsNull(actual);
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void GetHotelChanels_ShouldThrowApiException_WhenGiveNotExitHotelId()
        {
            // Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;

            mockHotelTVChannelManager.Setup(m => m.SearchHotelTVChannels(mockHeader))
                .Callback(() => { throw new ApiException("TestApiException"); });

            // Act 
            var actual = (movieTVChanelsResourcesController.GetHotelChanels() ).obj as List<HotelTVChannelApiModel>;
            
            // Assert
           
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void GetHotelMovies_ShouldReturnHotelMovies_WhenGiveExitHotelId()
        {
            //Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;
            var mockmovies = GetMockHotelMovies(10, mockHeader);
            var excepted = mockmovies;
            mockHotelMovieTraceManager.Setup(m => m.SearchMoviesForApi(mockHeader)).Returns(mockmovies);
            // Act 
            var actual = movieTVChanelsResourcesController.GetHotelMovies().obj;
            // Assert
            Assert.AreEqual(excepted.ToJsonString(), actual.ToJsonString());
        }

        [TestMethod]
        public void GetHotelMovies_ShouldThrowApiException_WhenGiveNotExitHoteId()
        {
            //Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;
            mockHotelMovieTraceManager.Setup(m => m.SearchMoviesForApi(mockHeader)).Callback(() => { throw new ApiException("TestApiException"); });
            mockLogManager.Setup(m => m.SaveError(It.IsAny<Exception>(), It.IsAny<object>(), It.IsAny<AppType>(), It.IsAny<string>()));

            //Act
            var actual = movieTVChanelsResourcesController.GetHotelMovies().obj as List<MovieApiModel>;

            // Assert
            //Assert.IsNull(actual);
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void GetHotelMovies_ShouldReturnNull_WhenGiveNotExitHoteId()
        {
            //Arrange
            var mockHeader = mockHttpContextService.Object.Current.Items[RequestParameter.Header] as RequestHeader;
            List<MovieApiModel> mockMovieApiModels = null ;
            mockHotelMovieTraceManager.Setup(m => m.SearchMoviesForApi(mockHeader)).Returns(mockMovieApiModels);
            //Act
            var actual = movieTVChanelsResourcesController.GetHotelMovies().obj;
            //Assert
            Assert.AreEqual(null, actual);
        }

        private List<HotelTVChannel> GetMockHotelTvChannels(int count,RequestHeader header)
        {
            var hotelTVChannels = new List<HotelTVChannel>();

            for (int i = 0; i < count; i++)
            {
                hotelTVChannels.Add(new HotelTVChannel
                {
                    HotelId = header.HotelID,
                    Name = "Testname",
                    Category = "TestCategory"

                });
            }
            return hotelTVChannels;
        }

        private List<MovieApiModel> GetMockHotelMovies(int count, RequestHeader header)
        {
            var hotelMovies = new List<MovieApiModel>();

            for (int i = 0; i < count; i++)
            {
                hotelMovies.Add(new MovieApiModel
                {
                    Name = "Testname",
                    MovieReview = "TestCategorMovieReview"

                });
            }
            return hotelMovies;
        }
    }

}

