using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YeahAppCentre.Controllers;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Tests.Controllers
{
    [TestClass]
    public class BehaviorLogControllerTest
    {
        private BehaviorLogController behaviorLogController;
        private Mock<IBehaviorLogManager> mockManager;
        private Mock<IHttpContextService> mockHttpContextService;
        private Mock<ILogManager> mockLogManager;
        private List<BehaviorLog> mockBehaviorLogs;

        [TestInitialize]
        public void Setup()
        {
            mockBehaviorLogs = GetMockBehaviorLogs(30);
            mockManager = new Mock<IBehaviorLogManager>();
            mockLogManager = new Mock<ILogManager>();
            mockHttpContextService = new Mock<IHttpContextService>();
            behaviorLogController = new BehaviorLogController(
                mockManager.Object,
                mockLogManager.Object,
                mockHttpContextService.Object);
        }

        [TestMethod]
        public void Index_ShouldReturnList_WhenGiveValidCriteria()
        {
            //Arrange
            var expectedBehaviorLogs = mockBehaviorLogs.OrderBy(m => m.Id).Take(15).ToList();

            var logCriteria = new LogCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 0,
                OrderAsc = true,
                SortFiled = "Id"
            };
            mockManager.Setup(m => m.Search(logCriteria)).Returns(expectedBehaviorLogs);

            //Act
            var actual = behaviorLogController.Index(logCriteria) as ViewResult;
            var behaviorLogList = actual.ViewBag.List as PagedViewList<BehaviorLog>;

            //Assert
            Assert.IsNotNull(actual.ViewBag.List);
            expectedBehaviorLogs.ForEach(m =>
            {
                Assert.IsTrue(behaviorLogList.Source.Contains(m));
            });
            Assert.AreEqual(expectedBehaviorLogs.Count, behaviorLogList.Source.Count);
        }

        [TestMethod]
        public void Index_ShouldReturnEmptyList_WhenGiveErrorPage()
        {
            //Arrange
            var expectedBehaviorLogs = mockBehaviorLogs.OrderBy(m => m.Id).Take(15).ToList();

            var logCriteria = new LogCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 10,
                OrderAsc = true,
                SortFiled = "Id"
            };
            mockManager.Setup(m => m.Search(logCriteria)).Returns(() => new List<BehaviorLog>());

            //Act
            var actual = behaviorLogController.Index(logCriteria) as ViewResult;
            var behaviorLogList = actual.ViewBag.List as PagedViewList<BehaviorLog>;

            //Assert
            Assert.AreEqual(0, behaviorLogList.Source.Count);
        }

        [TestMethod]
        public void GetBehaviorLogList_ShouldReturnBehaviorLogs_WhenGiveValidCriteria()
        {
            //Arrange
            var expectedBehaviorLogs = mockBehaviorLogs.OrderBy(m => m.Id).Take(15).ToList();
            var logCriteria = new LogCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 0,
                OrderAsc = true,
                SortFiled = "Id"
            };

            mockManager.Setup(m => m.Search(logCriteria)).Returns(() => expectedBehaviorLogs);

            //Act
            var actual = behaviorLogController.List(logCriteria) as PartialViewResult;
            var hotelList = actual.Model as PagedViewList<BehaviorLog>;

            //Assert            
            expectedBehaviorLogs.ForEach(m =>
            {
                Assert.IsTrue(hotelList.Source.Contains(m));
            });
            Assert.AreEqual(expectedBehaviorLogs.Count, hotelList.Source.Count);
        }

        [TestMethod]
        public void GetBehaviorLogList_ShouldReturnEmptyList_WhenGiveErrorPage()
        {
            //Arrange
            var expectedBehaviorLogs = mockBehaviorLogs.OrderBy(m => m.Id).Take(15).ToList();
            var logCriteria = new LogCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 10,
                OrderAsc = true,
                SortFiled = "Id"
            };

            mockManager.Setup(m => m.Search(logCriteria)).Returns(() => new List<BehaviorLog>());

            //Act
            var actual = behaviorLogController.List(logCriteria) as PartialViewResult;
            var hotelList = actual.Model as PagedViewList<BehaviorLog>;

            //Assert            
            Assert.AreEqual(0, hotelList.Source.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
        public void GetBehaviorLogList_ShouldThrowException_WhenGiveNotExistSortFiled()
        {
            //Arrange
            var expectedHotels = mockBehaviorLogs.OrderBy(m => m.Id).Take(15).ToList();
            var logCriteria = new LogCriteria()
            {
                NeedPaging = true,
                PageSize = 15,
                Page = 10,
                OrderAsc = true,
                SortFiled = "NoExistId"
            };

            mockManager.Setup(m => m.Search(logCriteria)).Throws(new Exception("SortFiled Error!"));

            //Act
            var actual = behaviorLogController.List(logCriteria) as PartialViewResult;
        }

        [TestMethod]
        public void HttpGetEdit_ShouldReturnBehaviorLog_WhenGiveExistHotelId()
        {
            //Arrange
            var expectedBehaviorLog = mockBehaviorLogs.FirstOrDefault();

            mockManager.Setup(m => m.GetById(expectedBehaviorLog.Id)).Returns(() => expectedBehaviorLog);

            //Act
            var actual = behaviorLogController.Edit(expectedBehaviorLog.Id, YeahTVApi.DomainModel.Enum.OpType.View) as PartialViewResult;

            //Assert
            Assert.IsNotNull(actual.Model);
            Assert.AreEqual(YeahTVApi.DomainModel.Enum.OpType.View, actual.ViewBag.OpType);
        }

        [TestMethod]
        public void HttpGetEdit_ShouldReturnNull_WhenGiveNotExistHotelId()
        {
            //Arrange
            var expectedHotelId = "99";

            mockManager.Setup(m => m.GetById(expectedHotelId)).Returns(() => null);

            //Act
            var actual = behaviorLogController.Edit(expectedHotelId, YeahTVApi.DomainModel.Enum.OpType.View) as PartialViewResult;
            var actualBehaviorLog = actual.Model as BehaviorLog;

            //Assert
            Assert.IsNull(actualBehaviorLog);
        }

        #region MockList
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
        #endregion
    }
}
