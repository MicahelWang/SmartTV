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
using YeahTVApi.Infrastructure.RepositoriesInterface.EntityFrameworkRepositoryInterface.IRepertory.YeahCentre;

namespace YeahCentre.UnitTest.ManagerTest
{
    [TestClass]
    public class TvTemplateTypeManagerTest
    {

        private Mock<ITvTemplateTypeRepertory> mockTypeRepertory;
        private Mock<ITvTemplateElementRepertory> mockElementRepertory;
        private Mock<ITvTemplateAttributeRepertory> mockAttributeRepertory;

        private Mock<IRedisCacheService> mockRedisCacheService;

        private ITvTemplateTypeManager tvTemplateTypeManager;
        private List<TvTemplateType> mockTvTemplateTypes;

        [TestInitialize]
        public void Setup()
        {
            mockTvTemplateTypes = GetMockTvTemplateTypes(10);
            mockTypeRepertory = new Mock<ITvTemplateTypeRepertory>();
            mockElementRepertory = new Mock<ITvTemplateElementRepertory>();
            mockAttributeRepertory = new Mock<ITvTemplateAttributeRepertory>();
            mockRedisCacheService = new Mock<IRedisCacheService>();

            tvTemplateTypeManager = new TvTemplateTypeManager(
                mockTypeRepertory.Object,
                mockRedisCacheService.Object,
                mockElementRepertory.Object,
                mockAttributeRepertory.Object);
        }


        [TestMethod]
        public void AddWithBaseNode_ShouldAddTvTemplateType_WhenGiveValidTvTemplateType()
        {
            //Arrange
            var expectedId = 99;
            var expectedTvTemplateType = new TvTemplateType
            {
                Id = expectedId,
                Name = "TestAddTvTemplateType",
                Description = "TestAddDescription"
            };

            List<TvTemplateElement> elementList = new List<TvTemplateElement>();
            List<TvTemplateAttribute> attributeList = new List<TvTemplateAttribute>();
            var actual = new List<CoreSysHotel>();

            mockTypeRepertory.Setup(m => m.Insert(It.IsAny<TvTemplateType>())).Callback((TvTemplateType entity) =>
            {
                mockTvTemplateTypes.Add(entity);
            });

            mockElementRepertory.Setup(m => m.Insert(It.IsAny<TvTemplateElement>())).Callback((TvTemplateElement entity) =>
            {
                elementList.Add(entity);
            });
            mockAttributeRepertory.Setup(m => m.Insert(It.IsAny<IEnumerable<TvTemplateAttribute>>())).Callback<IEnumerable<TvTemplateAttribute>>((entity) =>
            {
                attributeList.AddRange(entity);
            });

            mockTypeRepertory.Setup(m => m.GetAll()).Returns(mockTvTemplateTypes);
            mockRedisCacheService.Setup(m => m.Remove(It.IsAny<string>())).Callback(() => { });
            mockRedisCacheService.Setup(m => m.Set(It.IsAny<string>(), It.IsAny<object>())).Callback(() => { });
            mockRedisCacheService.Setup(m => m.Add(It.IsAny<string>(), It.IsAny<object>())).Callback(() => { });
            mockRedisCacheService.Setup(m => m.IsSet(It.IsAny<string>())).Returns(false);

            //Act
            var actTvTemplateTypelId = tvTemplateTypeManager.AddWithBaseNode(expectedTvTemplateType);

            //Assert
            Assert.AreEqual(expectedId, actTvTemplateTypelId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        public void AddWithBaseNode_ShouldThrowArgumentException_WhenGiveNullTvTemplateType()
        {
            //Arrange

            List<TvTemplateElement> elementList = new List<TvTemplateElement>();
            List<TvTemplateAttribute> attributeList = new List<TvTemplateAttribute>();
            var actual = new List<CoreSysHotel>();

            mockTypeRepertory.Setup(m => m.Insert(It.IsAny<TvTemplateType>())).Throws(new ArgumentException("null"));

            mockElementRepertory.Setup(m => m.Insert(It.IsAny<TvTemplateElement>())).Callback((TvTemplateElement entity) =>
            {
                elementList.Add(entity);
            });
            mockAttributeRepertory.Setup(m => m.Insert(It.IsAny<IEnumerable<TvTemplateAttribute>>())).Callback<IEnumerable<TvTemplateAttribute>>((entity) =>
            {
                attributeList.AddRange(entity);
            });

            mockTypeRepertory.Setup(m => m.GetAll()).Returns(mockTvTemplateTypes);
            mockRedisCacheService.Setup(m => m.Remove(It.IsAny<string>())).Callback(() => { });
            mockRedisCacheService.Setup(m => m.Set(It.IsAny<string>(), It.IsAny<object>())).Callback(() => { });
            mockRedisCacheService.Setup(m => m.Add(It.IsAny<string>(), It.IsAny<object>())).Callback(() => { });
            mockRedisCacheService.Setup(m => m.IsSet(It.IsAny<string>())).Returns(false);

            //Act
            var actTvTemplateTypelId = tvTemplateTypeManager.AddWithBaseNode(null);
        }


        private List<TvTemplateType> GetMockTvTemplateTypes(int count)
        {
            var templateTypes = new List<TvTemplateType>();

            for (int i = 0; i < count; i++)
            {
                templateTypes.Add(new TvTemplateType
                {
                    Id = i,
                    Description = "TestDescription" + i,
                    Name = "TestName" + i
                });
            }

            return templateTypes;
        }
    }
}
