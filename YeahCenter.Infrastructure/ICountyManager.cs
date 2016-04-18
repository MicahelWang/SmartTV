using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahCenter.Infrastructure
{
    public interface ICountyManager
    {
        CoreSysCounty GetById(int id);
        List<CoreSysCounty> GetCountysByParentId(int parentId);
        List<CoreSysCounty> GetAll();
    }
}
