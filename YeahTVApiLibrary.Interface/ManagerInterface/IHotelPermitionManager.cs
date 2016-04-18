using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahCenter.Infrastructure
{
    public interface IHotelPermitionManager
    {
        [UnitOfWork]
        void InsertAll(List<HotelPermition> listOjb);
        void DelelteByUserId(string userId);
        List<HotelPermition> Search(HotelPermitionCriteria searchCriteria);
        List<HotelPermition> GetHotelByUserId(string userId);
        List<HotelPermition> GetAllFromCache();
        List<CoreSysHotel> GetHotelListByPermition(string uid);

        Dictionary<string,string> GetBrandConfigUrlsByUserId(string userId);
    }
}
