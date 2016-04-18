using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.ManagerTest
{
    [TestClass]
    public class SystemConfigManagerTest
    {
        private Mock<ISystemConfigRepertory> mockSystemConfigRepertory;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private ISystemConfigManager systemConfigManager;
        private List<SystemConfig> mockConfigList;
        [TestInitialize]
        public void Setup()
        {
            mockConfigList = GetMockConfigs(10);
            mockSystemConfigRepertory = new Mock<ISystemConfigRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();

            systemConfigManager = new SystemConfigManager(mockRedisCacheService.Object, mockSystemConfigRepertory.Object);
        }

        [TestMethod]
        public void SearchConfigs_ShouldReturnConfigs_WhenGiveValidCriteria()
        {
            // Arrange
            var criteria = new SystemConfigCriteria();
            criteria.AppType = AppType.AppCenter.ToString();
            criteria.ConfigName = "TestConfigName1";
            criteria.ConfigValue = "TestConfigValue1";
            criteria.Enable = true;

            var mockConfigs = GetMockConfigs(10);
            mockSystemConfigRepertory.Setup(m => m.Search(It.IsAny<SystemConfigCriteria>())).Returns(mockConfigs);

            // Act 
            var actual = systemConfigManager.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Select(a => a.ConfigName).Contains(mockConfigs.Select(m => m.ConfigName).First()));
        }

        [TestMethod]
        public void SearchConfigs_ShouldReturnNull_WhenSystemConfigIsNotExit()
        {
            // Arrange

            mockSystemConfigRepertory.Setup(m => m.Search(It.IsAny<SystemConfigCriteria>())).Returns(new List<SystemConfig>());
            // Act 
            var actual = systemConfigManager.Search(new SystemConfigCriteria());

            // Assert 
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count.Equals(0));
        }

        [TestMethod]
        public void FindConfigsByKey_ShouldReturnConfig_WhenGiveValidConfigId()
        {
            // Arrange
            var exceptedId = 0;

            var mockConfig = GetMockConfigs(1).First();

            mockRedisCacheService.Setup(m => m.IsSet(It.IsAny<string>())).Returns(true);
            mockRedisCacheService.Setup(m => m.Get<List<SystemConfig>>(It.IsAny<string>())).Returns(mockConfigList);

            // Act 
            var actual = systemConfigManager.FindByKey(exceptedId);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(exceptedId, actual.Id);
        }

        [TestMethod]
        public void FindConfigsByKey_ShouldReturnNull_WhenConfigIdNotExit()
        {
            // Arrange
            var exceptedId = 0;
            SystemConfig returnConfig = null;

            mockRedisCacheService.Setup(m => m.IsSet(It.IsAny<string>())).Returns(true);
            mockRedisCacheService.Setup(m => m.Get<List<SystemConfig>>(It.IsAny<string>())).Returns(new List<SystemConfig>());

            // Act 
            var actual = systemConfigManager.FindByKey(exceptedId);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void AddSystemConfig_ShouldReturnSystemConfig_WhenGiveSystemConfig()
        {
            // Arrange
            var mockConfig = new SystemConfig
            {
                ConfigType = AppType.AppCenter.ToString(),
                ConfigName = "TestConfigName",
                ConfigValue = "TestConfigValue",
                Active = true,
                LastUpdateTime = DateTime.Now
            };
            var excepteSystemConfig = mockConfigList.Where(m => m.ConfigName == mockConfig.ConfigName).ToList();
            mockSystemConfigRepertory.Setup(m => m.Search(It.IsAny<SystemConfigCriteria>())).Returns(excepteSystemConfig);
            mockSystemConfigRepertory.Setup(m => m.Insert(mockConfig)).Callback<SystemConfig>((f) =>
            {
                mockConfigList.Add(f);
            });

            // Act 
            systemConfigManager.AddSystemConfig(mockConfig);

            // Assert
            Assert.AreEqual(1, mockConfigList.Count(m => m.ConfigName.Equals(mockConfig.ConfigName)));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddSystemConfig_ShouldReturnException_WhenGiveHaveSystemConfig()
        {
            // Arrange             
            var mockConfig = GetMockConfigs(1).First();
            mockSystemConfigRepertory.Setup(m => m.Search(It.IsAny<SystemConfigCriteria>())).Returns(mockConfigList);
            SystemConfig sysconfig = null;
            mockSystemConfigRepertory.Setup(m => m.Search(It.IsAny<SystemConfigCriteria>())).Returns(mockConfigList);
            // Act 
            systemConfigManager.AddSystemConfig(mockConfig);
            systemConfigManager.AddSystemConfig(sysconfig);

        }

        [TestMethod]
        public void UpdateSystemConfig_ShouldReturnSystemConfig_WhenGiveSystemConfig()
        {
            // Arrange
            var mockConfig = new SystemConfig
            {
                ConfigType = AppType.AppCenter.ToString(),
                ConfigName = "TestConfigName",
                ConfigValue = "TestConfigValue",
                Active = true,
                LastUpdateTime = DateTime.Now
            };

            mockSystemConfigRepertory.Setup(m => m.Update(mockConfig)).Callback<SystemConfig>((f) =>
            {
                mockConfigList.Add(f);
            });
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<SystemConfig>>>())).Returns(new List<SystemConfig>() { mockConfig });
            // Act 
            systemConfigManager.UpdateSystemConfig(mockConfig);

            // Assert
            Assert.IsNotNull(mockConfig);
            Assert.IsTrue(mockConfigList.Where(m => m.ConfigName == mockConfig.ConfigName).FirstOrDefault().Equals(mockConfig));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateSystemConfig_ShouldReturnException_WhenGiveHaveSystemConfig()
        {
            // Arrange             
            var mockConfig = GetMockConfigs(1).First();
            mockSystemConfigRepertory.Setup(m => m.Update(mockConfig)).Throws(new Exception());

            // Act 
            systemConfigManager.UpdateSystemConfig(mockConfig);
        }

        [TestMethod]
        public void FindByKey_ShouldReturnSystemConfig_WhenGiveSystemConfigKey()
        {
            // Arrange
            var mockConfigKey = 1;
            var mockSystemConfig = mockConfigList.FirstOrDefault(m => m.Id == mockConfigKey);

            mockRedisCacheService.Setup(m => m.IsSet(It.IsAny<string>())).Returns(true);
            mockRedisCacheService.Setup(m => m.Get<List<SystemConfig>>(It.IsAny<string>())).Returns(mockConfigList);

            // Act 
            var actual = systemConfigManager.FindByKey(mockConfigKey);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual, mockSystemConfig);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindByKey_ShouldReturnException_WhenGiveSystemConfigKey()
        {
            // Arrange             
            var mockConfigKey = 1;
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<SystemConfig>>>())).Throws(new Exception());

            // Act 
            systemConfigManager.FindByKey(mockConfigKey);
        }

        public void GetAllSysType_ShouldReturnDic_WhenEachSystemConfigTypeEnum()
        {

        }

        public void GetAllSysType_ShouldReturnNull_WhenEachSystemConfigTypeEnum()
        {

        }
        private List<SystemConfig> GetMockConfigs(int count)
        {
            var configs = new List<SystemConfig>();

            for (int i = 0; i < count; i++)
            {
                configs.Add(new SystemConfig
                {
                    ConfigType = AppType.AppCenter.ToString(),
                    ConfigName = "TestConfigName" + i,
                    ConfigValue = "TestConfigValue" + i,
                    Active = true,
                    LastUpdateTime = DateTime.Now,
                    Id = i
                });
            }

            return configs;
        }
    }
}
