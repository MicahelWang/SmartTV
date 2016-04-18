using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;

namespace YeahTVApiLibrary.Manager
{
    public class HCSCacheVersionManager : BaseManager<HCSCacheVersion, HCSCacheVersionCriteria>, IHCSCacheVersionManager
    {
        private IHCSCacheVersionRepertory iHCSCacheVersionRepertory;
        public HCSCacheVersionManager(IHCSCacheVersionRepertory iHCSCacheVersionRepertory)
            : base(iHCSCacheVersionRepertory)
        {
            this.iHCSCacheVersionRepertory = iHCSCacheVersionRepertory;
        }
        public HCSCacheVersion EditVersion(HCSCacheVersionCriteria hCSCacheVersionCriteria, string updateUser)
        {
            var version = iHCSCacheVersionRepertory.Search(hCSCacheVersionCriteria).FirstOrDefault();
            if (version == null)
            {
                version = new HCSCacheVersion()
                {
                    Version = 2,
                    LastUpdateTime = DateTime.Now,
                    LastUpdateUser = updateUser,
                    PermitionType = hCSCacheVersionCriteria.PermitionType,
                    TypeId = hCSCacheVersionCriteria.TypeId
                };
                iHCSCacheVersionRepertory.Insert(version);
            }
            else
            {
                version.Version++;
                version.LastUpdateTime = DateTime.Now;
                version.LastUpdateUser = updateUser;
                iHCSCacheVersionRepertory.Update(version);
            }
            return version;
        }
    }
}
