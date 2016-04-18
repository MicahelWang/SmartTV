using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahCentre.EntityFrameworkRepository.Repertory;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.EntityFrameworkRepositoryTest;
using System;
using YeahTVApi.DomainModel.SearchCriteria;
using System.Linq;

namespace YeahCentre.UnitTest.EntityFrameworkRepositoryTest
{
    [TestClass]
    public class SysHotelRepertoryTest : YeahCentreBaseRepertoryTest<CoreSysHotel, string>
    {
        public SysHotelRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockApps(10); };
            base.SetRepertory = () => { return new SysHotelRepertory(); };
        }

        [TestMethod]
        public void SearchSysHotel_ShouldReturnSysHotels_WhenGiveValidCriteria()
        {
            //Arrange
            var criteria = new CoreSysHotelCriteria()
            {
                Id = "TestId" + 2,
                HotelName = "TestHotelName" + 2
            };

            var excepted = base.mockEntities.Where(m => m.Id == criteria.Id && m.HotelName.Contains(criteria.HotelName)).ToList().FirstOrDefault();

            //Act
            var actual = base.entityRepertory.Search(criteria).FirstOrDefault();

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreSame(excepted, actual);
        }
        
        [TestMethod]
        public void SearchSysHotel_ShouldReturnNull_WhenHotelIsNotExist()
        {
            //Arrange
            var criteria = new CoreSysHotelCriteria()
            {
                Id = "TestId" + 2,
                TemplateId = "noExist"
            };

            //Act
            var actual = base.entityRepertory.Search(criteria);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNull(actual.FirstOrDefault());
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void GetSameBrandHotelCount_ShouldReturnCount_WhenGiveExistBrandId()
        {
            //Arrange
            var exceptedBrandId = "TestBrandId1";
            var exceptedCount = base.mockEntities.Where(m => m.BrandId == exceptedBrandId).Count();

            //Act
            var actual = ((SysHotelRepertory)base.entityRepertory).GetSameBrandHotelCount(exceptedBrandId);

            //Assert
            Assert.AreEqual(exceptedCount, actual);
        }

        [TestMethod]
        public void GetSameBrandHotelCount_ShouldReturnZero_WhenGiveNotExistBrandId()
        {
            //Arrange
            var exceptedBrandId = "TestBrandId1111";

            //Act
            var actual = ((SysHotelRepertory)base.entityRepertory).GetSameBrandHotelCount(exceptedBrandId);

            //Assert
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        private List<CoreSysHotel> GetMockApps(int count)
        {
            var apps = new List<CoreSysHotel>();
            string tempGuid;

            for (var i = 0; i < count; i++)
            {
                tempGuid = Guid.NewGuid().ToString("N");
                var item = new CoreSysHotel
                {
                    Address = "TestAddress" + i,
                    BrandId = "TestBrandId" + i,
                    City = i,
                    Country = i,
                    GroupId = "TestGroupId" + i,
                    HotelCode = "TestHotelCode" + i,
                    HotelName = "TestHotelName" + i,
                    HotelNameEn = "HotelNameEn" + i,
                    Id = "TestId" + i,
                    IsLocalPMS = false,
                    Latitude = 0,
                    Longitude = 0,
                    Province = i,
                    Tel = "TestTel" + i,
                    TemplateId = "TestTemplateId" + tempGuid,
                    IsDelete = false,
                    CoreSysHotelSencond = new CoreSysHotelSencond()
                    {
                        Id = "TestId" + i,
                        WelcomeWord = "WelcomeWord" + i,
                        LaunchBackground = "LaunchBackground" + i,
                        LogoImageUrl="http://test"
                    },
                    TvTemplate = new TvTemplate()
                    {
                        Id = "TestTemplateId" + tempGuid,
                        Name = "TestTvTemplate" + i,
                    },
                    CoreSysBrand = new CoreSysBrand()
                    {
                        Id = "TestBrandId" + tempGuid,
                        BrandName = "TestBrandName" + i,
                        GroupId="TestCoreSysGroupId"+tempGuid,
                        CoreSysGroup = new CoreSysGroup() {
                            Id = "TestCoreSysGroupId"+tempGuid,
                            GroupName = "TestGroupName"+i
                        }
                    }
                };
                apps.Add(item);
            }

            return apps;
        }
    }
}
