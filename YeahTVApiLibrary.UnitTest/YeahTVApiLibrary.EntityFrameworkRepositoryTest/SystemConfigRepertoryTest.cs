using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.EntityFrameworkRepository;
using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.SearchCriteria;
using System.Linq;
using YeahTVApi.DomainModel;
using YeahTVApiLibrary.EntityFrameworkRepository.Models;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.EntityFrameworkRepositoryTest
{
    [TestClass]
    public class SystemConfigRepertoryTest : BaseRepertoryTest<SystemConfig, int>
    {
        public SystemConfigRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockConfigs(10); };
            base.SetRepertory = () => { return new SystemConfigRepertory(); };
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

            var excepted = base.mockEntities.Where(m => m.Active == criteria.Enable
                && m.ConfigType == criteria.AppType
                && m.ConfigName == criteria.ConfigName
                && m.ConfigValue == criteria.ConfigValue);

            // Act 
            var actual = base.entityRepertory.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.Count(), actual.Count);
        }

        [TestMethod]
        public void SearchConfigs_ShouldReturnNull_WhenConfigIsNotExit()
        {
            // Arrange
            var criteria = new SystemConfigCriteria();
            criteria.AppType = AppType.AppCenter.ToString();
            criteria.ConfigName = "TestConfigName1sss";
            criteria.ConfigValue = "TestConfigValue1sss";
            criteria.Enable = true;

            // Act 
            var actual = base.entityRepertory.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
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
                    LastUpdateTime = DateTime.Now
                });
            }

            return configs;
        }
    }
}
