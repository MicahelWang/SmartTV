
namespace YeahTVApi.Controllers
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Mapping;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApi.Infrastructure;
    using YeahTVApi.Manager;
    using YeahTVApiLibrary.Controllers;
    using YeahTVApiLibrary.Filter;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.Models.ViewModels;
    using YeahTVApi.Filter;
    using YeahAppCentre.Web.Utility;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 客户端APP控制器
    /// </summary>
    public class AppController : BaseController
    {
        private IAppLibraryManager applibrarymanager;
        private IHttpContextService httpContext;
        private ITraceManager traceManager;
        private ILogManager logManager;
        private IRedisCacheManager redisCacheManager;
        private IRequestApiService requestApiService;
        private IDeviceAppsMonitorManager deviceAppsMonitorManager;
        private ITVHotelConfigManager tVHotelConfigManager;
        private IConstantSystemConfigManager constantSystemConfigManager;

        public AppController(
            ITraceManager traceManager,
            IHttpContextService httpContext,
            ILogManager logManager,
            IRedisCacheManager redisCacheManager,
            IRequestApiService requestApiService,
            IDeviceAppsMonitorManager deviceAppsMonitorManager,
            ITVHotelConfigManager tVHotelConfigManager, IAppLibraryManager applibrarymanager,
            IConstantSystemConfigManager constantSystemConfigManager)
        {

            this.traceManager = traceManager;
            this.httpContext = httpContext;
            this.logManager = logManager;
            this.redisCacheManager = redisCacheManager;
            this.requestApiService = requestApiService;
            this.deviceAppsMonitorManager = deviceAppsMonitorManager;
            this.tVHotelConfigManager = tVHotelConfigManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
        }

        /// <summary>
        /// 启动应用程序
        /// </summary>
        /// <returns>返回数据</returns>
        [HTApiFilter]
        public ApiObjectResult<StartResponse> Start()
        {
            //获取数据
            //结果封装
            bool isOldVersion = true;
            var rst = new ApiObjectResult<StartResponse>();
            var host = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.AbsolutePath));
            rst.obj = GetAppStartConfig(this.Header, host, isOldVersion,
                (templateId, getTemplateUrl) =>
                {
                    return UtilityHelper.RequestTemplateByRootName(templateId, getTemplateUrl, TemplateRootType.Document);
                },
                (hotel) =>
                {
                    return hotel.CopyTo<ObsoleteSimpleHotel>();
                });

            return rst;
        }

        [HTApiFilter]
        public ApiObjectResult<StartResponse> NewStart()
        {
            //获取数据
            //结果封装
            bool isOldVersion = false;
            var rst = new ApiObjectResult<StartResponse>();
            var host = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf(Request.Url.AbsolutePath));

            rst.obj = GetAppStartConfig(this.Header, host, isOldVersion,
               (templateId, getTemplateUrl) =>
               {
                   return UtilityHelper.RequestTemplateByRootName(templateId, getTemplateUrl, TemplateRootType.Launcher);
               },
               (hotel) =>
               {
                   return hotel.CopyTo<SimpleHotel>();
               });

            return rst;
        }

        ///// <summary>
        /// 启动应用程序
        /// </summary>
        /// <returns>返回数据</returns>
        [HTApiFilter(ShouldNotBindDevice = true)]
        public ApiObjectResult<WorkStartResponse> WorkStart()
        {
            var workStartResponse = new WorkStartResponse();
            var traces = traceManager.LogDeviceTraceShouldNotCheckBind(this.Header);
            workStartResponse.ConfigData = new Dictionary<string, string>();
            foreach (var trace in traces)
            {
                string key = trace.DictCode;
                string val = trace.DictValue;
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(val))
                    continue;
                else
                {
                    workStartResponse.ConfigData.Add(key, val);
                }

            }

            workStartResponse.ConfigData.Add("SYSTEM_TIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            return new ApiObjectResult<WorkStartResponse> { obj = workStartResponse };
        }


        [HTWebFilter]
        public ApiObjectResult<object> GetUserNameByPwd(string name, string password)
        {
            var url = string.Format(constantSystemConfigManager.OpenApiAddress + Constant.GetAuthUrl, name, password);
            var objs = requestApiService.HttpRequest(url, "Post").JsonStringToObj<MsgResult>();
            return new ApiObjectResult<object>() { obj = objs.Data };
        }


        [HTApiFilter(ShouldNotBindDevice = true)]
        public ApiObjectResult<object> GetAuthForApp(string password)
        {
            var url = string.Format(constantSystemConfigManager.OpenApiAddress + Constant.GetAuthForAppUrl + "deviceNo={0}&password={1}", Header.DEVNO, password);
            // var url = string.Format("http://localhost:8088/" + Constant.GetAuthForAppUrl + "deviceNo={0}&password={1}", Header.DEVNO, password);
            var response = requestApiService.HttpRequest(url, "Post");
            var objs = response.JsonStringToObj<MsgResult>();

            if (objs.HasError)
            {
                throw new ApiException(objs.Msg);
            }

            return new ApiObjectResult<object>() { obj = objs.Data };
        }


        [HTApiFilter]
        public ApiObjectResult<List<DeviceAppsMonitoApiMode>> GetDeviceApps(string appListRequestModelsString)
        {
            var appListRequestModels = appListRequestModelsString.JsonStringToObj<List<AppListRequestModel>>();
            var deviceAppsMonitors = new List<DeviceAppsMonitoApiMode>();

            try
            {
                deviceAppsMonitors = deviceAppsMonitorManager.SearchDeviceAppsMonitorResponse(Header, appListRequestModels.Distinct().ToList());
                logManager.SaveInfo("设置APP版本成功", "SearchDeviceAppsMonitorResponse", AppType.CommonFramework);
            }
            catch (CommonFrameworkManagerException ex)
            {
                logManager.SaveError("设置APP版本失败", ex, AppType.CommonFramework);
            }

            return new ApiObjectResult<List<DeviceAppsMonitoApiMode>> { obj = deviceAppsMonitors };
        }

        [HTApiFilter]
        public ApiObjectResult<object> AddBehaviorLog(string behaviorLogRequestString)
        {
            try
            {
                var behaviorLogRequests = behaviorLogRequestString.JsonStringToObj<List<BehaviorLogRequest>>();

                var logs = behaviorLogRequests.Select(m => new BehaviorLogRequestNew()
                {
                    BehaviorType = BehaviorType.Other.ToString(),
                    ObjectInfo = m.Message + m.ObjectInfo
                }).ToList();

                logManager.SaveBehavior(logs, Header.HotelID, Header.DEVNO);
                return new ApiObjectResult<object>();
            }
            catch (Exception ex)
            {
                logManager.SaveError("AddBehaviorLog Error", ex, AppType.TV);
                throw new ApiException(ex.ToString());
            }
        }

        [HTApiFilter]
        public ApiObjectResult<object> AddBehaviorLogNew(string behaviorLogRequestString)
        {
            try
            {
                var behaviorLogRequests = behaviorLogRequestString.JsonStringToObj<List<BehaviorLogRequestNew>>();
                logManager.SaveBehavior(behaviorLogRequests, Header.HotelID, Header.DEVNO);
                return new ApiObjectResult<object>();
            }
            catch (Exception ex)
            {
                logManager.SaveError("AddBehaviorLogNew Error", ex, AppType.TV);
                throw new ApiException(ex.ToString());
            }
        }


        /// <summary>
        /// Add at 2015.6
        /// </summary>
        /// <param name="systemLogRequestString"></param>
        /// <returns></returns>
        [HTApiFilter]
        public ApiObjectResult<object> AddSystemLog(string systemLogRequestString)
        {
            try
            {
                var systemLogRequests = systemLogRequestString.JsonStringToObj<List<SystemLogRequest>>();
                logManager.SaveSystemLog(systemLogRequests, Header.APP_ID);
                return new ApiObjectResult<object>();
            }
            catch (Exception ex)
            {
                logManager.SaveError("AddSystemLog Error", ex, AppType.TV);
                throw new ApiException(ex.ToString());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        [HTWebFilterAttribute]//这里要改为false(ShouldNotBindDevice = true)
        public ApiListResult<SimpleHotelEntity> GetHotelsByLocation(string lat, string lng, string radius = "500")
        {
            logManager.SaveInfo("start GetHotelsByLocation", "lat:" + lat + "lng:" + lng + "radius:" + radius, AppType.TV);

            var newlist = new List<SimpleHotelEntity>();
            var hotels = new List<HotelEntity>();
            string url = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl;
            hotels = requestApiService.HttpRequest(url, "GetAll").JsonStringToObj<List<HotelEntity>>();
            //hotels=hotels.Where(h => h.GetDistance(double.Parse(lat), double.Parse(lng), double.Parse(radius))).ToList();
            foreach (var item in hotels)
            {
                newlist.Add(new SimpleHotelEntity()
                {
                    HotelId = item.HotelId,
                    HotelCode = item.HotelCode,
                    Address = item.Address,
                    HotelName = item.HotelName,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    Logo = LogoUrl(item.BrandId)
                });
            }

            logManager.SaveInfo("GetHotelsByLocation", newlist.ToJsonString(), AppType.TV);

            return new ApiListResult<SimpleHotelEntity> { list = newlist };
        }

        private string LogoUrl(string brandId)
        {
            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetBrandUrl + brandId;
            var brand = requestApiService.Get(requestHotelUrl).JsonStringToObj<CoreSysBrand>();
            if (brand != null)
            {
                return constantSystemConfigManager.ResourceSiteAddress + brand.Logo;
            }
            return "";
        }

        /// <summary>
        /// 获取设备启动配置
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private StartResponse GetAppStartConfig(RequestHeader header, string host, bool isOldVersion, Func<string, string, object> GetTemplateAction, Func<HotelEntity, object> GetHotelEntityAction)
        {

            var rst = new StartResponse();
            try
            {
                if (string.IsNullOrWhiteSpace(header.Ver)) throw new ApiException("错误 - 版本(ver)未定义");
                int status = 0;

                //登录信息写入数据库
                String tvkey;
                var traces = traceManager.LogDeviceTrace(header, out status, out tvkey);

                if (status == -1)
                {
                    throw new ApiException("此版本非法，不允许访问系统");
                }
                logManager.SaveInfo("GetAppStartConfig - header", header.ToJsonString(), AppType.TV);

                //载入该设备对应的酒店以及房间数据
                var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + header.HotelID;//
                var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();
                rst.TemplateContent = GetTemplateAction(hotel.TemplateId, constantSystemConfigManager.AppCenterUrl + Constant.GetTemplateUrl);
                Deassign(hotel.BrandId, isOldVersion, rst);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="isOldVersion"></param>
        /// <param name="rst"></param>
        private void Deassign(string brandId, bool isOldVersion, StartResponse rst)
        {
            string content = rst.TemplateContent.ToString();
            if (!content.Contains("{}") && !isOldVersion)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        private object GetBrandTemplateId(string brandId)
        {

            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetBrandUrl + brandId;//
            var brand = requestApiService.Get(requestHotelUrl).JsonStringToObj<CoreSysBrand>();
            return brand;
        }

    }
}
