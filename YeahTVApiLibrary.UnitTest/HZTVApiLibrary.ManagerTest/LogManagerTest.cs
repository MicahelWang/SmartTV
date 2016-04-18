namespace YeahTVApiLibrary.UnitTests.HZTVApiLibrary.ManagerTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using YeahTVApiLibrary.Manager;

    [TestClass]
    public class LogManagerTest
    {
        private Mock<IBehaviorLogRepertory> mockBehaviorLogRepertory;
        private Mock<ISystemLogRepertory> mockSystemLogRepertory;
        private Mock<IMongoLogRepository> mockMongoLogRepository;
        private ILogManager logManager;
        private string Url = "http://baidu.com";
        private int count = 5;
        List<BehaviorLog> behaviorList;
        List<SystemLog> systemLogList;
        [TestInitialize]
        public void Setup()
        {
            mockBehaviorLogRepertory = new Mock<IBehaviorLogRepertory>();
            mockSystemLogRepertory = new Mock<ISystemLogRepertory>();
            mockMongoLogRepository =new Mock<IMongoLogRepository>();
            behaviorList = GetInitiaBehaviorList(count);
            systemLogList = GetInitialSystemList(count);
            logManager = new LogManager(mockBehaviorLogRepertory.Object, mockSystemLogRepertory.Object, mockMongoLogRepository.Object);
        }
        
        [TestMethod]
        public void SaveError_ShouldSaveErrorMessage_WhenGiveErrorMessage()
        {
            // Arrange
            var mockInfos = new List<MongoLog>();

            var mockAppType = AppType.TV;
            var mockMessageInfo = "TestMessageInfo";

            mockMongoLogRepository.Setup(m => m.Add(It.IsAny<MongoLog>(),null))
                .Callback(() =>
                {
                    mockInfos.Add(new MongoLog
                    {
                        AppType = mockAppType.ToString(),
                        MessageInfo = mockMessageInfo,
                        MessageType = LogType.Error.ToString(),
                    });
                });

            // Act 
            logManager.SaveError(mockMessageInfo, null, mockAppType, null);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Assert
            Assert.IsTrue(mockInfos.Any(m => m.AppType.Equals(mockAppType.ToString())));
            Assert.IsTrue(mockInfos.Any(m => m.MessageInfo.Equals(mockMessageInfo)));
            Assert.IsTrue(mockInfos.Any(m => m.MessageType.Equals(LogType.Error.ToString())));
        }

        [TestMethod]
        public void SaveError_ShouldSaveErrorObject_WhenGiveErrorMessage()
        {
            // Arrange
            var mockInfos = new List<MongoLog>();


            var mockAppType = AppType.TV;
            var mockError = new Exception("TestException");

            mockMongoLogRepository.Setup(m => m.Add(It.IsAny<MongoLog>(),null))
                .Callback(() =>
                {
                    mockInfos.Add(new MongoLog
                    {
                        AppType = mockAppType.ToString(),
                        MessageInfo = mockError.Data.ToString(),
                        MessageType = LogType.Error.ToString(),
                    });
                });

            // Act 
            logManager.SaveError(mockError, null, mockAppType, null);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Assert
            Assert.IsTrue(mockInfos.Any(m => m.AppType.Equals(mockAppType.ToString())));
            Assert.IsTrue(mockInfos.Any(m => m.MessageInfo.Equals(mockError.Data.ToString())));
            Assert.IsTrue(mockInfos.Any(m => m.MessageType.Equals(LogType.Error.ToString())));
        }

        [TestMethod]
        public void SaveInfo_ShouldSaveInfo_WhenGiveInfoMessage()
        {
            // Arrange
            var mockInfos = new List<MongoLog>();

            var mockAppType = AppType.TV;
            var mockMessageInfo = "TestMessageInfo";
             
            mockMongoLogRepository.Setup(m => m.Add(It.IsAny<MongoLog>(),null))
                .Callback(() =>
                {
                    mockInfos.Add(new MongoLog
                    {
                        AppType = mockAppType.ToString(),
                        MessageInfo = mockMessageInfo,
                        MessageType = LogType.Infomation.ToString(),
                    });
                });

            // Act 
            logManager.SaveInfo(mockMessageInfo, null, mockAppType, null);
            Thread.Sleep(TimeSpan.FromSeconds(1));

             //Assert             
            Assert.IsTrue(mockInfos.Any(m => m.AppType.Equals(mockAppType.ToString())));
            Assert.IsTrue(mockInfos.Any(m => m.MessageInfo.Equals(mockMessageInfo)));
            Assert.IsTrue(mockInfos.Any(m => m.MessageType.Equals(LogType.Infomation.ToString())));
        }

        [TestMethod]
        public void SaveWaring_ShouldSaveWaring_WhenGiveWaringMessage()
        {
            // Arrange
            var mockInfos = new List<MongoLog>();

            var mockAppType = AppType.TV;
            var mockMessageInfo = "TestMessageInfo";

            mockMongoLogRepository.Setup(m => m.Add(It.IsAny<MongoLog>(),null))
                .Callback(() =>
                {
                    mockInfos.Add(new MongoLog
                    {
                        AppType = mockAppType.ToString(),
                        MessageInfo = mockMessageInfo,
                        MessageType = LogType.Waring.ToString(),
                    });
                });

            // Act 
            logManager.SaveWarning(mockMessageInfo, mockAppType, null, null);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Assert
            Assert.IsTrue(mockInfos.Any(m => m.AppType.Equals(mockAppType.ToString())));
            Assert.IsTrue(mockInfos.Any(m => m.MessageInfo.Equals(mockMessageInfo)));
            Assert.IsTrue(mockInfos.Any(m => m.MessageType.Equals(LogType.Waring.ToString())));
        }

        [TestMethod]
        public void SaveBehavior_ShouldSaveBehavior_WhenGiveBehaviorMessage()
        {
            // Arrange
            var mockInfos = new List<BehaviorLog>();

            var mockAppid = "TestAppid";
            var mockMessageInfo = "TestMessageInfo";

            mockBehaviorLogRepertory.Setup(m => m.Insert(It.IsAny<BehaviorLog>()))
                .Callback(() =>
                {
                    mockInfos.Add(new BehaviorLog
                    {
                        HotelId = mockAppid,
                        BehaviorInfo = mockMessageInfo,
                        BehaviorType = LogType.UserBehavior.ToString(),
                    });
                });

            // Act 
            logManager.SaveBehavior(mockAppid, null, mockMessageInfo, null);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Assert
            Assert.IsTrue(mockInfos.Any(m => m.HotelId.Equals(mockAppid)));
            Assert.IsTrue(mockInfos.Any(m => m.BehaviorInfo.Equals(mockMessageInfo)));
            Assert.IsTrue(mockInfos.Any(m => m.BehaviorType.Equals(LogType.UserBehavior.ToString())));
        }

        [TestMethod]
        public void SearchSystemLog_ShouldReturnSystemLog_WhenGiveExitCriteria()
        {
            // Arrange
            var mockAppid = "TestAppid";
            var mockMessageInfo = "TestMessageInfo";

            var mockApps = new List<SystemLog>
            {
                new SystemLog
                {
                   AppType = mockAppid,
                   MessageInfo = mockMessageInfo
                }
            };

            mockSystemLogRepertory.Setup(m => m.Search(It.IsAny<LogCriteria>())).Returns(mockApps);

            // Act 
            var actual = logManager.SearchSystemLog(new LogCriteria());

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any(a => a.AppType.Equals(mockAppid)));
            Assert.IsTrue(actual.Any(a => a.MessageInfo.Equals(mockMessageInfo)));
        }

        [TestMethod]
        public void SearchSystemLog_ShouldReturnNull_WhenGiveNotExitCriteria()
        {
            // Arrange
            mockSystemLogRepertory.Setup(m => m.Search(It.IsAny<LogCriteria>())).Returns(new List<SystemLog>());

            // Act 
            var actual = logManager.SearchSystemLog(new LogCriteria());

            // Assert
            Assert.IsTrue(actual.Count == 0);
        }

        [TestMethod]
        public void SearchBehaviorLog_ShouldBehaviorLog_WhenGiveExitCriteria()
        {
            // Arrange
            var mockAppid = "TestAppid";
            var mockMessageInfo = "TestMessageInfo";

            var mockApps = new List<BehaviorLog>
            {
                new BehaviorLog
                {
                   HotelId = mockAppid,
                   BehaviorInfo = mockMessageInfo
                }
            };

            mockBehaviorLogRepertory.Setup(m => m.Search(It.IsAny<LogCriteria>())).Returns(mockApps);

            // Act 
            var actual = logManager.SearchBehaviorLog(new LogCriteria());

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Any(a => a.HotelId.Equals(mockAppid)));
            Assert.IsTrue(actual.Any(a => a.BehaviorInfo.Equals(mockMessageInfo)));
        }

        [TestMethod]
        public void SearchBehaviorLog_ShouldReturnNull_WhenGiveNotExitCriteria()
        {
            // Arrange
            mockBehaviorLogRepertory.Setup(m => m.Search(It.IsAny<LogCriteria>())).Returns(new List<BehaviorLog>());

            // Act 
            var actual = logManager.SearchSystemLog(new LogCriteria());

            // Assert
            Assert.IsNull(actual);
        }
        /// <summary>
        //
        /// </summary>
        [TestMethod]
        public void TestTransaction_ShouldSaveBehaviorAndSystemLog_WhenGiveMessage()
        {
            var message = "this is a test";
            var moreInfo = "MoreInfo";
            var appType = AppType.AppCenter.ToString();
            // Arrange
            mockBehaviorLogRepertory.Setup(m => m.Insert(It.IsAny<BehaviorLog>()))
                .Callback<BehaviorLog>((b) =>
                {
                    behaviorList.Add(b);
                });
            mockSystemLogRepertory.Setup(m => m.Insert(It.IsAny<SystemLog>()))
                .Callback<SystemLog>((s) =>
                {
                    systemLogList.Add(s);
                });

            // Act 
            logManager.TestTransaction(Url, message, moreInfo, appType);

            // Assert
            Assert.IsNotNull(systemLogList.Find(m => m.AppType == AppType.AppCenter.ToString()));
            Assert.AreEqual(6, behaviorList.Count);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<SystemLog> GetInitialSystemList(int count)
        {
            List<SystemLog> systemList = new List<SystemLog>();
            for (int i = 0; i < count; i++)
            {
                systemList.Add(new SystemLog()
                {
                    Id = i,
                    AppType = AppType.AppCenter.ToString(),
                    CreateTime = DateTime.Now,
                    MessageInfo = "Messages" + i,
                    MessageInfoEx = "MessageInfoEx",
                    MessageType = "Test"
                });
            }
            return systemList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<BehaviorLog> GetInitiaBehaviorList(int count)
        {
            List<BehaviorLog> behaviorList = new List<BehaviorLog>();
            for (int i = 0; i < count; i++)
            {
                behaviorList.Add(new BehaviorLog()
                {
                    HotelId = "HotelId" + i,
                    Id = i.ToString(),
                    BehaviorInfo = "behaviorInfo" + i,
                    DeviceSerise = "DeviceSerise",
                    BehaviorType = "Behaviortype",
                    CreateTime = DateTime.Now
                });
            }
            return behaviorList;
        }

    }
}