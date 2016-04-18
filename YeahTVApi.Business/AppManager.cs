namespace HZTVApi.Business
{
    using HZTVApi.Common;
    using HZTVApi.DomainModel;
    using HZTVApi.DomainModel.Mapping;
    using HZTVApi.DomainModel.Models;
    using HZTVApi.DomainModel.SearchCriteria;
    using HZTVApi.Entity;
    using HZTVApi.Filter;
    using HZTVApi.Infrastructure;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class AppManager : IAppManager
    {
        private IHotelManager hotelManager;
        private ITVModelRepertory modelRepertory;
        private IRedisCacheManager redisCacheManager;
        private ITVTraceRepertory tVTraceRepertory;
        private ITVAppsRepertory appsRepertory;

        public AppManager(
            IHotelManager hotelManager,
            ITVModelRepertory modelRepertory,
            IRedisCacheManager redisCacheManager,
            ITVTraceRepertory tVTraceRepertory,
            ITVAppsRepertory appsRepertory)
        {
            this.hotelManager = hotelManager;
            this.modelRepertory = modelRepertory;
            this.redisCacheManager = redisCacheManager;
            this.tVTraceRepertory = tVTraceRepertory;
            this.appsRepertory = appsRepertory;
        }

        /// <summary>
        /// 获取设备跟踪数据
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public TVTrace GetTrace(RequestHeader header)
        {
            return tVTraceRepertory.Search(new TVTraceModelCriteria { DeviceSeries = header.DEVNO }).SingleOrDefault();
                }

        /// <summary>
        /// 获取TV所有版本列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, TVApps> GetTVVersionList()
        {
            var list = new Dictionary<string, TVApps>();

            appsRepertory.Search(new TVAppsCriteria { Active = true }).ForEach(a =>
            {
                list.Add(a.Id, a);
            });

            return list;
        }

        /// <summary>
        /// 获取设备启动配置
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public StartResponse GetAppStartConfig(BaseRequestData bd, RequestHeader header, String host)
        {
            var modelId = tVTraceRepertory.Search(new TVTraceModelCriteria { DeviceSeries = header.DEVNO }).FirstOrDefault().ModelId;

            StartResponse rst = new StartResponse();
            try
            {
                if (string.IsNullOrWhiteSpace(header.Ver)) throw new ApiException("错误 - 版本(ver)未定义");
                int status = 1;

                //登录信息写入数据库
                String tv_key;
                var dataset = apiDBManager.LogDeviceTrace(header, out status, out tv_key);

                if (status == -3 || status == -4)
                {
                    throw new ApiException("该设备(" + header.DEVNO + ")没有相关应用的授权");
                }

                if (status != 0)
                {
                    throw new ApiException("此版本非法，不允许访问系统");
                }

                apiDBManager.RefreshDeviceHotel(header);

                //载入该设备对应的酒店以及房间数据
                Hotel hotel = hotelManager.QueryHotel(apiDBManager.GetHotelID(header));

                //生成这家店的界面展现
                rst.HotelName = hotel.hotelName;
                rst.GeoInfo = hotel.geoInfo;
                rst.BrandCode = hotel.hotelStyle;
                rst.BrandName = hotel.hotelStyleName;
                rst.CityName = hotel.cityName;
                rst.SecureKey = tv_key;
                var modelId = tVTraceRepertory.Search(new TVTraceModelCriteria{ DeviceSeries = header.DEVNO }).FirstOrDefault().ModelId;
                var deviceColumnMembersCaches = GetAllColumnMembersCacheModels().Where(m=>m.TVModelId == modelId);

                var list = new List<double>();
                deviceColumnMembersCaches
                    .GroupBy(g => g.TVModelColumnMemberModelColumnId).ForEach(f=>list.Add(f.FirstOrDefault().TVModelColumnWeight.Value));

                rst.ModelList = deviceColumnMembersCaches.ToList().ToModelEntities(LanguageType.Default, host, hotel.hotelID, header);
                rst.EnModelList = deviceColumnMembersCaches.ToList().ToModelEntities(LanguageType.English, host, hotel.hotelID, header);
                
                rst.ColumnWeight = list.ToArray();
                var httpHost = PubFun.ChangetHttpsToHttps(host);

                //添加配置参数
                rst.ConfigData = new Dictionary<string, string>();
                foreach (DataRow drConfig in dataset.Tables[0].Rows)
                {
                    string key = PubFun.ConvertToStr(drConfig["DictCode"]);
                    string val = PubFun.ConvertToStr(drConfig["DictValue"]);
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(val))
                        continue;
                    if (val.IndexOf("{host}") > -1)
                        val = val.Replace("{host}", host);
                    if (val.IndexOf("{httpHost}") > -1)
                        val = val.Replace("{httpHost}", httpHost);
                    if (key == "appUpdateUrl")
                        rst.NewVersionURL = val;
                    else
                    {
                        rst.ConfigData.Add(key, val);
                    }

                }
                //添加当前系统时间
                rst.ConfigData.Add("SYSTEM_TIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception err)
            {
                HTOutputLog.SaveError(err.ToString(), err, null);
                throw err;
            }

            return rst;
        }

        [RedisCacheHandler(Key = Constant.ColumnMembersCacheModelKey, Type = typeof(List<ColumnMembersCacheModel>))]
        private List<ColumnMembersCacheModel> GetAllColumnMembersCacheModels()
        {
            var columnMembers = new List<TVModelColumnMember>();
            var models = modelRepertory.GetAllModelsIncludeModelColumn().Select(m=>m.TVModelColumns.Select(v=>v.TVModelColumnMembers));

            models.ToList().ForEach(m => m.ToList().ForEach(c => columnMembers.AddRange(c)));
            return columnMembers.ToMapColumnMembersCacheModels();
        }
    }
}
