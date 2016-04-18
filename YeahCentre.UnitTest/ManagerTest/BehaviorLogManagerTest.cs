using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahCenter.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahCentre.Manager;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahCentre.UnitTest.ManagerTest
{
    [TestClass]
    public class BehaviorLogManagerTest
    {
        private Mock<IBehaviorLogRepertory> mockBehaviorLogRepertory;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private IBehaviorLogManager manager;
        private List<BehaviorLog> mockBehaviorLogs;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;

        [TestInitialize]
        public void Setup()
        {
            mockBehaviorLogs = GetMockBehaviorLogs(10);
            mockBehaviorLogRepertory = new Mock<IBehaviorLogRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();

            manager = new BehaviorLogManager(mockBehaviorLogRepertory.Object, mockRedisCacheService.Object, mockConstantSystemConfigManager.Object);
        }

        [TestMethod]
        public void GetBehaviorLogById_ShouldReturnBehaviorLog_WhenGiveExistBehaviorLogId()
        {
            //Arrange
            var expectedBehaviorLogId = "2";
            var expectedBehaviorLog = mockBehaviorLogs.Where(m => m.Id == expectedBehaviorLogId).FirstOrDefault();

            mockBehaviorLogRepertory.Setup(m => m.FindByKey(expectedBehaviorLogId)).Returns(expectedBehaviorLog);

            //Act
            var actual = manager.GetById(expectedBehaviorLogId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreSame(expectedBehaviorLog, actual);
        }

        [TestMethod]
        public void GetBehaviorLogById_ShouldReturnNull_WhenGiveNotExistBehaviorLogId()
        {
            //Arrange
            var expectedBehaviorLogId = "99";

            mockBehaviorLogRepertory.Setup(m => m.FindByKey(expectedBehaviorLogId)).Returns((string id) => {
                return mockBehaviorLogs.FirstOrDefault(m => m.Id == expectedBehaviorLogId);
            });

            //Act
            var actual = manager.GetById(expectedBehaviorLogId);

            //Assert
            Assert.IsNull(actual);
        }


        [TestMethod]
        public void SearchBehaviorLogs_ShouldReturnBehaviorLogs_WhenGiveValidCriteria()
        {
            //Arrange
            var mockBehaviorLogCriteria = new LogCriteria()
            {
                LogInfo = "Test"
            };
            var expectedBehaviorLogs = mockBehaviorLogs.Where(m => m.BehaviorInfo.Contains(mockBehaviorLogCriteria.LogInfo)).ToList();

            mockBehaviorLogRepertory.Setup(m => m.Search(mockBehaviorLogCriteria)).Returns<LogCriteria>((s) =>
                mockBehaviorLogs.Where(m => m.BehaviorInfo.Contains(s.LogInfo)).ToList());

            //Act
            var actual = manager.Search(mockBehaviorLogCriteria);

            //Assert
            Assert.IsNotNull(actual);
            expectedBehaviorLogs.ForEach(m =>
            {
                Assert.IsTrue(actual.Contains(m));
            });
            Assert.AreEqual(expectedBehaviorLogs.Count, actual.Count);
        }

        [TestMethod]
        public void SearchBehaviorLogs_ShouldReturnEmpty_WhenGiveNotExistCriteria()
        {
            //Arrange
            var mockBehaviorLogCriteria = new LogCriteria()
            {
                LogInfo = "Testddddddd"
            };
            var expectedBehaviorLogs = mockBehaviorLogs.Where(m => m.BehaviorInfo.Contains(mockBehaviorLogCriteria.LogInfo)).ToList();

            mockBehaviorLogRepertory.Setup(m => m.Search(mockBehaviorLogCriteria)).Returns<LogCriteria>((s) =>
                mockBehaviorLogs.Where(m => m.BehaviorInfo.Contains(s.LogInfo)).ToList());

            //Act
            var actual = manager.Search(mockBehaviorLogCriteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        private List<BehaviorLog> GetMockBehaviorLogs(int count)
        {
            var behaviorLogs = new List<BehaviorLog>();

            for (int i = 0; i < count; i++)
            {
                behaviorLogs.Add(new BehaviorLog
                {
                    Id = i.ToString(),
                    HotelId = "TestHotelId" + i,
                    BehaviorInfo = "TestBehaviorInfo" + i,
                    BehaviorType = "TestBehaviorType" + i,
                    DeviceSerise = "DeviceSerise" + i,
                    CreateTime = DateTime.Now
                });
            }

            return behaviorLogs;
        }
    }
}
