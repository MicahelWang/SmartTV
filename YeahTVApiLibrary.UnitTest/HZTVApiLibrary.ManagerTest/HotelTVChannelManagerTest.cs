using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApi.Manager;
using YeahTVApiLibrary.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.DomainModel.Mapping;
using YeahTVLibrary.Manager;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApiLibrary.Manager;

namespace YeahTVApiLibrary.UnitTests.HZTVApiLibrary.ManagerTest
{
    [TestClass]
    public class HotelTVChannelManagerTest
    {
        private Mock<IHotelTVChannelRepertory> mockHotelTVChannelRepertory;
        private Mock<ITVChannelManager> mockTVChannelManager;
        private Mock<ISysAttachmentRepertory> mockSysAttachmentRepertory;
        private Mock<ITVHotelConfigRepertory> mockTVHotelConfigRepertory;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private Mock<IRedisCacheService> mockRedisCacheService;
        private IHotelTVChannelManager hotelTVChannelManager;
        private List<HotelTVChannel> mockChannel;
        [TestInitialize]
        public void Setup()
        {
            mockChannel = GetMockChannel(100);
            mockHotelTVChannelRepertory = new Mock<IHotelTVChannelRepertory>();
            mockTVChannelManager = new Mock<ITVChannelManager>();
            mockSysAttachmentRepertory = new Mock<ISysAttachmentRepertory>();
            mockTVHotelConfigRepertory = new Mock<ITVHotelConfigRepertory>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            mockRedisCacheService =new Mock<IRedisCacheService>();
            hotelTVChannelManager = new HotelTVChannelManager(mockHotelTVChannelRepertory.Object,
                mockTVChannelManager.Object,
                mockSysAttachmentRepertory.Object,
                mockTVHotelConfigRepertory.Object,
                mockConstantSystemConfigManager.Object,mockRedisCacheService.Object);
        }

        //[TestMethod]
     
        //public void SearchHotelTVChannels_ShouldreturnHotelTVChannels_WhenGiveExitRequestHeader()
        //{
        //    //Arrange
        //    var mockHeader = new RequestHeader
        //    {
        //        HotelID = "TestHotelId",
        //    };

        //    var excepted = mockChannel.Where(m => m.HotelId.Contains(mockHeader.HotelID)).ToList();
        //    mockHotelTVChannelRepertory.Setup(m => m.Search(It.IsAny<HotelTVChannelCriteria>())).Returns(mockChannel);

        //    //Act
        //    var actual = hotelTVChannelManager.SearchHotelTVChannels(mockHeader);

        //    //Assert
        //    Assert.IsNotNull(actual);
        //    Assert.AreEqual(excepted.Count, actual.Count);
        //}
  
        private List<HotelTVChannel> GetMockChannel(int count)
        {
            var Channel = new List<HotelTVChannel>();

            for (int i = 0; i < count; i++)
            {
                Channel.Add(
                new HotelTVChannel
                {
                    HotelId = "TestHotelId" 
                   
                });
            };

            return Channel;
        }
    }

}
