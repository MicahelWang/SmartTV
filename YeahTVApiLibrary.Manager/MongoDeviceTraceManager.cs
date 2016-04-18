using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Manager
{
    public class MongoDeviceTraceManager : IMongoDeviceTraceManager
    {
        private readonly IDeviceTraceLibraryManager _deviceTraceLibraryManager;
        private readonly IMongoDeviceTraceRepository _mongoDeviceTraceRepository;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IConstantSystemConfigManager _constantSystemConfigManager;

        public MongoDeviceTraceManager(IMongoDeviceTraceRepository mongoDeviceTraceRepository,
            IRedisCacheService redisCacheService, IDeviceTraceLibraryManager deviceTraceLibraryManager
            , IConstantSystemConfigManager constantSystemConfigManager)
        {
            _mongoDeviceTraceRepository = mongoDeviceTraceRepository;
            _redisCacheService = redisCacheService;
            _deviceTraceLibraryManager = deviceTraceLibraryManager;
            _constantSystemConfigManager = constantSystemConfigManager;
        }

        #region Redis

        private List<HotelStartPercentage> GetStatisticsInfoFromCache()
        {
            return _redisCacheService.GetAllFromCache(RedisKey.DashBoard_HotelStartPercentage, () => GetHotelStartPercentage());
        }
        private void AddRangToCache(List<HotelStartPercentage> items)
        {
            items.AsParallel()
                .ForAll(
                    m =>
                        _redisCacheService.AddItemToSet(RedisKey.DashBoard_HotelStartPercentage, m,
                            () => GetHotelStartPercentage()));
        }

        #endregion

        public List<HotelStartPercentage> GetHotelStartPercentage(DashboardCriteria criteria)
        {
            var resultList = new List<HotelStartPercentage>();
            var queryable = GetStatisticsInfoFromCache().AsQueryable();
            if (criteria.VisitTimeBegin.HasValue && criteria.VisitTimeEnd.HasValue)
                queryable =
                    queryable.Where(
                        m => m.Date.Date >= criteria.VisitTimeBegin.Value.Date && m.Date.Date <= criteria.VisitTimeEnd.Value.Date);
            if (!string.IsNullOrWhiteSpace(criteria.HotelId))
                queryable = queryable.Where(m => m.HotelId.Equals(criteria.HotelId));

            resultList.AddRange(queryable.ToList());


            if (criteria.VisitTimeBegin.HasValue && criteria.VisitTimeEnd.HasValue)
            {
                var currentDate = criteria.VisitTimeBegin.Value;
                while (currentDate <= criteria.VisitTimeEnd.Value.Date)
                {
                    if (resultList.All(m => m.Date.Date != currentDate.Date))
                    {
                        resultList.Add(new HotelStartPercentage()
                        {
                            Date = currentDate.Date,
                            ActiveCount = 0,
                            DeviceCount = 0,
                            HotelId = criteria.HotelId,
                            StartPercentageOfDay = 0d

                        });
                    }
                    currentDate = currentDate.Date.AddDays(1);
                }
            }


            return resultList;
        }

        public void RefreshHotelStartPercentage()
        {
            var startPercentage = GetStatisticsInfoFromCache();

            var lastElement = startPercentage.Select(m => m.Date).OrderByDescending(m => m).FirstOrDefault();
            int? days = int.Parse((DateTime.Now.AddDays(-1).Date - lastElement.Date).TotalDays.ToString());

            AddRangToCache(GetHotelStartPercentage(days));
        }

        private List<HotelStartPercentage> GetHotelStartPercentage(int? days = null)
        {
            var hotelStartPercentage = new List<HotelStartPercentage>();
            var date = DateTime.Now;
            var yesterday = date.AddDays(-1);

            var deviceTraceFirstVistTime = _deviceTraceLibraryManager.Search(new DeviceTraceCriteria());

            var hotels = deviceTraceFirstVistTime.Select(m => m.HotelId).Distinct();

            var criteria = new MongoDeviceCriteria();

            if (days.HasValue)
            {
                criteria.VisitTimeBegin = date.AddDays(-days.Value).Date;
                criteria.VisitTimeEnd = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);
            }
            else
            {
                criteria.VisitTimeBegin = date.AddDays(-_constantSystemConfigManager.DashBoardValidityDays).Date;
                criteria.VisitTimeEnd = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);
            }


            var deviceTraces = _mongoDeviceTraceRepository.Search(criteria);
            if (deviceTraces == null)
                deviceTraces = new List<MongoDeviceTrace>();


            DateTime currentDay = days.HasValue ? date.AddDays(-days.Value).Date : (deviceTraces.Count == 0 ? criteria.VisitTimeBegin.Value : deviceTraces.Select(m => m.VisitTime).OrderBy(m => m).First().Date);

            var deviceDistinct = deviceTraces.Select(m => new { m.HotelId, m.DeviceSeries, VisitTime = m.VisitTime.Date }).Distinct();

            while (currentDay < date.Date)
            {
                hotels.ToList().ForEach(hotel =>
                {
                    var activeCount = deviceDistinct.Count(m => m.HotelId.Equals(hotel) && m.VisitTime.Date == currentDay.Date);
                    var deviceCount = deviceTraceFirstVistTime.Count(m => m.HotelId.Equals(hotel) && m.FirstVisitTime.Date < currentDay.AddDays(1).Date);
                    hotelStartPercentage.Add(new HotelStartPercentage
                    {
                        Date = currentDay,
                        DeviceCount = deviceCount,
                        HotelId = hotel,
                        ActiveCount = activeCount,
                        StartPercentageOfDay = (deviceCount == 0 ? 0 : Math.Round((activeCount * 100.00 / deviceCount), 2))
                    });
                });
                currentDay = currentDay.AddDays(1);
            }

            return hotelStartPercentage;
        }
    }
}