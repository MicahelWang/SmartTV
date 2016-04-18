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

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.EntityFrameworkRepositoryTest
{
    [TestClass]
    public class AppsRepertoryTest : BaseRepertoryTest<Apps,string>
    {

        public AppsRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockApps(10); };
            base.SetRepertory = () => { return new AppsRepertory(); };
        }

        [DeploymentItem(@"MySql.Data.dll")]
        [DeploymentItem(@"MySql.Data.Entity.EF6.dll")]
        [DeploymentItem(@"EntityFramework.SqlServer.dll")]
        [TestMethod]
        public void SearchApps_ShouldReturnApps_WhenGiveValidCriteria()
        {
            // Arrange
            var criteria = new AppsCriteria();
            criteria.Active = true;
            criteria.Id = "TestAppId" + 1;
            criteria.ShowInStroe = false;

            var excepted = base.mockEntities.Where(m => m.Id == criteria.Id 
                && m.Active == criteria.Active 
                && m.ShowInStroe == criteria.ShowInStroe);

            // Act 
            var actual = base.entityRepertory.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(excepted.Count(), actual.Count);
        }

        [DeploymentItem(@"MySql.Data.dll")]
        [DeploymentItem(@"MySql.Data.Entity.EF6.dll")]
        [DeploymentItem(@"EntityFramework.SqlServer.dll")]
        [TestMethod]
        public void SearchApps_ShouldReturnNull_WhenAppIsNotExit()
        {
            var criteria = new AppsCriteria();
            criteria.Active = true;
            criteria.Id = "TestAppIdsfsfs" + 1;
            criteria.ShowInStroe = false;

            // Act 
            var actual = base.entityRepertory.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        private List<Apps> GetMockApps(int count)
        {
            var apps = new List<Apps>();

            for (int i = 0; i < count; i++)
            {
                apps.Add(new Apps
                {
                    
                    Active = true,
                    AppKey = "TestAppKey" +i,
                    CreateTime = DateTime.Now,
                    Id = "TestAppId" + i,
                    ShowInStroe = false,
                    SecureKey = "SecureKey" + i,
                    Name = "TestName" + i,
                    Platfrom = "TestPlatfrom" +i,
                    PackageName = "TestPackageName"
                });
            }

            return apps;
        }
    }
}
