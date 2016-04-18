using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure.ManagerInterface
{
   public interface IHCSCacheVersionManager:IBaseManager<HCSCacheVersion,HCSCacheVersionCriteria >
    {
       HCSCacheVersion EditVersion(HCSCacheVersionCriteria hCSCacheVersionCriteria, string updateUser);
    }
}
