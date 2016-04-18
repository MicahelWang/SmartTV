using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Entity;
using YeahTVApi.Filter;
using YeahTVApi.Infrastructure;

using YeahTVApi.Utilty;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;
using System.Linq;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.SearchCriteria;


namespace YeahTVApi.Controllers
{
    /// <summary>
    /// 酒店相关数据申请接口
    /// </summary>
    public class HotelController : BaseController
    {
        private IRequestApiService requestApiService;
        private IHttpContextService httpContextService;
        private ILogManager logManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IRedisCacheManager redisCacheManager;
        private ITVHotelConfigManager tVHotelConfigManager;

        public HotelController(
            IRequestApiService requestApiService,
            IHttpContextService httpContextService,
            ILogManager logManager,
            IConstantSystemConfigManager constantSystemConfigManager,
            IRedisCacheManager redisCacheManager,
            ITVHotelConfigManager tVHotelConfigManager)
        {

            base.HttpContextService = httpContextService;
            this.httpContextService = httpContextService;
            this.requestApiService = requestApiService;
            this.logManager = logManager;
            this.redisCacheManager = redisCacheManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.tVHotelConfigManager = tVHotelConfigManager;
        }

        /// <summary>
        /// 获取酒店详情
        /// </summary>
        /// <param name="HotelID">酒店ID</param>
        /// <returns></returns>
        [HTApiFilter]
        public ApiObjectResult<HotelObject> GetHotelDetail()
        {
            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + Header.HotelID;
            var hotel = requestApiService.HttpRequest(requestHotelUrl, "DETAIL").JsonStringToObj<HotelObject>();

            return new ApiObjectResult<HotelObject> { obj = hotel };
        }

        /// <summary>
        /// 获取天气预报数据
        /// </summary>
        /// <param name="CityName">城市名称</param>
        /// <returns>json格式的天气预报数据</returns>
        [HTApiFilter]
        public ApiObjectResult<object> GetWeather()
        {
            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + Header.HotelID;
            var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();

            requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetBrandUrl + hotel.BrandId;
            var brand = requestApiService.Get(requestHotelUrl).JsonStringToObj<CoreSysBrand>();

            var weatherString = redisCacheManager.Get<List<WeatherData>>(Constant.SystemWeatherKey + hotel.City.ToString());

            weatherString.ForEach(w => 
            {
                w.DayPictureUrl = string.Format("{0}Template/{1}/Weather/{2}", constantSystemConfigManager.ResourceSiteAddress, brand.TemplateId, w.DayPictureUrl); 
            });

            return new ApiObjectResult<object> { obj = weatherString };
        }

        [HTWebFilterAttribute(ShouldNotBindDevice = true)]
        public ApiResult PutBaseData(string hotelId, string baseData)
        {
            var res = new ApiResult();
            try
            {
           
                //var requestHotelUrl = "http://192.168.8.8:8099/" + Constant.GetHotelApiUrl + hotelId + "?baseData=" + baseData;
                var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + hotelId + "?baseData=" + baseData;
                var hotel = requestApiService.HttpRequest(requestHotelUrl, "PutBaseData");
                return res.WithOk();
            }
            catch (Exception ex)
            {
                logManager.SaveError("添加失败", ex, AppType.CommonFramework);
                return res.WithError(ex.ToString());
            }

        }

        [HTWebFilterAttribute(ShouldNotBindDevice = true)]
        public ApiObjectResult<object> GetBaseData(string hotelId)
        {
            try
            {
               // var requestHotelUrl = "http://192.168.8.8:8099/" + Constant.GetHotelApiUrl + hotelId ;
                var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + hotelId;
                var hotel = requestApiService.HttpRequest(requestHotelUrl, "GetDataBase").JsonStringToObj<object>();
                return new ApiObjectResult<object>() { obj = hotel == null ? null : hotel };
            }
            catch (Exception ex)
            {
                logManager.SaveError("添加失败", ex, AppType.CommonFramework);
                throw ex;
            }
        }
        
        public ApiObjectResult<object> GetPrivateKey()
        {
            try
            {
                var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl;
                var hotel = requestApiService.HttpRequest(requestHotelUrl, "GetDataBase").JsonStringToObj<object>();
                return new ApiObjectResult<object>() { obj = hotel == null ? null : hotel };
            }
            catch (Exception ex)
            {
                logManager.SaveError("添加失败", ex, AppType.CommonFramework);
                throw ex;
            }
        }

        [HTApiFilter]
        public ApiObjectResult<object> GetHotelConfig(string configCodes)
        {
            if (string.IsNullOrEmpty(configCodes))
            {
                logManager.SaveError("configNames 为空！", null, AppType.TV);
                return new ApiObjectResult<object>().WithError("configNames 为空！");
            }

            var configs = tVHotelConfigManager.SearchFromCache(new HotelConfigCriteria { HotelId = Header.HotelID, ConfigCodes = configCodes, Active = true });

            var configForApis = configs.AsParallel().Select(s => new TVHotelConfigForApi
            {
                ConfigCode = s.ConfigCode,
                ConfigName = s.ConfigName,
                ConfigValue = s.ConfigValue,
                HotelId = s.HotelId
            });

            return new ApiObjectResult<object>() { obj = configForApis };
        }
    }
}
