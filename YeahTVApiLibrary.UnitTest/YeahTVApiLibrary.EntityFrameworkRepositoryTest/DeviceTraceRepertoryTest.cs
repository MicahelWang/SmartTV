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
    public class DeviceTraceRepertoryTest : BaseRepertoryTest<DeviceTrace, string>
    {
        public DeviceTraceRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockDeviceTraces(100); };
            base.SetRepertory = () => { return new DeviceTraceRepertory(); };
        }

        [TestMethod]
        public void SearchTraces_ShouldReturnTraces_WhenGiveValidCriteria()
        {
            // Arrange
            var criteria = new DeviceTraceCriteria();
            criteria.DeviceSeries = "TestDeviceSeries1";
            criteria.Platfrom = "TestPlatfrom1";
            criteria.HotelId = "Test1";

            var excepted = base.mockEntities.Where(m => m.DeviceSeries.Contains(criteria.DeviceSeries)
                && m.Platfrom.Contains(criteria.Platfrom)
                && m.HotelId.Equals(criteria.HotelId));

            // Act 
            var actual = base.entityRepertory.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.Count(), actual.Count);
        }

         [TestMethod]
        public void test()
        {
            // Arrange
            var criteria = new DeviceTraceCriteria();
            criteria.PageSize = 1000;
            criteria.Page = 0;
            criteria.HotelId = "7e0abcf540414495ab46fb320a400fcf";
            criteria.NeedPaging = true;

            // Act 
            var deviceTraceRepertory = base.entityRepertory as DeviceTraceRepertory;
            var actual = deviceTraceRepertory.SearchOrderByRoomNo(criteria);


        }

        [TestMethod]
        public void SearchTraces_ShouldReturnTwentyTraces_WhenGiveValidCriteriaNeedPageing()
        {
            // Arrange
            var criteria = new DeviceTraceCriteria();
            criteria.DeviceSeries = "TestDeviceSeries";
            criteria.Platfrom = "TestPlatfrom";
            criteria.HotelId = "Test";
            criteria.NeedPaging = true;

            var excepted = base.mockEntities.Where(m => m.DeviceSeries.Contains(criteria.DeviceSeries)
                && m.Platfrom.Contains(criteria.Platfrom)
                && m.HotelId.Equals(criteria.HotelId))
                .Page(criteria.PageSize, criteria.Page);

            // Act 
            var actual = base.entityRepertory.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.Count(), actual.Count);
        }

        [TestMethod]
        public void SearchTraces_ShouldReturnNull_WhenTraceIsNotExit()
        {
            // Arrange
            var criteria = new DeviceTraceCriteria();
            criteria.DeviceSeries = "TestDeviceSeries1ss";
            criteria.Platfrom = "TestPlatfrom1sss";
            criteria.HotelId = "Test1sss";

            // Act 
            var actual = base.entityRepertory.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GetSingleTrace_ShouldReturnTrace_WhenGiveValidCriteria()
        {
            // Arrange
            var criteria = new DeviceTraceCriteria();
            criteria.DeviceSeries = "TestDeviceSeries1";
            criteria.Platfrom = "TestPlatfrom1";
            criteria.HotelId = "Test1";

            var excepted = base.mockEntities.Where(m => m.DeviceSeries.Equals(criteria.DeviceSeries)
                && m.Platfrom.Equals(criteria.Platfrom)
                && m.HotelId.Equals(criteria.HotelId)).FirstOrDefault();

            var traceRepertory = base.entityRepertory as DeviceTraceRepertory;

            // Act 
            var actual = traceRepertory.GetSingle(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.DeviceSeries, actual.DeviceSeries);
        }

        [TestMethod]
        public void GetSingleTrace_ShouldReturnNull_WhenTraceIsNotExit()
        {
            // Arrange
            var criteria = new DeviceTraceCriteria();
            criteria.DeviceSeries = "TestDeviceSeries1adasd";
            criteria.Platfrom = "TestPlatfrom1adasd";
            criteria.HotelId = "Test1asdas";

            var traceRepertory = base.entityRepertory as DeviceTraceRepertory;

            // Act 
            var actual = traceRepertory.GetSingle(criteria);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void DeviceSeriesFilter_ShouldReturnList_WhenGiveValidHotelId()
        {
            // Arrange
            var hotelId = "Test2";
            var appPublishDeviceSeries = new List<string>() { "TestDeviceSeries1", "TestDeviceSeries2", "TestDeviceSeries999" };
            var exceptedInsertList = new List<string>() { "TestDeviceSeries999", "TestDeviceSeries1" };
            var exceptedUpdateList = new List<string>() { "TestDeviceSeries2", };

            var traceRepertory = base.entityRepertory as DeviceTraceRepertory;

            // Act 
            var actual = traceRepertory.DeviceSeriesFilter(appPublishDeviceSeries, hotelId);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Item1.Count, exceptedInsertList.Count);
            Assert.AreEqual(actual.Item2.Count, exceptedUpdateList.Count);
        }

        private List<DeviceTrace> GetMockDeviceTraces(int count)
        {
            var traces = new List<DeviceTrace>();

            for (int i = 0; i < count; i++)
            {
                traces.Add(new DeviceTrace
                {
                    Active = true,
                    Brand = "TestBrand" + i,
                    DeviceKey = "TestDeviceKey" + i,
                    DeviceSeries = "TestDeviceSeries" + i,
                    FirstVisitTime = DateTime.Now,
                    GuestId = "TestGuestId" + i,
                    HotelId = "Test" + i,
                    Ip = "",
                    LastVisitTime = DateTime.Now,
                    Manufacturer = "TestManufacturer" + i,
                    Model = "TestModel" + i,
                    Platfrom = "TestPlatfrom" + i,
                    ModelId = 0,
                    OsVersion = "TestOsVersion" + i,
                    Remark = "",
                    RoomNo = "",
                    Token = "",
                    GroupId = "TestGroupId" + i
                });
            }

            return traces;
        }
    }
}
