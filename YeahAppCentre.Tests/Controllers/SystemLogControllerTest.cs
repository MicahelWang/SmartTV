using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YeahAppCentre.Controllers;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Tests.Controllers
{
    [TestClass]
    public class SystemLogControllerTest
    {
        private SystemLogController SystemLogController;
        private Mock<ISystemLogManager> mockManager;
        private Mock<IHttpContextService> mockHttpContextService;
        private Mock<ILogManager> mockLogManager;
        private List<MongoLog> mockSystemLogs;
        private Mock<IMongoLogManager> mockMongoLogManager;
        [TestInitialize]
        public void Setup()
        {
            mockSystemLogs = GetMockSystemLogs(30);
            mockMongoLogManager = new Mock<IMongoLogManager>();
            mockLogManager = new Mock<ILogManager>();
            mockManager = new Mock<ISystemLogManager>();
            mockHttpContextService = new Mock<IHttpContextService>();
            SystemLogController = new SystemLogController(
                mockManager.Object,
                mockLogManager.Object,
                mockMongoLogManager.Object);
        }

        [TestMethod]
        public void Index_ShouldReturnList_WhenGiveValidCriteria()
        {
            //Arrange
            var expectedSystemLogs = mockSystemLogs.OrderBy(m => m.CreateTime).Take(15).ToList();

            var logCriteria = new MongoCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 0,
                OrderAsc = true,
                SortFiled = "Id"
            };
            mockMongoLogManager.Setup(m => m.Search(logCriteria)).Returns(expectedSystemLogs);

            //Act
            var actual = SystemLogController.Index(logCriteria) as ViewResult;
            var SystemLogList = actual.ViewBag.List as PagedViewList<MongoLog>;

            //Assert
            Assert.IsNotNull(actual.ViewBag.List);
            expectedSystemLogs.ForEach(m =>
            {
                Assert.IsTrue(SystemLogList.Source.Contains(m));
            });
            Assert.AreEqual(expectedSystemLogs.Count, SystemLogList.Source.Count);
        }

        [TestMethod]
        public void Index_ShouldReturnEmptyList_WhenGiveErrorPage()
        {
            //Arrange
            var expectedSystemLogs = mockSystemLogs.OrderBy(m => m.CreateTime).Take(15).ToList();

            var logCriteria = new MongoCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 10,
                OrderAsc = true,
                SortFiled = "Id"
            };
            mockMongoLogManager.Setup(m => m.Search(logCriteria)).Returns(() => new List<MongoLog>());

            //Act
            var actual = SystemLogController.Index(logCriteria) as ViewResult;
            var SystemLogList = actual.ViewBag.List as PagedViewList<MongoLog>;

            //Assert
            Assert.AreEqual(0, SystemLogList.Source.Count);
        }

        [TestMethod]
        public void GetSystemLogList_ShouldReturnSystemLogs_WhenGiveValidCriteria()
        {
            //Arrange
            var expectedSystemLogs = mockSystemLogs.OrderBy(m => m.CreateTime).Take(15).ToList();
            var logCriteria = new MongoCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 0,
                OrderAsc = true,
                SortFiled = "Id"
            };

            mockMongoLogManager.Setup(m => m.Search(logCriteria)).Returns(() => expectedSystemLogs);

            //Act
            var actual = SystemLogController.List(logCriteria) as PartialViewResult;
            var hotelList = actual.Model as PagedViewList<MongoLog>;

            //Assert            
            expectedSystemLogs.ForEach(m =>
            {
                Assert.IsTrue(hotelList.Source.Contains(m));
            });
            Assert.AreEqual(expectedSystemLogs.Count, hotelList.Source.Count);
        }

        [TestMethod]
        public void GetSystemLogList_ShouldReturnEmptyList_WhenGiveErrorPage()
        {
            //Arrange
            var expectedSystemLogs = mockSystemLogs.OrderBy(m => m.CreateTime).Take(15).ToList();
            var logCriteria = new MongoCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 10,
                OrderAsc = true,
                SortFiled = "Id"
            };

            mockMongoLogManager.Setup(m => m.Search(logCriteria)).Returns(() => new List<MongoLog>());

            //Act
            var actual = SystemLogController.List(logCriteria) as PartialViewResult;
            var hotelList = actual.Model as PagedViewList<MongoLog>;

            //Assert            
            Assert.AreEqual(0, hotelList.Source.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void GetSystemLogList_ShouldThrowException_WhenGiveNotExistSortFiled()
        {
            //Arrange
            var expectedHotels = mockSystemLogs.OrderBy(m => m.CreateTime).Take(15).ToList();
            var logCriteria = new MongoCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 10,
                OrderAsc = true,
                SortFiled = "NoExistId"
            };

            mockMongoLogManager.Setup(m => m.Search(logCriteria)).Throws(new Exception("SortFiled Error!"));

            //Act
            var actual = SystemLogController.List(logCriteria) as PartialViewResult;
        }

        [TestMethod]
        public void HttpGetEdit_ShouldReturnSystemLog_WhenGiveExistHotelId()
        {
            //Arrange
            var expectedSystemLog = mockSystemLogs.FirstOrDefault();
            string id = Convert.ToString(expectedSystemLog.Id);
            mockMongoLogManager.Setup(m => m.GetById(id)).Returns(() => expectedSystemLog);

            //Act
            var actual = SystemLogController.Edit(id, YeahTVApi.DomainModel.Enum.OpType.View) as PartialViewResult;

            //Assert
            Assert.IsNotNull(actual.Model);
            Assert.AreEqual(YeahTVApi.DomainModel.Enum.OpType.View, actual.ViewBag.OpType);
        }

        [TestMethod]
        public void HttpGetEdit_ShouldReturnNull_WhenGiveNotExistHotelId()
        {
            //Arrange
            var expectedHotelId = "99";

            mockMongoLogManager.Setup(m => m.GetById(expectedHotelId)).Returns(() => null);

            //Act
            var actual = SystemLogController.Edit(expectedHotelId, YeahTVApi.DomainModel.Enum.OpType.View) as PartialViewResult;
            var actualSystemLog = actual.Model as MongoLog;

            //Assert
            Assert.IsNull(actualSystemLog);
        }

        #region MockList
        private List<MongoLog> GetMockSystemLogs(int count)
        {
            var SystemLogs = new List<MongoLog>();

            for (int i = 0; i < count; i++)
            {
                
                SystemLogs.Add(new MongoLog
                {
                    Id = new ObjectId(),
                    AppType = "TestAppType" + i,
                    MessageInfo = "TestMessageInfo" + i,
                    MessageEx = "TestMessageInfoEx" + i,
                    MessageType = "TestMessageType" + i,
                    CreateTime = DateTime.Now
                });
            }

            return SystemLogs;
        }
        #endregion
    }
}
