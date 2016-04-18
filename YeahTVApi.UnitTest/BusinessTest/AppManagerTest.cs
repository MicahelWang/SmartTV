using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApi.Infrastructure;
using Moq; 
using YeahTVApi.Entity;
using Moq.Language;
using System.Data;
using Moq.Linq;
using System.Collections.Generic;
using System.Linq;

//namespace YeahTVApi.UnitTest
//{
//    [TestClass]
//    public class AppManagerTest
//    {
//        private Mock<ITVAppsRepertory> appsRepertory;
//        private Mock<IHotelManager> mockHotelManager;
//        private Mock<ITVModelRepertory> mockModelRepertory;
//        private Mock<IRedisCacheManager> mockRedisCacheManager;
//        private Mock<ITVTraceRepertory> mockTVTraceRepertory;
//        private IAppManager appManager;

//        [TestInitialize]
//        public void Setup()
//        {
//            appsRepertory = new Mock<ITVAppsRepertory>();
//            mockHotelManager = new Mock<IHotelManager>();
//            mockModelRepertory = new Mock<ITVModelRepertory>();
//            mockRedisCacheManager = new Mock<IRedisCacheManager>();
//            mockTVTraceRepertory = new Mock<ITVTraceRepertory>();

//            appManager = new AppManager(
//                mockHotelManager.Object,
//                mockModelRepertory.Object,
//                mockRedisCacheManager.Object, 
//                mockTVTraceRepertory.Object);
//        }

//        [TestMethod]
//        public void GetTrace_ShouldReturnTvTrace_WhenGiveValidHeader()
//        {
//            // Arrange
//            var mockHeader = new RequestHeader
//            {
//                DEVNO = "Test"
//            };

//            var excepted = MockGetTraceIDataReader();

//            mockTVTraceRepertory.Setup(m => m.Search(new TVTraceModelCriteria { DeviceSeries = mockHeader.DEVNO })).Returns(excepted);

//            // Act 
//            var actual = appManager.GetTrace(mockHeader);

//            // Assert
//            Assert.IsNotNull(actual);
//            Assert.AreEqual(excepted.GetBoolean(excepted.GetOrdinal("Active")), actual.Active);
//            Assert.AreEqual(excepted.GetValue(excepted.GetOrdinal("Hotel_ID")).ToString(), actual);
//            Assert.AreEqual(excepted.GetValue(excepted.GetOrdinal("Room_No")).ToString(), actual.RoomNo);
//            Assert.AreEqual(excepted.GetValue(excepted.GetOrdinal("TV_KEY")).ToString(), actual.TV_KEY);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(NullReferenceException))]
//        public void GetTrace_ShouldReturnThrownException_WhenGiveWorngHeader()
//        {
//            // Arrange
//            RequestHeader mockHeader = null;

//            var excepted = MockGetTraceIDataReader(false);

//            mockApiDBManager.Setup(m => m.GetTVTrace(mockHeader)).Returns(excepted);

//            // Act 
//            var actual = appManager.GetTrace(mockHeader);
//        }

//        [TestMethod]
//        public void GetTVVersionList_ShouldReturnTvTrace_WhenHasApps()
//        {
//            // Arrange
//            var excepted = MockGetTVVersionListIDataReader();

//            mockApiDBManager.Setup(m => m.GetAppVersionList()).Returns(excepted);

//            // Act 
//            var actual = appManager.GetTVVersionList();

//            // Assert
//            Assert.IsNotNull(actual);
//            foreach (var a in actual)
//            {
//                Assert.AreEqual(excepted.GetString(0), a.Key);
//                Assert.AreEqual(excepted.GetString(0), a.Value.APP_ID);
//                Assert.AreEqual(excepted.GetString(1), a.Value.NAME);
//                Assert.AreEqual(excepted.GetString(2), a.Value.PLATFORM);
//                Assert.AreEqual(excepted.GetString(3), a.Value.DESCRIPTION);
//                Assert.AreEqual(excepted.GetString(4), a.Value.APP_KEY);
//                Assert.AreEqual(excepted.GetBoolean(5), a.Value.ACTIVE);
//                Assert.AreEqual(excepted.GetString(6), a.Value.SECURE_KEY);
//            }
           
//        }

//        [TestMethod]
//        public void GetTVVersionList_ShouldReturnThrownException_WhenAppsIsNull()
//        {
//            // Arrange
//            var excepted = MockGetTraceIDataReader(false);

//            mockApiDBManager.Setup(m => m.GetAppVersionList()).Returns(excepted);

//            // Act 
//            var actual = appManager.GetTVVersionList();

//            // Assert
//            Assert.IsFalse(actual.Count>0);
//        }

    

       // #region private methods

//        private IDataReader MockGetTVVersionListIDataReader(bool hasValue = true)
//        {
//            var moq = new Mock<IDataReader>();

//            bool readToggle = true;

//            moq.Setup(x => x.Read())
//                // Returns value of local variable 'readToggle' (note that 
//                // you must use lambda and not just .Returns(readToggle) 
//                // because it will not be lazy initialized then)
//                .Returns(() => readToggle)
//                // After 'Read()' is executed - we change 'readToggle' value 
//                // so it will return false on next calls of 'Read()'
//                .Callback(() => readToggle = false);

//            if (!hasValue)
//                return moq.Object;

//            moq.Setup(x => x.GetString(0))
//              .Returns("TestAPP_ID");

//            moq.Setup(x => x.GetString(1))
//               .Returns("TestNAME");

//            moq.Setup(x => x.GetString(2))
//               .Returns("TestPLATFORM");

//            moq.Setup(x => x.GetString(3))
//                .Returns("TestDESCRIPTION");

//            moq.Setup(x => x.GetString(4))
//               .Returns("TestAPP_KEY");

//            moq.Setup(x => x.GetBoolean(5))
//               .Returns(true);

//            moq.Setup(x => x.GetString(6))
//               .Returns("TestSECURE_KEY");

//            return moq.Object;
//        }

//        private IDataReader MockGetTraceIDataReader(bool hasValue = true)
//        {
//            var moq = new Mock<IDataReader>();

//            bool readToggle = true;

//            moq.Setup(x => x.Read())
//                // Returns value of local variable 'readToggle' (note that 
//                // you must use lambda and not just .Returns(readToggle) 
//                // because it will not be lazy initialized then)
//                .Returns(() => readToggle)
//                // After 'Read()' is executed - we change 'readToggle' value 
//                // so it will return false on next calls of 'Read()'
//                .Callback(() => readToggle = false);

//            if (!hasValue)
//                return moq.Object;

//            moq.Setup(x => x.GetOrdinal("Active"))
//               .Returns(0);

//            moq.Setup(x => x.GetOrdinal("Hotel_ID"))
//              .Returns(1);

//            moq.Setup(x => x.GetOrdinal("Room_No"))
//              .Returns(2);

//            moq.Setup(x => x.GetOrdinal("TV_KEY"))
//              .Returns(3);

//            moq.Setup(x => x.GetBoolean(0))
//              .Returns(true);

//            moq.Setup(x => x.GetValue(1))
//               .Returns("TestHotel_ID");

//            moq.Setup(x => x.GetValue(2))
//               .Returns("TestRoom_No");

//            moq.Setup(x => x.GetValue(3))
//                .Returns("TestTV_KEY");

//            return moq.Object;
//        }

//        #endregion
//    }
//}
