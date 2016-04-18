using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApiLibrary.Infrastructure;
using Moq;
using YeahTVApiLibrary.Manager;
using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using System.Linq.Expressions;

namespace YeahTVApiLibrary.UnitTests.HZTVApiLibrary.ManagerTest
{
    [TestClass]
    public class TVChannelManagerTest
    {
        ITVChannelManager tvchannel;
        private Mock<ITVChannelRepertory> mockTVChannelRepertory;
        private Mock<IRedisCacheService> mockRedisCacheService;
        List<TVChannel> mockTVChannels;
        int count = 5;
        List<TVChannel> mockAddChannels;
        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void InitialMethod()
        {
            mockTVChannelRepertory = new Mock<ITVChannelRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();
            tvchannel = new TVChannelManager(mockTVChannelRepertory.Object, mockRedisCacheService.Object);
            mockAddChannels = GetChannelList(count);

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void SearchTVChannels_ShouldPullTVChanelList_WhenGiveCriteria()
        {

            // Arrange
            var mockCategory = "Test";
            var mockName = "TestMessageInfo";

            var mockTVchanel = new List<TVChannel>
            {
                new TVChannel
                {
                    Category=mockCategory,
                    Id="1",
                    Name=mockName
                }
            };

            mockTVChannelRepertory.Setup(m => m.Search(It.IsAny<TVChannelCriteria>())).Returns(mockTVchanel);

            // Act 
            var actual = tvchannel.SearchTVChannels(new TVChannelCriteria());

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Find(m => m.Name.Equals(mockName)) != null);

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void AddTVChannels_ShouldInsertTVChannelsModels_WhenGiveListTVChannel()
        {
            //Arrange
            mockTVChannels = new List<TVChannel>();
            mockTVChannelRepertory.Setup(m => m.Insert(It.IsAny<IEnumerable<TVChannel>>()))
                .Callback<IEnumerable<TVChannel>>((b) =>
                {
                    mockTVChannels.AddRange(b);
                });

            mockTVChannelRepertory.Setup(m => m.GetAll()).Returns(new List<TVChannel>());
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<TVChannel>(), It.IsAny<Func<List<TVChannel>>>()));
            mockTVChannelRepertory.Setup(m => m.Search(It.IsAny<TVChannelCriteria>())).Returns(mockAddChannels);

            //Act
            tvchannel.AddTVChannels(mockAddChannels);
            var actual = mockTVChannels.FindAll(m => m.Category.Contains("categoryStr"));
            //Assert

            Assert.IsNotNull(actual);
            Assert.AreEqual(5, actual.Count);
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void AddTVChannels_ShouldInsertSingleChannel_WhenGiveOneChannel()
        {
            string cat = "Test";
            //Arrange
            var mockTVSingleChannelList = new List<TVChannel>();
            TVChannel tvSingel = new TVChannel()
                        {
                            Category = cat,
                            CategoryEn = "TestEx",
                            Name = "Name",
                            DefaultCode = "DefualtCode",
                            Icon = "",
                            Id = "1",
                            NameEn = "NameEn",
                            LastUpdateTime = DateTime.Now,
                            IconPath = ""
                        };
            mockTVChannelRepertory.Setup(m => m.Insert(It.IsAny<TVChannel>()))
                .Callback<TVChannel>((b) =>
                {
                    mockTVSingleChannelList.Add(b);
                });

            mockTVChannelRepertory.Setup(m => m.GetAll()).Returns(new List<TVChannel>());
            mockRedisCacheService.Setup(m => m.AddItemToSet(It.IsAny<string>(), It.IsAny<TVChannel>(), It.IsAny<Func<List<TVChannel>>>()));
            mockTVChannelRepertory.Setup(m => m.Search(It.IsAny<TVChannelCriteria>())).Returns(new List<TVChannel> { tvSingel });

            //Act
            tvchannel.AddTVChannels(tvSingel);

            //Assert
            Assert.IsNotNull(mockTVSingleChannelList);
            Assert.IsTrue(mockTVSingleChannelList.Find(m => m.Category.Equals(cat)) != null);
        }
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<TVChannel> GetChannelList(int count)
        {
            List<TVChannel> channelList = new List<TVChannel>();
            for (int i = 0; i < count; i++)
            {
                channelList.Add(new TVChannel()
                 {
                     Category = "categoryStr" + i,
                     CategoryEn = "categoryEn" + i,
                     DefaultCode = "defualtCode" + i,
                     Id = i.ToString(),
                     Icon = "",
                     IconPath = "",
                     LastUpdateTime = DateTime.Now,
                     Name = "ChannelName",
                     NameEn = "channelEn"

                 });
            }
            return channelList;
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UpdateTVChannel_ShouldSuccess_WhenUpdateTVChannel()
        {
            // Arrange
            var mockTVChannel = new TVChannel
            {
                Category = "Category",
                DefaultCode = "DefaultCode",
                CategoryEn = "CategoryEn",
                Icon = "",
                IconPath = "",
                Id = "1",
                LastUpdateTime = DateTime.Now,
                Name = "Name",
                NameEn = "NameEn"
            };

            mockTVChannelRepertory.Setup(m => m.Update(It.IsAny<TVChannel>())).Callback<TVChannel>((b) =>
            {
                mockAddChannels.SingleOrDefault(m => m.Category == "categoryStr1").Category = mockTVChannel.Category;
                mockAddChannels.SingleOrDefault(m => m.CategoryEn == "categoryEn1").CategoryEn = mockTVChannel.CategoryEn;
                mockAddChannels.SingleOrDefault(m => m.DefaultCode == "defualtCode1").DefaultCode = mockTVChannel.DefaultCode;
            });
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<TVChannel>>>())).Returns(new List<TVChannel>() { mockTVChannel});
            mockTVChannelRepertory.Setup(m => m.Search(It.IsAny<TVChannelCriteria>())).Returns(new List<TVChannel>() { mockTVChannel });

            // Act 
            tvchannel.Update(mockTVChannel);

            // Assert

            Assert.IsTrue(mockAddChannels.Any(m => m.Category == mockTVChannel.Category));
            Assert.IsTrue(mockAddChannels.Any(m => m.CategoryEn == mockTVChannel.CategoryEn));
            Assert.IsTrue(mockAddChannels.Any(m => m.DefaultCode == mockTVChannel.DefaultCode));
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void DeleteTVChannel_ShouldSuccess_WhenDeleteTVChannel()
        {
            // Arrange
            var mockTVChannel = new TVChannel
            {
                Category = "Category",
                DefaultCode = "DefaultCode",
                CategoryEn = "CategoryEn",
                Icon = "",
                IconPath = "",
                Id = "1",
                LastUpdateTime = DateTime.Now,
                Name = "Name",
                NameEn = "NameEn"
            };

            mockTVChannelRepertory.Setup(m => m.Delete(It.IsAny<Expression<Func<TVChannel, bool>>>()))
                .Callback<Expression<Func<TVChannel, bool>>>((b) =>
            {
                var fun = b.Compile();
                mockAddChannels.Where(fun).ToList().ForEach(item =>
                {
                    mockAddChannels.Remove(item);
                });
            });
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<TVChannel>>>())).Returns(new List<TVChannel>() { mockTVChannel });
            
            // Act 
            tvchannel.Delete(mockAddChannels.FirstOrDefault());
            // Assert
            Assert.IsNull(mockAddChannels.Find(m => m.Category == "categoryStr0"));
        }


    }
}