namespace HZTVApi.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using HZTVApi.Common;
    using HZTVApi.Entity;
    using HZTVApi.Infrastructure;
    using Newtonsoft.Json;

    /// <summary>
    /// 酒店数据管理对象
    /// </summary>
    public class HotelManager : IHotelManager
    {
        private ITVTraceManager traceEFMagager;
        private IRedisCacheManager redisCacheManager;

        public HotelManager(
            ITVTraceManager traceEFMagager,
            IRedisCacheManager redisCacheManager)
        {
            this.traceEFMagager = traceEFMagager;
            this.redisCacheManager = redisCacheManager;
        }

        public String GetWeatherData(String CityName)
        {
            return redisCacheManager.GetCache(CityName);
        }

        public Hotel QueryHotel(RequestHeader header)
        {
            return QueryHotel(traceEFMagager.GetHotelID(header));
        }

        public Hotel QueryHotel(String HotelID)
        {
            var hotel = new Hotel();
            var json = redisCacheManager.GetCache(Constant.HtotelCacheKey + HotelID);

            hotel = string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<Hotel>(json);
          
            return hotel;
        }
    }
}
