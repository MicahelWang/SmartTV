using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.Models.YeahHcsApi;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;


namespace YeahHCSTVApi.Controllers
{
    public class AppController : BaseApiController
    {
        private IAppLibraryManager applibrarymanager;
        private IHttpContextService httpContext;
        private IDeviceTraceLibraryManager traceManager;
        private ILogManager logManager;
        private IRedisCacheManager redisCacheManager;
        private IRequestApiService requestApiService;
        private IDeviceAppsMonitorManager deviceAppsMonitorManager;
        private ITVHotelConfigManager tVHotelConfigManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IAppLibraryManager AppManager;
        private IAppPublishLibraryRepertory appPublish;

        public AppController(
            IAppPublishLibraryRepertory appPublish,
           IDeviceTraceLibraryManager traceManager,
           IHttpContextService httpContext,
           ILogManager logManager,
           IRedisCacheManager redisCacheManager,
           IRequestApiService requestApiService,
           IDeviceAppsMonitorManager deviceAppsMonitorManager,
           ITVHotelConfigManager tVHotelConfigManager, IAppLibraryManager applibrarymanager,
           IConstantSystemConfigManager constantSystemConfigManager,
            IAppLibraryManager AppManager
           )
        {

            this.traceManager = traceManager;
            this.httpContext = httpContext;
            this.logManager = logManager;
            this.redisCacheManager = redisCacheManager;
            this.requestApiService = requestApiService;
            this.deviceAppsMonitorManager = deviceAppsMonitorManager;
            this.tVHotelConfigManager = tVHotelConfigManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.applibrarymanager = applibrarymanager;
            this.AppManager = AppManager;
            this.appPublish = appPublish;
        }

        [HttpPost]
        [ActionName("Initialize")]
        [YeahApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = true)]
        public ResponseData<Dictionary<string, string>> Initialize(PostParameters<RequestHcsHeader> request)
        {
            var hotelCofigList = new Dictionary<string, string>();
            var configCodeArray = new[] { "launcherVersion", "launcherDownLoadUrl" };

            try
            {
                List<TVHotelConfig> tvConfigList = tVHotelConfigManager.SearhTVHotelConfig(new HotelConfigCriteria { HotelId = Header.HotelID, Active = true });
                tvConfigList.FindAll(m => m.ConfigCode.Equals("TvHomePage") || m.ConfigCode.Equals("EnbleUmeng")).ForEach(
                    m =>
                    {
                        m.ConfigValue = m.ConfigCode == "EnbleUmeng" ? m.ConfigValue.ToLower() : m.ConfigValue;
                        hotelCofigList.Add(m.ConfigCode, m.ConfigValue);
                    });

                var deviceTraceConfigs = traceManager.GetConfigData(request.Data.PackageName, Header);
                if (deviceTraceConfigs != null)
                    deviceTraceConfigs.Where(d => configCodeArray.Contains(d.DictCode)).ToList().ForEach(c => hotelCofigList.Add(c.DictCode, c.DictValue));

                hotelCofigList.Add("systemTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                traceManager.InitializeLogDeviceTrace(Header);
                logManager.SaveInfo("获得启动URL和配置接口成功", "SearhTVHotelConfig", AppType.TV);
            }
            catch (Exception e)
            {
                logManager.SaveInfo("获得启动URL和配置接口失败", "Initialize", AppType.TV);
                throw new ApiException(ApiErrorType.System, e.Message);
            }
            return new ResponseData<Dictionary<string, string>> { Data = hotelCofigList };
        }

        [HttpPost]
        [ActionName("Start")]
        [YeahApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = true)]
        public ResponseData<StartResponse> Start(PostParameters<RequestHcsHeader> request)
        {
            var rst = new ResponseData<StartResponse>();
            var host = Request.RequestUri.AbsoluteUri.Substring(0, Request.RequestUri.AbsoluteUri.IndexOf(Request.RequestUri.AbsolutePath));
            rst.Data = GetAppStartConfig(this.Header, host, request.Data.PackageName);
            return rst;
        }

        [HttpPost]
        [ActionName("GetDeviceApps")]
        [YeahApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = true)]
        public ResponseData<List<DeviceAppsMonitoApiMode>> GetDeviceApps(PostParameters<PostAppsListParameters> request)
        {
            var deviceAppsMonitors = new List<DeviceAppsMonitoApiMode>();

            try
            {
                deviceAppsMonitors = deviceAppsMonitorManager.SearchDeviceAppsMonitorResponse(Header, request.Data.AppListRequestModels.Distinct().ToList());
                logManager.SaveInfo("设置APP版本成功", "SearchDeviceAppsMonitorResponse", AppType.TV);
            }
            catch (CommonFrameworkManagerException ex)
            {
                logManager.SaveError("设置APP版本失败", ex, AppType.TV);
            }
            return new ResponseData<List<DeviceAppsMonitoApiMode>> { Sign = "", Data = deviceAppsMonitors };
        }

        [HttpPost]
        [ActionName("AddBehaviorLog")]
        [YeahApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = true)]
        public ResponseData<string> AddBehaviorLog(PostParameters<BehaviorLogParameters> request)
        {
            var result = "";
            try
            {
                //var behaviorLogRequests = JsonConvert.DeserializeObject<>(request.Data);
                logManager.SaveBehavior(request.Data.BehaviorLogRequestString, Header.HotelID, request.DeviceSeries);
            }
            catch (CommonFrameworkManagerException ex)
            {
                result = ex.Message;
                logManager.SaveError("记录用户行为日志失败", ex, AppType.TV);
            }
            return new ResponseData<string>() { Data = result };
        }
        private object GetTemplateAction(string templateId, string getTemplateUrl)
        {
            return UtilityHelper.RequestTemplateByRootName(templateId, getTemplateUrl, TemplateRootType.Launcher);
        }
        private StartResponse GetAppStartConfig(RequestHeader header, string host, string packageName)
        {
            var rst = new StartResponse();
            try
            {
                var traces = traceManager.GetConfigData(packageName, header);
                //载入该设备对应的酒店以及房间数据
                var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + header.HotelID;//
                var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();
                rst.TemplateContent = GetTemplateAction(hotel.TemplateId, constantSystemConfigManager.AppCenterUrl + Constant.GetTemplateUrl);
                Deassign(hotel.BrandId, rst);
                logManager.SaveInfo("GetAppStartConfig - hotel", hotel.ToJsonString(), AppType.TV);
                if (null == hotel)
                {
                    throw new ApiException("未查到酒店信息:hotelId" + header.HotelID);
                }
                var httpHost = PubFun.ChangetHttpsToHttps(host);
                rst.Hotel = GetHotelEntityAction(hotel);
                rst.RoomNo = header.RoomNo;
                //添加配置参数
                rst.ConfigData = new Dictionary<string, string>();
                foreach (var trace in traces)
                {
                    string key = trace.DictCode;
                    string val = trace.DictValue;
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(val))
                        continue;
                    if (val.IndexOf("{host}") > -1)
                        val = val.Replace("{host}", host);
                    if (val.IndexOf("{httpHost}") > -1)
                        val = val.Replace("{httpHost}", httpHost);
                    else
                    {
                        rst.ConfigData.Add(key, val);
                    }

                }
                //添加当前系统时间
                rst.ConfigData.Add("SYSTEM_TIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                var hotelConfig = tVHotelConfigManager.SearchFromCache(new HotelConfigCriteria { HotelId = header.HotelID })
                    .Where(t => t.Active.Value).ToList();

                hotelConfig.ForEach(h =>
                {
                    if (h.ConfigCode != "VodAddress")
                        rst.ConfigData.Add(h.ConfigCode, h.ConfigValue);
                });
            }
            catch (Exception err)
            {
                logManager.SaveError(err.ToString(), err, AppType.TV);
                throw err;
            }
            return rst;
        }
        private void Deassign(string brandId, StartResponse rst)
        {
            string content = rst.TemplateContent.ToString();
            if (!content.Contains("{}"))
            {
                object coreBrand = GetBrandTemplateId(brandId);

                if (rst.TemplateContent != null && coreBrand != null && ((CoreSysBrand)coreBrand).TemplateId != null)
                {
                    var templateContent = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

                    templateContent.Remove("templateType");
                    templateContent.Add("templateType", ((CoreSysBrand)coreBrand).TemplateId);

                    rst.TemplateContent = templateContent;
                }

            }

        }
        private object GetBrandTemplateId(string brandId)
        {

            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetBrandUrl + brandId;//
            var brand = requestApiService.Get(requestHotelUrl).JsonStringToObj<CoreSysBrand>();
            return brand;
        }
        private object GetHotelEntityAction(HotelEntity hotelEntity)
        {
            return hotelEntity.CopyTo<SimpleHotel>();
        }
        [HttpPost]
        [ActionName("UploadLog")]
        [YeahApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = false, IsCheckDeviceBind = true)]
        public ResponseData<string> UploadLog(PostParameters<YeahInfoLog> request)
        {
            var result = "";
            try
            {
                logManager.SaveInfo(request.ToJsonString(), request.Data.DEVNO + "/" + request.Data.PackageName, AppType.TV);
            }
            catch (CommonFrameworkManagerException ex)
            {
                result = ex.Message;
                logManager.SaveError("记录用户行为日志失败", ex, AppType.TV);
            }
            return new ResponseData<string>() { Data = result };
        }
    }
}
