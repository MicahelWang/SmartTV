using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YeahTVApi.Controllers;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApi.UnitTests.ControllerTest
{
    [TestClass]
    public class CacheControllerTest
    {
        private Mock<ICacheManager> mockCacheManager;
        private Mock<ILogManager> mockLogManager;
        private CacheController cacheController;

        [TestInitialize]
        public void Setup()
        {
            mockCacheManager = new Mock<ICacheManager>();
            mockLogManager = new Mock<ILogManager>();

            cacheController = new CacheController(mockCacheManager.Object, mockLogManager.Object);
        }

        [TestMethod]
        public void SetCache_ShouldSuccessful_WhenSetCacheSuccessful()
        {
            // Arrange
            var exceptedMessage = "设置缓存成功";

            mockCacheManager.Setup(m => m.SetAppsList()).Callback(() => { });
            mockCacheManager.Setup(m => m.SetWeather()).Callback(() => { });
            mockLogManager.Setup(m => m.SaveInfo(exceptedMessage, exceptedMessage, AppType.TV, It.IsAny<string>()));

            // Act 
            var actual = cacheController.SetCache().Data.ToString();

            // Assert
            Assert.AreEqual(exceptedMessage, actual);
        }
        [TestMethod]
        public void SetCache_ShouldError_WhenSetCacheError()
        {
            // Arrange
            var exceptedMessage = "设置缓存失败";

            mockCacheManager.Setup(m => m.SetAppsList()).Callback(() => { throw new Exception(); });
            mockCacheManager.Setup(m => m.SetWeather()).Callback(() => { });
            mockLogManager.Setup(m => m.SaveError(exceptedMessage, exceptedMessage, AppType.TV, It.IsAny<string>()));
            // Act 
            var actual = cacheController.SetCache().Data.ToString();
            // Assert
            Assert.AreEqual(exceptedMessage, actual);
        }
    }
}
