using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Manager
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Mapping;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json.Linq;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.Models.ViewModels;

    public class CacheManager : ICacheManager
    {
        private IAppsLibraryRepertory appsRepertory;
        private IRedisCacheService redisCacheService;
        private ILogManager logManager;
        private IRequestApiService requestApiService;
        private ISystemConfigManager systemConfigManager;
        private IConstantSystemConfigManager constantSystemConfigManager;

        public CacheManager(
            IRedisCacheService redisCacheService,
            IAppsLibraryRepertory appsRepertory,
            ILogManager logManager,
            IRequestApiService requestApiService,
            ISystemConfigManager systemConfigManager,
            IConstantSystemConfigManager constantSystemConfigManager)
        {
            this.redisCacheService = redisCacheService;
            this.appsRepertory = appsRepertory;
            this.logManager = logManager;
            this.requestApiService = requestApiService;
            this.systemConfigManager = systemConfigManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
        }

        public void SetWeather()
        {
            DateTime startTime = DateTime.Now;

            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl;
            var cities = requestApiService.HttpRequest(requestHotelUrl + Constant.GetCitiesUrl, "GetUseCity").JsonStringToObj<List<CoreSysCity>>();

            if (!cities.Any())
            {
                cities.Add(new CoreSysCity { Id = 73, Name = "上海" });
            }

            cities.ForEach(city =>
            {
                try
                {
                    var wReq = System.Net.WebRequest.Create(Constant.BaiduWeatherURL + city.Name);
                    // Get the response instance.
                    var wResp = wReq.GetResponse();
                    var respStream = wResp.GetResponseStream();
                    // Dim reader As StreamReader = New StreamReader(respStream)
                    using (var reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("utf-8")))
                    {
                        var data = reader.ReadToEnd();
                        var weathers = new List<WeatherDataWithPm25>();

                        if (data.ToString().Contains("\"status\":\"success\""))
                        {
                            var weatherListJson = JObject.Parse(data).GetValue("results").First["weather_data"].AsEnumerable().ToList();
                            var pm25 = JObject.Parse(data).GetValue("results").First["pm25"].ToString();

                            weatherListJson.ForEach(w =>
                            {
                                weathers.Add(new WeatherDataWithPm25
                                {
                                    Date = w["date"].ToString(),
                                    DayPictureUrl = w["dayPictureUrl"].ToString().Replace("http://api.map.baidu.com/images/weather/day/", ""),
                                    Ttemperature = w["temperature"].ToString(),
                                    Weather = w["weather"].ToString(),
                                    Wind = w["wind"].ToString(),
                                    Pm25 = pm25
                                });
                            });

                            redisCacheService.Set(Constant.SystemWeatherKey + city.Id.ToString(), weathers);
                        }
                        logManager.SaveInfo("GetWeatherData", weathers.ToJsonString(), YeahTVApi.DomainModel.Enum.AppType.TV);
                    }
                }
                catch (System.Exception ex)
                {
                    logManager.SaveError("GetWeatherData" + city, ex, YeahTVApi.DomainModel.Enum.AppType.TV);
                }
            });

            SaveFunctionTime("setCache", "redis:GetWeatherData", startTime);
        }

        public void SetAppsList()
        {
            DateTime startTime = DateTime.Now;
            var list = new Dictionary<string, Apps>();

            appsRepertory.Search(new AppsCriteria { NeedVersion = true }).ForEach(a =>
            {
                list.Add(a.Id, a);
            });

            redisCacheService.Set(Constant.AppsListKey, list);
            SaveFunctionTime("setCache", "redis:SetAppsList", startTime);
        }

        /// <summary>
        /// 记录方法访问时间
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="startTime"></param>
        public void SaveFunctionTime(string funName, string content, DateTime startTime)
        {
            DateTime StartTime = startTime;
            DateTime EndTime = DateTime.Now;
            TimeSpan span = EndTime - StartTime;
            logManager.SaveInfo(funName, content + ";耗时:" + span.TotalSeconds, AppType.TV);
        }
    }
}
