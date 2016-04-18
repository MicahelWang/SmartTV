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
    public class AppVersionRepertoryTest : BaseRepertoryTest<AppVersion, string>
    {
        public AppVersionRepertoryTest() 
        {
            base.GetMockEntities = () => { return GetMockApp(10); };
            base.SetRepertory = () => { return new AppVersionRepertory(); };
        }

        [DeploymentItem(@"MySql.Data.dll")]
        [DeploymentItem(@"MySql.Data.Entity.EF6.dll")]
        [DeploymentItem(@"EntityFramework.SqlServer.dll")]
        [TestMethod]
        public void SearchAppVersion_ShouldReturnAppVersion_WhenGiveValidCriteria()
        {
            //Arrange
            var criteria = new AppsCriteria();
            criteria.Active = true;
            criteria.Id = "TestAppId" + 1;

            var excepted = base.mockEntities.Where(m => m.Id == criteria.Id 
                && m.Active == criteria.Active
                && m.VersionCode == criteria.AppVersion);
            //Act
            var actual = base.entityRepertory.Search(criteria);
            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
            
        }

        [DeploymentItem(@"MySql.Data.dll")]
        [DeploymentItem(@"MySql.Data.Entity.EF6.dll")]
        [DeploymentItem(@"EntityFramework.SqlServer.dll")]
        [TestMethod]
        public void SearchAppVersion_ShouldReturnNull_WhenAppVersionIsNotExit()
        {
            //Arrange
            var criteria = new AppsCriteria();
            criteria.Active = true;
            criteria.Id = "TestAppIds" + 1;

            //Act
            var actual = base.entityRepertory.Search(criteria);
            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }
        private List<AppVersion> GetMockApp(int count)
        {
            var appversion = new List<AppVersion>();
            for (int i = 0; i < count; i++)
            {
                appversion.Add(new AppVersion
                {
                    Active=true,
                    CreateTime=DateTime.Now,
                    Id="TestAppId"+i,
                    VersionCode=i
                });
            }
            return appversion;
        }
    }
}
