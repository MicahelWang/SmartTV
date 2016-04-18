using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahCentre.EntityFrameworkRepository.Repertory;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.EntityFrameworkRepositoryTest;
using System;
using YeahTVApi.DomainModel.SearchCriteria;
using System.Linq;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.UnitTest.EntityFrameworkRepositoryTest
{
    [TestClass]
    public class TagRepertoryTest : YeahCentreBaseRepertoryTest<Tag, int>
    {
        public TagRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockApps(10); };
            base.SetRepertory = () => { return new TagRepertory(); };
        }

        [TestMethod]
        public void SearchSysHotel_ShouldReturnSysHotels_WhenGiveValidCriteria()
        {
            //Arrange
            var criteria = new TagCriteria()
            {
                
            };
            (base.entityRepertory as TagRepertory).SearchALLTagsWithRescource();

            //Act

            //Assert 
        }

        [TestMethod]
        private List<Tag> GetMockApps(int count)
        {
            var apps = new List<Tag>();
            string tempGuid;

            for (var i = 0; i < count; i++)
            {
                tempGuid = Guid.NewGuid().ToString("N");
                var item = new Tag
                { 
                    Id=i,
                    RescorceId=""
               
                };
                apps.Add(item);
            }

            return apps;
        }
         
    }
}
