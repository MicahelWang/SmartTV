using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahCenter.Infrastructure.WrapperFacadeInterface
{
    public interface IUerPermitionWrapperFacade
    {
        List<HotelPermition> HotelPermition(HotelPermitionCriteria hotelPermitionCriteria);

        [UnitOfWork]
        void UpdatePermintion(List<HotelPermition> listHotelPermition, ErpSysUser erpSysUser);

        void DeletePermintion(List<HotelPermition> listHotelPermition, ErpSysUser erpSysUser);
        [UnitOfWork]
        void AddPermintion(List<HotelPermition> listHotelPermition, ErpSysUser erpSysUser);
    }
}
