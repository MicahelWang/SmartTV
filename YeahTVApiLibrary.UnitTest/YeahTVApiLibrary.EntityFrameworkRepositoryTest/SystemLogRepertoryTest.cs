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
    public class SystemLogRepertoryTest : BaseRepertoryTest<SystemLog, int>
    {
        public SystemLogRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockConfigs(10); };
            base.SetRepertory = () => { return new SystemLogRepertory(); };
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

            var excepted = base.mockEntities.Where(m => m.AppType == criteria.AppId
                && m.MessageInfo.Contains(criteria.LogInfo)
                && m.MessageInfoEx.Contains(criteria.LogInfoEx)
                && m.MessageType == criteria.LogType);

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

        private List<SystemLog> GetMockConfigs(int count)
        {
            var configs = new List<SystemLog>();
            
            for (int i = 0; i < count; i++)
            {
                configs.Add(new SystemLog 
                {
                    AppType = "TestAppId" + i,
                    MessageInfo = "TestMessageInfo" + i,
                    MessageInfoEx = "TestMessageInfoEx" + i,
                    MessageType = "TestType",
                    CreateTime = DateTime.Now
                });
            }

            return configs;
        }
    }
}
