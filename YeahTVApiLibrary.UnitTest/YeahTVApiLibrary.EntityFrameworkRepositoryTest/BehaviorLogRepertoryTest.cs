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
    public class BehaviorLogRepertoryTest : BaseRepertoryTest<BehaviorLog, string>
    {
        public BehaviorLogRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockConfigs(10); };
            base.SetRepertory = () => { return new BehaviorLogRepertory(); };
        }

        [TestMethod]
        public void SearchConfigs_ShouldReturnConfigs_WhenGiveValidCriteria()
        {
            // Arrange
            var criteria = new LogCriteria();
            criteria.AppId = "TestAppId1";
            criteria.LogInfo = "TestBehaviorInfo";
            criteria.LogInfoEx = "TestBehaviorInfoEx";
            criteria.LogType = "TestType";
            criteria.HotelId = "TestHotelId1";

            var excepted = base.mockEntities.Where(m => m.HotelId == criteria.HotelId
                && m.BehaviorInfo.Contains(criteria.LogInfo)
                && m.BehaviorType == criteria.LogType);

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
            var criteria = new LogCriteria();
            criteria.AppId = "TestAppId1sss";
            criteria.LogInfo = "TestBehaviorInfosss";
            criteria.LogInfoEx = "TestBehaviorInfoExss";
            criteria.LogType = "TestTypess";

            // Act 
            var actual = base.entityRepertory.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        private List<BehaviorLog> GetMockConfigs(int count)
        {
            var configs = new List<BehaviorLog>();
            
            for (int i = 0; i < count; i++)
            {
                configs.Add(new BehaviorLog 
                {
                    HotelId = "TestHotelId" + i,
                    BehaviorInfo = "TestBehaviorInfo" + i,
                    DeviceSerise = "TestDeviceSerise" + i,
                    BehaviorType = "TestType",
                    CreateTime = DateTime.Now
                });
            }

            return configs;
        }
    }
}
