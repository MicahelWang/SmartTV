using YeahTVApi.DomainModel;
using YeahTVApi.EntityFrameworkRepository.Models;
using YeahTVApi.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.EntityFrameworkRepository;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YeahTVApi.UnitTest.RepertoryTest
{
    [TestClass]
    public class AppsTest
    {
        [TestMethod]
        public void GetAllApps()
        {
            // Arrange
            var appsRepertory = new TVAppsRepertory();

            EFUnitOfWork.Current = new EFUnitOfWork(new YeahTVContext(Constant.NameOrConnectionString));
            // Act 
            var actual = appsRepertory.GetAll();

            // Assert
            Assert.IsNotNull(actual);
        }
    }
}
