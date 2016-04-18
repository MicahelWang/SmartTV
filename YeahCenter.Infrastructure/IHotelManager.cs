using System;
using System.Collections.Generic;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahCenter.Infrastructure
{
    public interface IHotelManager
    {
        void UpdateDataBase(string id, string baseData);
        string GetDataBase(string id);
        string Add(CoreSysHotel entity);
        [UnitOfWork]
        void Update(CoreSysHotel entity);
        HotelEntity GetHotel(string hotelId);

        HotelRoomEntity GetHotelByDeviceId(string deviceId);

        CoreSysHotel GetCoreSysHotelByname(string name);

        HotelObject GetHotelObject(string hotelId);
        CoreSysHotel GetCoreSysHotelById(string id);
    
        void BatchDelete(string[] ids);
        int Update(System.Linq.Expressions.Expression<Func<CoreSysHotelSencond, bool>> Predicate, System.Linq.Expressions.Expression<Func<CoreSysHotelSencond, CoreSysHotelSencond>> Updater);
        List<HotelEntity> GetByBrand(string brandId);

        List<CoreSysHotel> GetByGroup(string groupId);

        List<HotelEntity> GetAllHotels();
        List<CoreSysHotel> GetAllCoreSysHotels();
        int[] GetAllUseCity();
        IPagedList<CoreSysHotel> PagedList(int pageIndex, int pageSize, string keyword);
        List<CoreSysHotel> Search(CoreSysHotelCriteria criteria);
        int GetSameBrandHotelCount(string brandId);
        //void UpdateHotelData(string id, string hotelDevice);

        string GetHotelDeviceInfo(string id);

        //string UpdateHotelDeviceDetail(string hotelId, string hotelDeviceDetail);

        string GetHotelDeviceDetail(string id);
    } 
}
