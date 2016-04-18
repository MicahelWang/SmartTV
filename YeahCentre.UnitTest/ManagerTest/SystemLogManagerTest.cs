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
    public class SystemLogManagerTest
    {
        private Mock<ISystemLogRepertory> mockSystemLogRepertory;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private ISystemLogManager manager;
        private List<SystemLog> mockSystemLogs;

        [TestInitialize]
        public void Setup()
        {
            mockSystemLogs = GetMockSystemLogs(10);
            mockSystemLogRepertory = new Mock<ISystemLogRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();

            manager = new SystemLogManager(mockSystemLogRepertory.Object);
        }

        [TestMethod]
        public void GetSystemLogById_ShouldReturnSystemLog_WhenGiveExistSystemLogId()
        {
            //Arrange
            var expectedSystemLogId = 2;
            var expectedSystemLog = mockSystemLogs.Where(m => m.Id == expectedSystemLogId).FirstOrDefault();

            mockSystemLogRepertory.Setup(m => m.FindByKey(expectedSystemLogId)).Returns(expectedSystemLog);

            //Act
            var actual = manager.GetById(expectedSystemLogId);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreSame(expectedSystemLog, actual);
        }

        [TestMethod]
        public void GetSystemLogById_ShouldReturnNull_WhenGiveNotExistSystemLogId()
        {
            //Arrange
            var expectedSystemLogId = 99;

            mockSystemLogRepertory.Setup(m => m.FindByKey(expectedSystemLogId)).Returns((int id) =>
            {
                return mockSystemLogs.FirstOrDefault(m => m.Id == expectedSystemLogId);
            });

            //Act
            var actual = manager.GetById(expectedSystemLogId);

            //Assert
            Assert.IsNull(actual);
        }


        [TestMethod]
        public void SearchSystemLogs_ShouldReturnSystemLogs_WhenGiveValidCriteria()
        {
            //Arrange
            var mockSystemLogCriteria = new LogCriteria()
            {
                LogInfo = "Test"
            };
            var expectedSystemLogs = mockSystemLogs.Where(m => m.MessageInfo.Contains(mockSystemLogCriteria.LogInfo)).ToList();

            mockSystemLogRepertory.Setup(m => m.Search(mockSystemLogCriteria)).Returns<LogCriteria>((s) =>
                mockSystemLogs.Where(m => m.MessageInfo.Contains(s.LogInfo)).ToList());

            //Act
            var actual = manager.Search(mockSystemLogCriteria);

            //Assert
            Assert.IsNotNull(actual);
            expectedSystemLogs.ForEach(m =>
            {
                Assert.IsTrue(actual.Contains(m));
            });
            Assert.AreEqual(expectedSystemLogs.Count, actual.Count);
        }

        [TestMethod]
        public void SearchSystemLogs_ShouldReturnEmpty_WhenGiveNotExistCriteria()
        {
            //Arrange
            var mockSystemLogCriteria = new LogCriteria()
            {
                LogInfo = "Testddddddd"
            };
            var expectedSystemLogs = mockSystemLogs.Where(m => m.MessageInfo.Contains(mockSystemLogCriteria.LogInfo)).ToList();

            mockSystemLogRepertory.Setup(m => m.Search(mockSystemLogCriteria)).Returns<LogCriteria>((s) =>
                mockSystemLogs.Where(m => m.MessageInfo.Contains(s.LogInfo)).ToList());

            //Act
            var actual = manager.Search(mockSystemLogCriteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        private List<SystemLog> GetMockSystemLogs(int count)
        {
            var SystemLogs = new List<SystemLog>();

            for (int i = 0; i < count; i++)
            {
                SystemLogs.Add(new SystemLog
                {
                    Id = i,
                    AppType = "TestAppType" + i,
                    MessageInfo = "TestMessageInfo" + i,
                    MessageInfoEx = "TestMessageInfoEx" + i,
                    MessageType = "TestMessageType" + i,
                    CreateTime = DateTime.Now
                });
            }

            return SystemLogs;
        }
    }
}
