using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class HotelManager : IHotelManager
    {
        private readonly ISysHotelRepertory _hotelRepertory;
        private readonly ISysHotelSencondRepertory _hotelSencondRepertory;
        private readonly ISysBrandRepertory _brandRepertory;
        private readonly ISysGroupRepertory _groupRepertory;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IHotelMovieTraceNoTemplateWrapperFacade _hotelMovieTraceNoTemplateWrapperFacade;
        private readonly IDeviceTraceLibraryManager _deviceTraceLibraryManager;
        private readonly IAppLibraryManager _appManager;

        #region Redis

        private IEnumerable<HotelEntity> Hotels()
        {

            if (!_redisCacheService.IsSet(RedisKey.HoteleKey))
            {
                _redisCacheService.Add(RedisKey.HoteleKey, _hotelRepertory.GetHotelEntity());
            }
            return _redisCacheService.Get<List<HotelEntity>>(RedisKey.HoteleKey);

        }

        private IEnumerable<CoreSysHotel> HotelEntities()
        {

            if (!_redisCacheService.IsSet(RedisKey.HotelEntityKey))
            {
                _redisCacheService.Add(RedisKey.HotelEntityKey, _hotelRepertory.GetAll());
            }
            return _redisCacheService.Get<List<CoreSysHotel>>(RedisKey.HotelEntityKey);

        }

        private IEnumerable<CoreSysBrand> Brands()
        {

            if (!_redisCacheService.IsSet(RedisKey.BrandKey))
            {
                _redisCacheService.Add(RedisKey.BrandKey, _brandRepertory.GetAll());
            }
            return _redisCacheService.Get<List<CoreSysBrand>>(RedisKey.BrandKey);

        }
        private IEnumerable<CoreSysGroup> Groups()
        {

            if (!_redisCacheService.IsSet(RedisKey.GroupKey))
            {
                _redisCacheService.Add(RedisKey.GroupKey, _groupRepertory.GetAll());
            }
            return _redisCacheService.Get<List<CoreSysGroup>>(RedisKey.GroupKey);

        }

        public void UpdateCache()
        {
            _redisCacheService.Remove(RedisKey.HoteleKey);
            _redisCacheService.Remove(RedisKey.HotelEntityKey);
        }

        #endregion

        public HotelManager(ISysHotelRepertory hotelRepertory, ISysBrandRepertory brandRepertory, ISysGroupRepertory groupRepertory, IRedisCacheService redisCacheService,
            ISysHotelSencondRepertory hotelSencondRepertory,
            IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceNoTemplateWrapperFacade,
            IDeviceTraceLibraryManager deviceTraceLibraryManager,
            IAppLibraryManager appManager)
        {
            _hotelRepertory = hotelRepertory;
            _brandRepertory = brandRepertory;
            _groupRepertory = groupRepertory;
            _redisCacheService = redisCacheService;
            _hotelSencondRepertory = hotelSencondRepertory;
            _hotelMovieTraceNoTemplateWrapperFacade = hotelMovieTraceNoTemplateWrapperFacade;
            _deviceTraceLibraryManager = deviceTraceLibraryManager;
            _appManager = appManager;
        }

        public void UpdateDataBase(string id, string baseData)
        {
            var entity = _hotelSencondRepertory.FindByKey(id);
            entity.BaseData = baseData;
            _hotelSencondRepertory.Update(entity);

            UpdateCache();
        }

        public string GetDataBase(string id)
        {
            var hotel = GetHotel(id);
            return hotel == null ? null : hotel.BaseData;
        }

        public string Add(CoreSysHotel entity)
        {
            if (entity.CoreSysHotelSencond == null)
                throw new NullReferenceException("entity.CoreSysHotelSencond is null!");
            _hotelRepertory.Insert(entity);
            UpdateCache();

            try
            {
                _hotelMovieTraceNoTemplateWrapperFacade.DistributeByHotel(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("酒店创建成功，分发电影失败：{0}", ex.Message));
            }

            return entity.Id;
        }
        [UnitOfWork]
        public void Update(CoreSysHotel entity)
        {
            var dbEntity = HotelEntities().FirstOrDefault(m => m.Id == entity.Id);
            if (dbEntity != null && dbEntity.CoreSysHotelSencond != null)
            {
                var tempBaseData = dbEntity.CoreSysHotelSencond.BaseData;
                var brandId = dbEntity.BrandId;
                var hotelCode = dbEntity.HotelCode;
                entity.CopyTo(dbEntity, new[] { "CreateTime" });
                dbEntity.CoreSysHotelSencond.BaseData = tempBaseData;
                dbEntity.BrandId = brandId;
                dbEntity.HotelCode = hotelCode;
                dbEntity.Id = entity.Id;
                dbEntity.CoreSysHotelSencond.Id = entity.Id;
                _hotelRepertory.Update(dbEntity);
                _hotelSencondRepertory.Update(dbEntity.CoreSysHotelSencond);
                UpdateCache();
            }
            else
                throw new NullReferenceException("CoreSysHotel or CoreSysHotelSencond is null!");
        }
        public HotelEntity GetHotel(string hotelId)
        {
            return Hotels().FirstOrDefault(m => m.HotelId.Equals(hotelId));
        }
        /// <summary>
        /// IU酒店个性化配置(CoreSysHotelSencond表):批量配置,AutoToHome,WelcomeWorld等字段信息
        /// </summary>
        /// <param name="Predicate">条件</param>
        /// <param name="Updater">修改项</param>
        /// <returns></returns>
        public int Update(System.Linq.Expressions.Expression<Func<CoreSysHotelSencond, bool>> Predicate, System.Linq.Expressions.Expression<Func<CoreSysHotelSencond, CoreSysHotelSencond>> Updater)
        {
            int c = _hotelSencondRepertory.Update(Predicate, Updater);
            UpdateCache();
            return c;
        }

        public HotelRoomEntity GetHotelByDeviceId(string deviceId)
        {
            string backupDeviceHotelId;

            // 判断设备是否有效
            backupDeviceHotelId = CheckDeviceBind(deviceId);

            string hotelId;
            string roomNo;
            if (null == backupDeviceHotelId)
            {
                // 获取HotelId
                var hotelInfo = _deviceTraceLibraryManager.Search(new DeviceTraceCriteria { DeviceSeries = deviceId }).FirstOrDefault();
                hotelId = hotelInfo.HotelId;
                roomNo = hotelInfo.RoomNo;
            }
            else
            {
                hotelId = backupDeviceHotelId;
                roomNo = string.Empty;
            }

            var hotelEntity = GetHotel(hotelId);

            HotelRoomEntity hotelRoomEntity = new HotelRoomEntity();
            hotelEntity.CopyTo(hotelRoomEntity);
            hotelRoomEntity.RoomNo = roomNo;

            return hotelRoomEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns>如果是备用机则返回备用机的HotelId</returns>
        private string CheckDeviceBind(string deviceId)
        {
            var header = new RequestHeader();
            header.DEVNO = deviceId;
            header.APP_ID = "KFC";      // 建立虚拟的APP_ID

            //判断该设备是否有绑定关系，如果没有绑定关系，则抛出该问题
            var trace = _deviceTraceLibraryManager.GetAppTrace(header);
            BackupDevice backupDevice = null;
            if (trace == null)
            {
                backupDevice = _appManager.GetAppBackupDevice(header);
            }
            if (trace == null && backupDevice == null)
            {
                throw new ApiException(ApiErrorType.Default, String.Format("该设备{0}尚未绑定，请绑定以后才可使用", header.DEVNO));
            }
            if (trace != null && !trace.Active)
            {
                throw new ApiException(ApiErrorType.Default, String.Format("该设备{0}已失效，无法继续使用", header.DEVNO));
            }

            string returnValue;
            if (backupDevice != null)
            {
                returnValue = backupDevice.HotelId;
            }
            else
            {
                returnValue = null;
            }

            return returnValue;
        }

        public HotelObject GetHotelObject(string hotelId)
        {
            var hotel = GetHotel(hotelId);
            var brand = Brands().FirstOrDefault(m => m.Id == hotel.BrandId);
            var group = Groups().FirstOrDefault(m => m.Id == hotel.GroupId);
            return new HotelObject()
            {
                Hotel = hotel,
                Brand = brand,
                Group = group
            };
        }
        public void BatchDelete(string[] ids)
        {
            var resultSet = HotelEntities().Where(m => ids.Contains(m.Id));
            foreach (var hotel in resultSet)
            {
                //hotel.TvTemplate = null;
                hotel.IsDelete = true;
                _hotelRepertory.Update(hotel);
            }

            UpdateCache();
        }

        public List<HotelEntity> GetByBrand(string brandId)
        {
            return Hotels().Where(m => m.BrandId.Equals(brandId)).ToList();
        }

        public List<CoreSysHotel> GetByGroup(string groupId)
        {
            return HotelEntities().Where(m => m.GroupId.Equals(groupId)).ToList();
        }
        public List<CoreSysHotel> GetAllCoreSysHotels()
        {
            return HotelEntities().ToList();
        }
        public CoreSysHotel GetCoreSysHotelById(string id)
        {
            return HotelEntities().FirstOrDefault(m => m.Id == id);
        }

        public CoreSysHotel GetCoreSysHotelByname(string name)
        {
            return HotelEntities().FirstOrDefault(m => m.HotelName == name);
        }
        public List<HotelEntity> GetAllHotels()
        {
            return Hotels().ToList();
        }

        public int[] GetAllUseCity()
        {
            var useCity = Hotels().Select(m => m.City).Distinct().ToArray();
            return useCity;
        }

        public IPagedList<CoreSysHotel> PagedList(int pageIndex, int pageSize, string keyword)
        {
            var pageList = new PagedList<CoreSysHotel>(HotelEntities().OrderByDescending(m => m.HotelName).ToList(), pageIndex, pageSize);
            return pageList;
        }
        public List<CoreSysHotel> Search(CoreSysHotelCriteria criteria)
        {
            return _hotelRepertory.Search(criteria);
        }
        public int GetSameBrandHotelCount(string brandId)
        {
            return _hotelRepertory.GetSameBrandHotelCount(brandId);
        }
        //public void UpdateHotelData(string id, string hotelDevice)
        //{
        //    var entity = _hotelSencondRepertory.FindByKey(id);
        //    List<HotelDeviceInfo> hotelDeviceList = null;
        //    if (entity.HotelDeviceInfo != null)
        //    {

        //        hotelDeviceList = entity.HotelDeviceInfo.JsonStringToObj<List<HotelDeviceInfo>>();
        //        hotelDeviceList.AddRange(hotelDevice.JsonStringToObj<List<HotelDeviceInfo>>());
        //    }
        //    else
        //        hotelDeviceList.AddRange(hotelDevice.JsonStringToObj<List<HotelDeviceInfo>>());
        //    entity.DeviceInfo = hotelDeviceList.ToJsonString();
        //    _hotelSencondRepertory.Update(entity);

        //    //UpdateCache();
        //}


        public string GetHotelDeviceInfo(string id)
        {
            var hotel = GetHotel(id);
            return hotel == null ? null : hotel.HoteDeviceInfo;
        }


        //public string UpdateHotelDeviceDetail(string hotelId, string hotelDeviceDetail)
        //{
        //    var entity = _hotelSencondRepertory.FindByKey(hotelId);
        //    List<DeviceInfo> hotelDeviceList = null;
        //    try
        //    {
        //        if (entity.DeviceInfo != null)
        //        {
        //            hotelDeviceList = entity.HotelDeviceInfo.JsonStringToObj<List<DeviceInfo>>();
        //            hotelDeviceList.AddRange(hotelDeviceDetail.JsonStringToObj<List<DeviceInfo>>());
        //        }
        //        else
        //            hotelDeviceList.AddRange(hotelDeviceDetail.JsonStringToObj<List<DeviceInfo>>());
        //        entity.DeviceInfo = hotelDeviceList.ToJsonString();
        //        _hotelSencondRepertory.Update(entity);
        //        return "success";
        //    }
        //    catch (Exception e)
        //    {
        //        throw (e);
        //    }

        //}

        public string GetHotelDeviceDetail(string id)
        {
            var hotel = GetHotel(id);
            return hotel == null ? null : hotel.DeviceInfo;
        }
    }
}
