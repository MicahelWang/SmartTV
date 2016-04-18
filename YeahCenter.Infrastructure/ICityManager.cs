using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahCenter.Infrastructure
{
    public interface ICityManager
    {
        List<CoreSysCity> GetByIds(int[] idArray);
        List<CoreSysCity> GetAll();

        CoreSysCity GetById(int id);
        List<CoreSysCity> GetCitysByParentId(int parentId);
    }
}
