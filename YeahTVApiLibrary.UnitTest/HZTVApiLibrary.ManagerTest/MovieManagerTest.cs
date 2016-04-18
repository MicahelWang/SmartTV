using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApiLibrary.Infrastructure;
using Moq;
using YeahTVApiLibrary.Manager;
using YeahTVApi.DomainModel.Models;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.DomainModel.SearchCriteria;
using System.Linq.Expressions;

namespace YeahTVApiLibrary.UnitTests.HZTVApiLibrary.ManagerTest
{
    [TestClass]
    public class MovieManagerTest
    {
        private Mock<IMovieRepertory> mockMovieRepertory;
        private Mock<IMovieTemplateManager> mockMovieTemplateManager;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private Mock<ISysAttachmentManager> mockSysAttachmentManager;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private IMovieManager movieManager;
        private List<Movie> movieList = new List<Movie>();

        [TestInitialize]
        public void Initial()
        {
            InitalList(10);
            mockMovieRepertory = new Mock<IMovieRepertory>();
            mockMovieTemplateManager = new Mock<IMovieTemplateManager>();
            mockSysAttachmentManager = new Mock<ISysAttachmentManager>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            movieManager = new MovieManager(mockMovieRepertory.Object,
                mockMovieTemplateManager.Object,
                mockConstantSystemConfigManager.Object,
                mockSysAttachmentManager.Object,
                mockRedisCacheService.Object);
        }

        [TestMethod]
        public void Update_WhenEntiyIsNotNull()
        {
            //Arrange
            var movie = new Movie()
            {
                CoverAddress = "上海市",
                CoverPath = "田林路487号",
                LastUpdateTime = DateTime.Now,
                MovieReview = "速度与激情7",
                MovieReviewEn = "suduyujiqing7",
                Name = "速度与激情7",
                NameEn = "speed and power",
                PosterAddress = "ghjklsfsdfs",
                Id = "1"
            };
            mockMovieRepertory.Setup(m => m.Update(movie)).Callback<Movie>((movieEf) =>
            {
                movieList.Where(f => f.Id == movieEf.Id).FirstOrDefault().Name = movie.Name;
            });

            mockMovieRepertory.Setup(m => m.Search(It.IsAny<MovieCriteria>())).Returns(new List<Movie> { movie });
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<Movie>>>())).Returns(movieList);
            mockRedisCacheService.Setup(m => m.RemoveItemFromSet(It.IsAny<string>(), It.IsAny<object>()));

            //act
            movieManager.Update(movie);
            //
            Assert.IsTrue(movieList.Any(m => m.Name.Equals(movie.Name)));
        }

        [TestMethod]
        public void Update_WhenException()
        {
            //Arrange
            var movie = new Movie()
            {
                CoverAddress = "上海市",
                CoverPath = "田林路487号",
                LastUpdateTime = DateTime.Now,
                MovieReview = "速度与激情7",
                MovieReviewEn = "suduyujiqing7",
                Name = "速度与激情7",
                NameEn = "speed and power",
                PosterAddress = "ghjklsfsdfs",
                Id = "1"
            };
            mockMovieRepertory.Setup(m => m.Update(movie)).Callback<Movie>((movieEf) =>
            {
                throw new Exception("异常，更新失败");
            });
        }

        [TestMethod]
        public void AddMovieList()
        {
            mockMovieRepertory.Setup(m => m.Insert(movieList)).Callback<IEnumerable<Movie>>((movieEf) =>
            {
                movieList.AddRange(movieEf);
            });
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<Movie>(), It.IsAny<Func<List<Movie>>>()));
            mockMovieRepertory.Setup(m => m.Search(It.IsAny<MovieCriteria>())).Returns(movieList);
            mockMovieRepertory.Setup(m => m.GetAll());

            //act
            movieManager.AddMovies(movieList);

            //Assert
            Assert.AreEqual(2, movieList.Count(f => f.Id == "1"));
        }


        [TestMethod]
        public void AddMovies_whenEfIsNotNull()
        {
            var movie = new Movie()
            {
                CoverAddress = "上海市",
                CoverPath = "田林路487号",
                LastUpdateTime = DateTime.Now,
                MovieReview = "速度与激情7",
                MovieReviewEn = "suduyujiqing7",
                Name = "速度与激情7",
                NameEn = "speed and power",
                PosterAddress = "ghjklsfsdfs",
                Id = "1"
            };

            mockMovieRepertory.Setup(m => m.Insert(movie)).Callback<Movie>((movieEf) =>
            {
                movieList.Add(movieEf);
            });
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<Movie>(), It.IsAny<Func<List<Movie>>>()));
            mockMovieRepertory.Setup(m => m.Search(It.IsAny<MovieCriteria>())).Returns(movieList);
            mockMovieRepertory.Setup(m => m.GetAll());

            //act
            movieManager.AddMovies(movie);
            //Assert
            Assert.IsNotNull(movieList.FirstOrDefault(m => m.Id == movie.Id));
        }


        [TestMethod]
        public void AddMovies_whenException()
        {
            var movie = new Movie()
            {
                CoverAddress = "上海市",
                CoverPath = "田林路487号",
                LastUpdateTime = DateTime.Now,
                MovieReview = "速度与激情7",
                MovieReviewEn = "suduyujiqing7",
                Name = "速度与激情7",
                NameEn = "speed and power",
                PosterAddress = "ghjklsfsdfs",
                Id = "1"
            };

            mockMovieRepertory.Setup(m => m.Insert(movie)).Callback<Movie>((movieEf) =>
            {
                throw new Exception("异常");
            });
        }

        [TestMethod]
        public void Delete()
        {
            //Arrange
            var movieCriteria = new MovieCriteria()
            {
                MovieId = "1"
            };
            var movie = new Movie()
            {
                CoverAddress = "上海市",
                CoverPath = "田林路487号",
                LastUpdateTime = DateTime.Now,
                MovieReview = "速度与激情7",
                MovieReviewEn = "suduyujiqing7",
                Name = "速度与激情7",
                NameEn = "speed and power",
                PosterAddress = "ghjklsfsdfs",
                Id = "1"
            };
            //manager 中 Delete if分支1
            mockMovieTemplateManager.Setup(m => m.GetAllFromCache()).Returns(new List<MovieTemplate>());

            mockMovieRepertory.Setup(m => m.Delete(It.IsAny<Expression<Func<Movie, bool>>>())).
                Callback<Expression<Func<Movie, bool>>>(f => movieList.Where(f.Compile()).ToList().ForEach(item => movieList.Remove(item)));

            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<Movie>>>())).Returns(movieList);
            mockRedisCacheService.Setup(m => m.RemoveItemFromSet(It.IsAny<string>(), It.IsAny<object>()));

            //Act
            movieManager.Delete(movie);

            //Assert
            Assert.IsTrue(!movieList.Where(movieItem => movieItem.Id == movie.Id).Any());

        }


        #region    初始化List
        public void InitalList(int count)
        {

            for (int i = 0; i < count; i++)
            {
                var movie = new Movie()
                {
                    CoverAddress = "上海市" + i,
                    CoverPath = "田林路487号" + i,
                    LastUpdateTime = DateTime.Now,
                    MovieReview = "速度与激情7" + i,
                    MovieReviewEn = "suduyujiqing7" + i,
                    Name = "速度与激情7" + i,
                    NameEn = "speed and power" + i,
                    PosterAddress = "ghjklsfsdfs" + i,
                    Id = i.ToString()
                };
                movieList.Add(movie);
            }
        }
        #endregion

    }
}
