namespace HZTVApi.Manager
{
    using HZTVApi.Common;
    using HZTVApi.DomainModel.Mapping;
    using HZTVApi.DomainModel.Models;
    using HZTVApi.Entity;
    using HZTVApi.Filter;
    using HZTVApi.Infrastructure;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CacheManager : ICacheManager
    {
        private ITVModelRepertory modelRepertory;
        private ITVTraceEFMagager traceEFMagager;
        private IRedisCacheManager redisCacheManager;
        private IAppManager appManager;
        private ISystemConfigRepertory systemConfigRepertory;

        public CacheManager(
            ITVModelRepertory modelRepertory,
            ITVTraceEFMagager traceEFMagager,
            IRedisCacheManager redisCacheManager,
            IAppManager appManager,
            ISystemConfigRepertory systemConfigRepertory)
        {
            this.modelRepertory = modelRepertory;
            this.traceEFMagager = traceEFMagager;
            this.redisCacheManager = redisCacheManager;
            this.appManager = appManager;
            this.systemConfigRepertory = systemConfigRepertory;
        }

        public void SetAllColumnMembersCacheModels()
        {
            var columnMembers = new List<TVModelColumnMember>();
            var models = modelRepertory.GetAllModelsIncludeModelColumn().Select(m => m.TVModelColumns.Select(v => v.TVModelColumnMembers));

            models.ToList().ForEach(m => m.ToList().ForEach(c => columnMembers.AddRange(c)));
            redisCacheService.SetCache(Constant.ColumnMembersCacheModelKey, JsonConvert.SerializeObject(columnMembers.ToMapColumnMembersCacheModels()));
        }

        public void SetAllHotelInfo()
        {
            var hotleIds = traceEFMagager.GetTraceHotelIds();

            var hotels = hotelListService.Query(hotleIds, DateTime.Now.ToString("yyyy-MM-dd"),
               DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));

            hotels.ForEach(h =>
            {
                redisCacheService.SetCache(Constant.HtotelCacheKey + h.hotelID, JsonConvert.SerializeObject(h));
            });

            redisCacheService.SetCache(Constant.CitiesKey, JsonConvert.SerializeObject(hotels.Select(h => h.cityName).Distinct()));
        }

        public void SetGetWeather()
        {
            var cities = JsonConvert.DeserializeObject<List<string>>(redisCacheService.GetCache(Constant.CitiesKey));

            cities.ForEach(city =>
            {
                try
                {
                    System.Net.WebRequest wReq = System.Net.WebRequest.Create(Constant.BaiduWeatherURL + city);
                    // Get the response instance.
                    System.Net.WebResponse wResp = wReq.GetResponse();
                    System.IO.Stream respStream = wResp.GetResponseStream();
                    // Dim reader As StreamReader = New StreamReader(respStream)
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("utf-8")))
                    {
                        var data = reader.ReadToEnd();
                        if (data.Contains("\"status\":\"success\""))
                        {
                            redisCacheManager.SetCache(city, data);
                        }
                        HTOutputLog.SaveInfo("GetWeatherData", data);
                    }
                }
                catch (System.Exception ex)
                {
                    HTOutputLog.SaveError("GetWeatherData", ex, city);
                }
            });
        }

        public void SetGetAppsList()
        {
            var apps = appManager.GetTVVersionList();

            redisCacheManager.SetCache(Constant.AppsListKey, JsonConvert.SerializeObject(apps));
        }

        public void SetSystemConfig()
        {
            var configs = systemConfigRepertory.GetAll().Where(s=>s.Enable).ToList();

            configs.ForEach(c => 
            {
                redisCacheManager.SetCache(Constant.SystemConfigKey + c.ConfigName, c.ConfigValue);
            });
        }
    }
}
