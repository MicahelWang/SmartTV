using System;
using System.Linq;
using System.Web.Http;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.Models.YeahHcsApi;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;

namespace YeahHCSTVApi.Controllers
{
    public class HCSCacheVersionController : BaseApiController
    {
        private readonly IDeviceTraceLibraryManager _deviceTraceLibraryManager;
        private readonly IRequestApiService _requestApiService;
        private readonly ILogManager logManager;
        private readonly IHCSCacheVersionManager _hcsCacheVersionManager;
        private readonly IConstantSystemConfigManager _constantSystemConfigManager;

        public HCSCacheVersionController(IDeviceTraceLibraryManager deviceTraceLibraryManager, ILogManager logManager, IHCSCacheVersionManager hcsCacheVersionManager, IConstantSystemConfigManager constantSystemConfigManager, IRequestApiService requestApiService)
        {
            this._deviceTraceLibraryManager = deviceTraceLibraryManager;
            this._requestApiService = requestApiService;
            this.logManager = logManager;
            this._hcsCacheVersionManager = hcsCacheVersionManager;
            this._constantSystemConfigManager = constantSystemConfigManager;
        }
        [HttpPost]
        [ActionName("GetVersion")]
        [YeahApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = false)]
        public ResponseData<string> GetVersion(PostParameters<string> request)
        {
            int result = 0;
            try
            {
                var hotelId=_deviceTraceLibraryManager.GetSingle(new YeahTVApi.DomainModel.SearchCriteria.DeviceTraceCriteria(){ DeviceSeries=request.Server_Id}).HotelId;
                var brandId = GetBranId(hotelId);
                var searchVersionFunc = new Func<string, string, HCSCacheVersion>((typeId, permitionType) => _hcsCacheVersionManager.Search(new YeahTVApi.DomainModel.SearchCriteria.HCSCacheVersionCriteria()
                {
                    TypeId = typeId,
                    PermitionType = permitionType
                }).FirstOrDefault());

                var hotelVersion = searchVersionFunc(hotelId, "Hotel");
                var brandVersion = searchVersionFunc(brandId, "Brand");

                //var result1 = (hotelVersion != null && brandVersion != null) ? ((hotelVersion.LastUpdateTime > brandVersion.LastUpdateTime) ? hotelVersion.Version : brandVersion.Version) : ((hotelVersion == null && brandVersion == null) ? 1 : (hotelVersion ?? brandVersion).Version);
                //var result2 = (hotelVersion == null) ? (brandVersion != null ? brandVersion.Version : 1) : (brandVersion != null ? hotelVersion.LastUpdateTime > brandVersion.LastUpdateTime ? hotelVersion.Version : brandVersion.Version : hotelVersion.Version);

                if (hotelVersion == null)
                {
                    if (brandVersion != null)
                    {
                        result = brandVersion.Version;
                    }
                    else
                    {
                        result = 1;
                    }
                }
                else
                {
                    if (brandVersion != null)
                    {
                        result = hotelVersion.LastUpdateTime > brandVersion.LastUpdateTime ? hotelVersion.Version : brandVersion.Version;
                    }
                    else
                    {
                        result = hotelVersion.Version;
                    }
                }
            }
            catch (CommonFrameworkManagerException ex)
            {
                logManager.SaveError("获取HCS缓存版本号失败！", ex, AppType.TV);
            }
            return new ResponseData<string>() { Data = result.ToString() };
        }
        private string GetBranId(string hotelId)
        {
            var requestHotelUrl = _constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + hotelId;
            var hotel = _requestApiService.HttpRequest(requestHotelUrl, "DETAIL").JsonStringToObj<HotelObject>();
            var brandId = hotel.Hotel.BrandId;
            return brandId;
        }

    }
}
