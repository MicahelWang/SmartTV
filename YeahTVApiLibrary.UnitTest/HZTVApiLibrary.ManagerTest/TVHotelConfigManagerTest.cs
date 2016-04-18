using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;
using YeahTVApi.DomainModel.SearchCriteria;
using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using System.Linq;
using YeahTVApi.DomainModel;

namespace YeahTVApiLibrary.UnitTests.HZTVApiLibrary.ManagerTest
{
    [TestClass]
    public class TVHotelConfigTest
    {
        private Mock<ITVHotelConfigRepertory> mockTVHotelConfigRepertory;
        private ITVHotelConfigManager tVHotelConfigManager;
        private List<TVHotelConfig> tvHotelConfigs;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        [TestInitialize]
        public void Setup()
        {
            tvHotelConfigs = GetMockTVHotelConfig(10);
            mockTVHotelConfigRepertory = new Mock<ITVHotelConfigRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            tVHotelConfigManager = new TVHotelConfigManager(mockTVHotelConfigRepertory.Object,
                mockRedisCacheService.Object, mockConstantSystemConfigManager.Object);
        }


        [TestMethod]
        public void GetHotelConfig_ShouldReturnTVHotelConfig_WhenGiveValidCriteria()
        {
            //Arrange
            var criteria = new HotelConfigCriteria()
            {
                HotelId = "TestHotelId1"
            };
            var exceptedHotelConfig = tvHotelConfigs.Where(h => h.HotelId == criteria.HotelId).FirstOrDefault();

            mockTVHotelConfigRepertory.Setup(m => m.Search(It.IsAny<HotelConfigCriteria>())).Returns<HotelConfigCriteria>(c => tvHotelConfigs.Where(m => m.HotelId.Equals(c.HotelId)).ToList());

            //Act
            var actual = tVHotelConfigManager.GetHotelConfig(criteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(exceptedHotelConfig, actual);
        }

        [TestMethod]
        public void GetHotelConfig_ShouldReturnNull_WhenGiveNotExistHotelId()
        {
            //Arrange
            var criteria = new HotelConfigCriteria()
            {
                HotelId = "TestHotelId222222"
            };

            mockTVHotelConfigRepertory.Setup(m => m.Search(It.IsAny<HotelConfigCriteria>())).Returns<HotelConfigCriteria>(c => tvHotelConfigs.Where(m => m.HotelId.Equals(c.HotelId)).ToList());

            //Act
            var actual = tVHotelConfigManager.GetHotelConfig(criteria);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void SearchHotelConfig_ShouldReturnHotelConfig_WhenGiveValidCriteria()
        {
            //Arrange
            var criteria = new HotelConfigCriteria();
            criteria.HotelId = "TestHotelId";
            criteria.ConfigCodes = "TestConfigCode";
            criteria.NeedPaging = true;

            var exceptedHotelConfig = tvHotelConfigs.Where(t => t.HotelId.Equals(criteria.HotelId)
                && t.ConfigCode.Equals(criteria.ConfigCodes)).ToList();

            mockTVHotelConfigRepertory.Setup(m => m.Search(criteria)).Returns(exceptedHotelConfig);

            //Act
            var actual = tVHotelConfigManager.Search(criteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(exceptedHotelConfig.Count, actual.Count);
        }

        [TestMethod]
        public void SearchHotelConfig_ShouldReturnNull_WhenGiveValidCriteria()
        {
            //Arrange
            var criteria = new HotelConfigCriteria();
            criteria.HotelId = "TestHotelId1";
            criteria.ConfigCodes = "TestConfigCode1";
            var exceptedtvhotelconfig = new List<TVHotelConfig>();

            mockTVHotelConfigRepertory.Setup(m => m.Search(criteria)).Returns(exceptedtvhotelconfig);

            //Act
            var actual = tVHotelConfigManager.Search(criteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void AddHotelConfig_shouldSucess_whenAddHotelConfig()
        {
            //Arrange
            var criteria = new HotelConfigCriteria();
            criteria.HotelId = "TestHotelId2";
            criteria.ConfigCodes = "TestConfigCode22";
            var exceptedHotelConfig = tvHotelConfigs.Where(t => t.HotelId.Equals(criteria.HotelId)
            && t.ConfigCode.Equals(criteria.ConfigCodes)).ToList();
            mockTVHotelConfigRepertory.Setup(m => m.Search(It.IsAny<BaseSearchCriteria>())).Returns(exceptedHotelConfig);

            var mockhotelConfig = new TVHotelConfig
            {
                HotelId = "TesthotelidInsert",
                ConfigCode = "TestconfigcodeInsert",
                ConfigName = "Testconfignameinsert",
                Active = true,
                LastUpdater = "admin",
                LastUpdateTime = DateTime.Now
            };
            mockRedisCacheService.Setup(m => m.GetAllFromCache(RedisKey.TvHotelConfigKey, It.IsAny<Func<List<TVHotelConfig>>>())).Returns(tvHotelConfigs);

            mockTVHotelConfigRepertory.Setup(m => m.Insert(mockhotelConfig)).Callback(() =>
                {
                    tvHotelConfigs.Add(mockhotelConfig);
                });


            //Act
            tVHotelConfigManager.AddTVHotelConfig(mockhotelConfig);
            //Assert
            Assert.IsTrue(tvHotelConfigs.Any(m => m.ConfigCode.Equals(mockhotelConfig.ConfigCode)));
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddHotelConfig_shouldFail_whenAddHotelConfig()
        {
            //Arrange
            var criteria = new HotelConfigCriteria();
            criteria.HotelId = "TestHotelId1";
            criteria.ConfigCodes = "TestConfigCode1";
            var exceptedHotelConfig = tvHotelConfigs.Where(t => t.HotelId.Equals(criteria.HotelId)
            && t.ConfigCode.Equals(criteria.ConfigCodes)).ToList();
            mockTVHotelConfigRepertory.Setup(m => m.Search(It.IsAny<BaseSearchCriteria>())).Returns(exceptedHotelConfig);

            var mockhotelConfig = new TVHotelConfig
            {
                HotelId = "TestHotelId1",
                ConfigCode = "TestConfigCode1",
                ConfigName = "Testconfignameinsert",
                Active = true,
                LastUpdater = "admin",
                LastUpdateTime = DateTime.Now
            };

            mockRedisCacheService.Setup(m => m.GetAllFromCache(RedisKey.TvHotelConfigKey, It.IsAny<Func<List<TVHotelConfig>>>())).Returns(tvHotelConfigs);

            mockTVHotelConfigRepertory.Setup(m => m.Insert(mockhotelConfig)).Callback(() =>
            {
                tvHotelConfigs.Add(mockhotelConfig);
            });


            //Act
            tVHotelConfigManager.AddTVHotelConfig(mockhotelConfig);

        }
        [TestMethod]
        public void UpdateTVHotelConfig_ShouldSuccess_WhenUpdateTVHotelConfig()
        {
            //Arrange
            var mockhotelConfig = new TVHotelConfig
            {
                Id = 1,
                HotelId = "TesthotelidInsert1",
                ConfigCode = "TestconfigcodeInsert1",
                ConfigName = "Testconfignameinsert1",
                Active = true,
                LastUpdater = "admin",
                LastUpdateTime = DateTime.Now,
                ConfigValue = "TestModifyConfigValue1"
            };
            mockTVHotelConfigRepertory.Setup(m => m.Update(mockhotelConfig)).Callback(() =>
                {
                    tvHotelConfigs.SingleOrDefault(m => m.ConfigCode == "TestConfigCode1").ConfigValue = mockhotelConfig.ConfigValue;
                });
            mockTVHotelConfigRepertory.Setup(m => m.FindByKey(mockhotelConfig.Id)).Returns<int>(id => tvHotelConfigs.FirstOrDefault(t => t.Id == id));

            mockRedisCacheService.Setup(m => m.GetAllFromCache(RedisKey.TvHotelConfigKey, It.IsAny<Func<List<TVHotelConfig>>>())).Returns(tvHotelConfigs);
            mockTVHotelConfigRepertory.Setup(m => m.Search(It.IsAny<HotelConfigCriteria>())).Returns(tvHotelConfigs);

            //Act
            tVHotelConfigManager.UpdateTVHotelConfig(mockhotelConfig);


            //Assert
            Assert.IsTrue(tvHotelConfigs.Any(m => m.ConfigValue == mockhotelConfig.ConfigValue));
        }
        [TestMethod]
        public void GetEntity_ShouldReturnTVHotelConfig_WhenGiveValidId()
        {
            //Arrange
            int mockid = 1;
            var exceptedtvhotelconfig = tvHotelConfigs.FirstOrDefault(m => m.Id == mockid);
            mockTVHotelConfigRepertory.Setup(m => m.FindByKey(mockid)).Returns<int>(id => tvHotelConfigs.FirstOrDefault(t => t.Id == id));

            mockRedisCacheService.Setup(m => m.IsSet(It.IsAny<string>())).Returns(false);
            mockRedisCacheService.Setup(m => m.GetAllFromCache(RedisKey.TvHotelConfigKey, It.IsAny<Func<List<TVHotelConfig>>>())).Returns(tvHotelConfigs);

            //Act
            var actual = tVHotelConfigManager.GetEntity(mockid);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(exceptedtvhotelconfig, actual);
        }
        [TestMethod]
        public void GetEntity_ShouldReturnnull_WhenGivenotexistId()
        {
            //Arrange
            int mockid = -10;
            mockTVHotelConfigRepertory.Setup(m => m.FindByKey(mockid)).Returns<int>(id => tvHotelConfigs.FirstOrDefault(t => t.Id == id)); ;
            mockRedisCacheService.Setup(m => m.GetAllFromCache(RedisKey.TvHotelConfigKey, It.IsAny<Func<List<TVHotelConfig>>>())).Returns(tvHotelConfigs);

            //Act
            var actual = tVHotelConfigManager.GetEntity(mockid);
            
            //Assert
            Assert.IsNull(actual);

        }
        private List<TVHotelConfig> GetMockTVHotelConfig(int count)
        {
            var tVHotelConfigList = new List<TVHotelConfig>();

            for (int i = 0; i < count; i++)
            {
                tVHotelConfigList.Add(new TVHotelConfig
                {
                    ConfigName = "TestConfigName" + i,
                    Id = i,
                    HotelId = "TestHotelId" + i,
                    ConfigCode = "TestConfigCode" + i,
                    LastUpdateTime = DateTime.Now,
                    LastUpdater = "admin"

                });
            }

            return tVHotelConfigList;
        }
    }
}
