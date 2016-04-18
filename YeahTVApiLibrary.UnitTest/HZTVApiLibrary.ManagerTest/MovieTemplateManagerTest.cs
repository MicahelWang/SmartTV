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


namespace YeahTVApiLibrary.UnitTests.HZTVApiLibrary.ManagerTest
{
    [TestClass]
    public class MovieTemplateManagerTest
    {
        private Mock<IMovieTemplateRepertory> mockMovieTemplateRepertory;
        private Mock<IHotelMovieTraceRepertory> mockHotelMovieTraceRepertory;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private IMovieTemplateManager movieTemplateManager;
        private List<MovieTemplate> mockTemplate;
        [TestInitialize]
        public void Setup()
        {
            mockTemplate = GetMockMovieTemplate(10);
            mockMovieTemplateRepertory = new Mock<IMovieTemplateRepertory>();
            mockHotelMovieTraceRepertory = new Mock<IHotelMovieTraceRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            movieTemplateManager = new MovieTemplateManager(mockMovieTemplateRepertory.Object, mockHotelMovieTraceRepertory.Object, mockRedisCacheService.Object);

        }
        [TestMethod]
        public void SearchMovieTemplates_ShouldReturnMovieTemplates_WhenGiveValidCriteria()
        {
            //Arrange
            var mockmovieCriteria = new MovieCriteria();
            mockmovieCriteria.MovieTemplateId = "TestMovieTemplateId";
                mockmovieCriteria.TemplateTitle = "TestTemplateTitle";
                var excepted = mockTemplate.Where(m => m.Id.Contains(mockmovieCriteria.MovieTemplateId)
                    && m.Title.Contains(mockmovieCriteria.TemplateTitle)).ToList();

                mockMovieTemplateRepertory.Setup(m => m.Search(mockmovieCriteria)).Returns(excepted);
            
            //Act
                var actual = movieTemplateManager.SearchMovieTemplates(mockmovieCriteria);
              // Assert
                Assert.IsNotNull(actual);
                Assert.AreEqual(excepted.Count, actual.Count);
        }
        private List<MovieTemplate> GetMockMovieTemplate(int count)
        {
            var Template = new List<MovieTemplate>();

            for (int i = 0; i < count; i++)
            {
                Template.Add(new MovieTemplate
                {
                    Id = "TestId",
                    Title = "TestTitle",
                    Tags = "TestTags",
                });
            }

            return Template;
        }
    }
}
