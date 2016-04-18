using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;

namespace YeahTVApiLibrary.UnitTests.HZTVApiLibrary.ManagerTest
{
    [TestClass]
    public class SysAttachmentManagerTest
    {
        private Mock<ISysAttachmentRepertory> mockSysAttachmentRepertory;
        private SysAttachmentManager mockSysAttachmentManager;
        private int[] mockIdList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private List<CoreSysAttachment> mockCoreSysAttachmentlist;
        private Mock<IRedisCacheService> mockRedisCacheService;

        public SysAttachmentManagerTest()
        {

        }
        [TestInitialize]
        public void SetUp()
        {
            mockRedisCacheService = new Mock<IRedisCacheService>();
            mockSysAttachmentRepertory = new Mock<ISysAttachmentRepertory>();
            mockSysAttachmentManager = new SysAttachmentManager(mockSysAttachmentRepertory.Object, mockRedisCacheService.Object);
            mockCoreSysAttachmentlist = GetMockCoreSysAttachment(19);
        }

        [TestMethod]
        public void GetByIds_ShouldReturnCoreSysAttachmentList_WhenGiveIds()
        {
            // Arrange 
            var expertCoreSysAttachmentlist = mockCoreSysAttachmentlist.Where(n => mockIdList.Contains(n.Id)).ToList();
            mockSysAttachmentRepertory.Setup(m => m.GetByIds(It.IsAny<int[]>())).Returns<int[]>(id =>
                mockCoreSysAttachmentlist.Where(n => id.Contains(n.Id)).ToList()
                );

            mockRedisCacheService.Setup(m => m.ContainsKey(It.IsAny<string>())).Returns(true);
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<CoreSysAttachment>>>())).Returns(mockCoreSysAttachmentlist);

            // Act 
            var actual = mockSysAttachmentManager.GetByIds(mockIdList);

            // Assert
            Assert.IsNotNull(actual);
            actual.ForEach(a =>
            {
                Assert.IsNotNull(expertCoreSysAttachmentlist.FirstOrDefault(c => c.Id == a.Id));
            });
        }

        [TestMethod]
        public void GetByIds_ShouldReturnNull_WhenGiveIds()
        {
            // Arrange         
            var mockLists = new int[] { 34, 46, 47 };
            mockSysAttachmentRepertory.Setup(m => m.GetByIds(mockLists)).Returns<int[]>(
                id => mockCoreSysAttachmentlist.Where(n => id.Contains(n.Id)).ToList());

            mockRedisCacheService.Setup(m => m.ContainsKey(It.IsAny<string>())).Returns(true);
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<CoreSysAttachment>>>())).Returns(mockCoreSysAttachmentlist);

            // Act 
            var actual = mockSysAttachmentManager.GetByIds(mockLists);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count == 0);
        }

        [TestMethod]
        public void GetById_ShouldReturnCoreSysAttachmentList_WhenGiveId()
        {
            // Arrange 
            var id = 1;
            var expertCoreSys = mockCoreSysAttachmentlist.Where(n => n.Id == id).FirstOrDefault();
            mockSysAttachmentRepertory.Setup(m => m.FindByKey(It.IsAny<int>())).Returns<int>((i) => mockCoreSysAttachmentlist.Where(n => n.Id == i).FirstOrDefault());

            mockRedisCacheService.Setup(m => m.ContainsKey(It.IsAny<string>())).Returns(true);
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<CoreSysAttachment>>>())).Returns(() => mockCoreSysAttachmentlist);

            // Act 
            var actual = mockSysAttachmentManager.GetById(id);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expertCoreSys, actual);
        }

        [TestMethod]
        public void GetById_ShouldReturnNull_WhenGiveNotExistId()
        {
            // Arrange         
            var id = 100;
            mockSysAttachmentRepertory.Setup(m => m.FindByKey(It.IsAny<int>())).Returns<int>(
                (key) => mockCoreSysAttachmentlist.FirstOrDefault(n => n.Id == key));

            mockRedisCacheService.Setup(m => m.ContainsKey(It.IsAny<string>())).Returns(true);
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<CoreSysAttachment>>>())).Returns(() => mockCoreSysAttachmentlist);

            // Act 
            var actual = mockSysAttachmentManager.GetById(id);

            //Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void Delete_ShouldReturnCoreSysAttachment_WhenGiveId()
        {
            // Arrange 
            var id = 1;

            mockSysAttachmentRepertory.Setup(m => m.Delete(It.IsAny<System.Linq.Expressions.Expression<Func<CoreSysAttachment, bool>>>())).Callback<
                System.Linq.Expressions.Expression<Func<CoreSysAttachment, bool>>>(obj =>
                    {
                        var fun = obj.Compile();
                        mockCoreSysAttachmentlist.Where(fun).ToList().
                            ForEach(m => mockCoreSysAttachmentlist.Remove(m));
                    }
                );
            mockRedisCacheService.Setup(m => m.ContainsKey(It.IsAny<string>())).Returns(true);
            mockRedisCacheService.Setup(m => m.GetAllFromCache(It.IsAny<string>(), It.IsAny<Func<List<CoreSysAttachment>>>())).Returns(() => mockCoreSysAttachmentlist);
            mockRedisCacheService.Setup(m => m.RemoveItemFromSet(It.IsAny<string>(), It.IsAny<object>()));

            // act 
            mockSysAttachmentManager.Delete(id);

            // Assert 
            Assert.IsTrue(mockCoreSysAttachmentlist.Where(obj => obj.Id == id).Count() == 0);
        }

        [TestMethod]
        public void Delete_ShouldNoItemRemove_WhenGiveNotExistId()
        {
            // Arrange         
            var expectedList = mockCoreSysAttachmentlist.Count;

            var id = -1;
            mockSysAttachmentRepertory.Setup(m => m.Delete(It.IsAny<System.Linq.Expressions.Expression<Func<CoreSysAttachment, bool>>>())).Callback<System.Linq.Expressions.Expression<Func<CoreSysAttachment, bool>>>(obj =>
                   {
                       var fun = obj.Compile();
                       mockCoreSysAttachmentlist.Where(fun).ToList().
                           ForEach(m => mockCoreSysAttachmentlist.Remove(m));
                   });

            mockRedisCacheService.Setup(m => m.ContainsKey(It.IsAny<string>())).Returns(true);
            mockRedisCacheService.Setup(m => m.GetAllFromCache<CoreSysAttachment>(It.IsAny<string>(), It.IsAny<Func<List<CoreSysAttachment>>>())).Returns(() => mockCoreSysAttachmentlist);
            mockRedisCacheService.Setup(m => m.RemoveItemFromSet(It.IsAny<string>(), It.IsAny<object>()));

            // act 
            mockSysAttachmentManager.Delete(id);

            // Assert 
            Assert.AreEqual(expectedList, mockCoreSysAttachmentlist.Count);
        }

        [TestMethod]
        public void Add_ShouldReturnKey_WhenGiveCoreSysAttachment()
        {
            // Arrange 
            var mockCoreSysAttachment = new CoreSysAttachment
            {

                FileName = "FileName",
                FileType = "FileType",
                FilePath = "FilePath",
                Unit = "Unit",
                CrateTime = DateTime.Now,
                FileSize = 10,
                Id = 100
            };
            mockSysAttachmentRepertory.Setup(m => m.Insert(It.IsAny<CoreSysAttachment>())).Callback<CoreSysAttachment>(obj => mockCoreSysAttachmentlist.Add(obj));
            mockRedisCacheService.Setup(
                m =>
                    m.AddItemToSet(It.IsAny<string>(), It.IsAny<CoreSysAttachment>(),
                        It.IsAny<Func<List<CoreSysAttachment>>>()));
            mockSysAttachmentRepertory.Setup(m => m.GetByIds(It.IsAny<int[]>())).Returns(new List<CoreSysAttachment>());
            mockSysAttachmentRepertory.Setup(m => m.GetAll());

            // Act 
            var actual = mockSysAttachmentManager.Add(mockCoreSysAttachment);

            // Assert 
            Assert.IsNotNull(mockCoreSysAttachmentlist.FirstOrDefault(obj => obj.Id == mockCoreSysAttachment.Id));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_ShouldReturnException_WhenGiveNullCoreSysAttachment()
        {
            // Arrange           
            CoreSysAttachment obj = null;
            mockSysAttachmentRepertory.Setup(m => m.Insert(It.IsAny<CoreSysAttachment>())).Throws(new ArgumentNullException());

            // Act 
            var actual = mockSysAttachmentManager.Add(obj);
        }

        private List<CoreSysAttachment> GetMockCoreSysAttachment(int count)
        {
            var configs = new List<CoreSysAttachment>();

            for (int i = 0; i < count; i++)
            {
                configs.Add(new CoreSysAttachment
                {
                    FileName = "FileName" + i,
                    FileType = "FileType" + i,
                    FilePath = "FilePath" + i,
                    Unit = "Unit" + i,
                    CrateTime = DateTime.Now,
                    FileSize = i * 10,
                    Id = i
                });
            }

            return configs;
        }
    }
}
