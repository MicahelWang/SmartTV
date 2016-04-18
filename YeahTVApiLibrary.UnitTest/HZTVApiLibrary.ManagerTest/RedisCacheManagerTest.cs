using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;
using System.IO;
using YeahTVApi.DomainModel.Models.DataModel;

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.ManagerTest
{
    [TestClass]
    public class RedisCacheManagerTest
    {
        private Mock<IRedisCacheService> mockRedisCacheService;
        private IRedisCacheManager redisCacheManager;

        [TestInitialize]
        public void Setup()
        {
            mockRedisCacheService = new Mock<IRedisCacheService>();
            redisCacheManager = new RedisCacheManager(mockRedisCacheService.Object);
        }

        [TestMethod]
        public void GetCache_ShouldReturnValue_WhenGiveValidKey()
        {
            //Arrange

            //Act

            //Assert
        }

    }
}
