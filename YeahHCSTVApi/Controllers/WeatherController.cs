using System.Collections.Generic;
using System.Web.Http;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.Models.YeahHcsApi;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using YeahTVApi.Entity;

namespace YeahHCSTVApi.Controllers
{
    public class WeatherController : BaseApiController
    {
        private IHttpContextService httpContext;
        private ILogManager logManager;
        private IRedisCacheManager redisCacheManager;
        private IRequestApiService requestApiService;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private ICacheManager cacheManager;

        public WeatherController(
           IHttpContextService httpContext,
           ILogManager logManager,
           IRedisCacheManager redisCacheManager,
           IRequestApiService requestApiService,
           IConstantSystemConfigManager constantSystemConfigManager,
           ICacheManager cacheManager)
        {

            this.httpContext = httpContext;
            this.logManager = logManager;
            this.redisCacheManager = redisCacheManager;
            this.requestApiService = requestApiService;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.cacheManager = cacheManager;
        }

        [HttpPost]
        [ActionName("GetWeather")]
        [YeahApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = true)]
        public ResponseData<object> GetWeather(PostParameters<object> request)
        {
            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + Header.HotelID;

            var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();

            requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetBrandUrl + hotel.BrandId;
            var brand = requestApiService.Get(requestHotelUrl).JsonStringToObj<CoreSysBrand>();

            var weathers = redisCacheManager.Get<List<WeatherDataWithPm25>>(Constant.SystemWeatherKey + hotel.City.ToString());

            if (weathers == null || !weathers.Any())
            {
                var task = new Task(() => { cacheManager.SetWeather(); });
                task.Start();

                throw new ApiException(ApiErrorType.System, "天气不存在！请稍后再试！");
            }
            else
            {
                weathers.ForEach(w =>
                {
                    w.DayPictureUrl = string.Format("{0}Template/{1}/Weather/{2}", constantSystemConfigManager.YeahInfoResourceSiteAddress, brand.TemplateId, w.DayPictureUrl);
                });
            }

            return new ResponseData<object> { Data = weathers };
        }
    }
}
