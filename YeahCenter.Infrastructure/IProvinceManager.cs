using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahCenter.Infrastructure
{
    public interface IProvinceManager
    {
        CoreSysProvince GetById(int id);
        List<CoreSysProvince> GetAll();
    }
}
