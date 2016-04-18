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
    public class AppPublishRepertoryTest : BaseRepertoryTest<AppPublish, string>
    {

        public AppPublishRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockApps(10); };
            base.SetRepertory = () => { return new AppPublishRepertory(); };
        }

        [DeploymentItem(@"MySql.Data.dll")]
        [DeploymentItem(@"MySql.Data.Entity.EF6.dll")]
        [DeploymentItem(@"EntityFramework.SqlServer.dll")]
        [TestMethod]
        public void SearchAppPublish_ShouldReturnAppPublishs_WhenGiveValidCriteria()
        {
            // Arrange
            var criteria = new AppPublishCriteria();
            criteria.Active = true;
            criteria.Id = "TestAppId" + 1;

            var excepted = base.mockEntities.Where(m => m.Id == criteria.Id 
                && m.Active == criteria.Active
                && m.VersionCode == criteria.VersionCode);

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
        public void SearchAppPublish_ShouldReturnNull_WhenAppIsNotExit()
        {
            // Arrange
            var criteria = new AppPublishCriteria();
            criteria.Active = true;
            criteria.Id = "TestAppIdsss" + 1;
            // Act 
            var actual = base.entityRepertory.Search(criteria);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }
        
        private List<AppPublish> GetMockApps(int count)
        {
            var appPublishs = new List<AppPublish>();

            for (int i = 2; i < count; i++)
            {
                appPublishs.Add(new AppPublish
                {
                    Active = true,
                    CreateTime = DateTime.Now,
                    Id = "TestAppId" + i,
                    HotelId = "HotelId"+i,
                    PublishDate = DateTime.Now,
                    VersionCode = i,
                   // AppVersion = new AppVersion() { VersionCode=i, Id="TestAppId" + i, VersionName="name"+i, LastUpdater="admin", LastUpdateTime=DateTime.Now, CreateTime=DateTime.Now, Active=false},
                });
            }

            return appPublishs;
        }
    }
}
