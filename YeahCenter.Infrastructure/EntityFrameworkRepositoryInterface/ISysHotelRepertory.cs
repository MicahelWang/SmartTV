using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCenter.Infrastructure
{
    public interface ISysHotelRepertory : IBsaeRepertory<CoreSysHotel>
    {
        HotelEntity GetHotelEntity(string hotelId);

        List<HotelEntity> GetHotelEntityByBrand(string brandId);

        List<HotelEntity> GetHotelEntity();
        int GetSameBrandHotelCount(string brandId);
    }
}
