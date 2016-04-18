using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahCentre.EntityFrameworkRepository.Repertory;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.EntityFrameworkRepositoryTest;

namespace YeahCentre.UnitTest.EntityFrameworkRepositoryTest
{
    [TestClass]
    public class GroupRepertoryTest : BaseRepertoryTest<CoreSysGroup, string>
    {
        public GroupRepertoryTest()
        {
            base.GetMockEntities = () => { return GetMockApps(10); };
            base.SetRepertory = () => { return new SysGroupRepertory(); };
        }
        

        [TestMethod]
        private List<CoreSysGroup> GetMockApps(int count)
        {
            var apps = new List<CoreSysGroup>();

            for (var i = 0; i < count; i++)
            {
                var item = new CoreSysGroup
                {
                    Id = "GroupId_" + i,
                    GroupCode = "GroupCode_" + i,
                    GroupName = "GroupName_" + i,
                    TemplateId = 0,
                    IsDelete = false
                };
                apps.Add(item);
            }

            return apps;
        }
    }
}
