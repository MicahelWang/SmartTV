using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Service.Cache;

namespace YeahTVApiLibrary.UnitTests
{
    [TestClass]
    public class RedisCacheServiceTest
    {
        readonly CoreSysBrand _brand = new CoreSysBrand { GroupId = "brand_GroupId", Id = "brandId_Id", BrandName = "Name" };

        [TestMethod]
        public void AddRedisCache_ShouldAddCacheObject_WhenGiveObject()
        {
            // Arrange
            var service = new RedisCacheService();
            service.Remove("Brand");
            service.Add("Brand",_brand);

            // Act 
            var actual = service.Get("Brand");

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(_brand.ToJsonString(), actual.JsonStringToObj<CoreSysBrand>().ToJsonString());
        }

        [TestMethod]
        public void SetRedisCache_ShouldSetNewValue_WhenGiveNewObject()
        {
            // Arrange
            var service = new RedisCacheService();
            service.Remove("Brand");
            service.Add("Brand", _brand);
            var temp = _brand;
            temp.Id = "BrandId";
            var flag = service.Set("Brand", temp);

            // Act 
            var actual = service.Get<CoreSysBrand>("Brand");

            // Assert
            Assert.IsTrue(flag);
            Assert.AreEqual(temp.ToJsonString(), actual.ToJsonString());
        }

        [TestMethod]
        public void FlushDb_ShouldFlushed_WhenClearAllData()
        {
            // Arrange
            var service = new RedisCacheService();
            service.Clear();

            // Act 
            var actual = service.Get<CoreSysBrand>("Brand");

            // Assert
            Assert.IsNull(actual);
        }
    }
}
