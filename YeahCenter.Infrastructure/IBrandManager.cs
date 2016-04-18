using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahCenter.Infrastructure
{
    public interface IBrandManager
    {

        List<CoreSysBrand> GetAll();

        CoreSysBrand GetBrand(string brandId);

        //HotelObject GetBrandObject(string brandId);

        List<CoreSysBrand> GetBrandsByGroup(string groupId);
        string Add(CoreSysBrand entity);
        void Update(CoreSysBrand entity);
        void Delete(string brandId);
        List<CoreSysBrand> Search(CoreSysBrandCriteria coreSysBrandCriteria);
    }
}
