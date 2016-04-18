using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Common;
using YeahCenter.Infrastructure.WrapperFacadeInterface;
using YeahCenter.Infrastructure;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.WrapperFacade
{
    public class UerPermitionWrapperFacade : IUerPermitionWrapperFacade
    {
        private IHotelPermitionManager hotelPermitionManager;
        private IUserManager userManager;

        public UerPermitionWrapperFacade(IHotelPermitionManager hotelPermitionManager, IUserManager userManager)
        {
            this.hotelPermitionManager = hotelPermitionManager;
            this.userManager = userManager;
        }

        public List<HotelPermition> HotelPermition(HotelPermitionCriteria hotelPermitionCriteria)
        {
            throw new NotImplementedException();
        }
         [UnitOfWork]
        public void UpdatePermintion(List< HotelPermition> listHotelPermition, ErpSysUser erpSysUser)
        {
            userManager.Update(erpSysUser);
            hotelPermitionManager.DelelteByUserId(erpSysUser.Id);
            hotelPermitionManager.InsertAll(listHotelPermition);
        }

        public void DeletePermintion(List< HotelPermition> listHotelPermition, ErpSysUser erpSysUser)
        {
            throw new NotImplementedException();
        }
        [UnitOfWork]
        public void AddPermintion(List< HotelPermition> listHotelPermition, ErpSysUser erpSysUser)
        {
            userManager.Add(erpSysUser);
            hotelPermitionManager.InsertAll(listHotelPermition);
        }
    }
}
