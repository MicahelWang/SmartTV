using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using System.Linq;
using System;
using System.Collections.Concurrent;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.Common;

namespace YeahCentre.Manager
{
    public partial class BehaviorLogManager
    {

        #region Redis

        private List<T> GetStatisticsInfoFromCache<T>(string key, Func<List<T>> getListFun)
        {
            return _redisCacheService.GetAllFromCache(key, getListFun);
        }
        private void AddRangToCache<T>(string key, List<T> items, Func<List<T>> getListFun)
        {
            items.AsParallel().ForAll(m => _redisCacheService.AddItemToSet(key, m, getListFun));
        }


        #endregion

        #region RefreshBehaviorLogDashBoard

        public void RefreshBehaviorLogDashBoard()
        {
            RefreshHotelModuleUsedTime();
            //RefreshHotelMovieVodOfDay();
            //RefreshHotelChannelUsedTime();
        }

        #endregion

        #region DashBoard_HotelModuleUsedTime

        public Dictionary<string, double> GetHotelModuleUsedTime(DashboardCriteria criteria)
        {
            var queryable = GetStatisticsInfoFromCache(RedisKey.DashBoard_HotelModuleUsedTime, () => GetHotelModuleUsedTime()).AsQueryable();
            if (criteria.VisitTimeBegin.HasValue && criteria.VisitTimeEnd.HasValue)
                queryable =
                    queryable.Where(
                        m => m.Date >= criteria.VisitTimeBegin.Value.Date && m.Date <= criteria.VisitTimeEnd.Value.Date);
            if (!string.IsNullOrWhiteSpace(criteria.HotelId))
                queryable = queryable.Where(m => m.HotelId.Equals(criteria.HotelId));

            var usedTime = new Dictionary<string, double>();
            
            queryable.SelectMany(m => m.ModuleUsedTimeItems)
                .GroupBy(m => m.ModuleName)
                .ToList().ForEach(m => usedTime.Add(m.Key,Math.Round(m.Sum(item => item.UsedTime),2)));

            return usedTime;
        }
        public void RefreshHotelModuleUsedTime()
        {
            var startPercentage = GetStatisticsInfoFromCache(RedisKey.DashBoard_HotelModuleUsedTime, () => GetHotelModuleUsedTime());

            var lastElement = startPercentage.Select(m => m.Date).OrderByDescending(m => m).FirstOrDefault();
            int? days = int.Parse((DateTime.Now.AddDays(-1).Date - lastElement.Date).TotalDays.ToString());

            AddRangToCache(RedisKey.DashBoard_HotelModuleUsedTime, GetHotelModuleUsedTime(days), () => GetHotelModuleUsedTime());
        }

        private List<HotelModuleUsedTime> GetHotelModuleUsedTime(int? days = null)
        {
            var hotelMovieVodOfDays = new List<HotelModuleUsedTime>();
            var date = DateTime.Now;
            var yesterday = date.AddDays(-1);

            var criteria = new LogCriteria() { BehaviorType = BehaviorType.ModuleUsed };

    
            if (days.HasValue)
            {
                criteria.CompleteBeginTime = date.AddDays(-days.Value).Date;
                criteria.CompleteEndTime = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);
            }
            else
            {
                criteria.CompleteBeginTime = date.AddDays(-_constantSystemConfigManager.DashBoardValidityDays).Date;
                criteria.CompleteEndTime = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);
            }

            var logs = _repertory.Search(criteria) ?? new List<BehaviorLog>();
            var moduleUsedTimeItems = new Dictionary<BehaviorLog, ModuleUsedTimeItem>();

            logs.ForEach(log =>
            {
                var moduleUsedTimeItem = log.BehaviorInfo.ToLower().JsonStringToObj<ModuleUsedTimeItem>();
                if (moduleUsedTimeItem != null)
                {
                    moduleUsedTimeItems.Add(log, moduleUsedTimeItem);
                }
            });


            moduleUsedTimeItems.GroupBy(m => new { m.Key.HotelId, m.Key.CreateTime.Date })
                .ToList()
                .ForEach(m =>
                {
                    hotelMovieVodOfDays.Add(new HotelModuleUsedTime
                    {
                        ModuleUsedTimeItems = m.Select(item => item.Value).GroupBy(item => item.ModuleName).Select(moduleUsedTimeItem => new ModuleUsedTimeItem() { ModuleName = moduleUsedTimeItem.Key, UsedTime =Math.Round(moduleUsedTimeItem.Sum(timeItem => timeItem.UsedTime)/ 1000 / 60 / 60, 2) }).ToList(),
                        HotelId = m.Key.HotelId,
                        Date = m.Key.Date
                    });
                });

            return hotelMovieVodOfDays;
        }

        #endregion

        #region DashBoard_HotelMovieVodOfDay

        public Dictionary<string, double> GetHotelMovieVodOfDay(DashboardCriteria criteria)
        {
            var queryable = GetStatisticsInfoFromCache(RedisKey.DashBoard_HotelMovieVodOfDay, () => GetHotelMovieVodOfDay()).AsQueryable();
            if (criteria.VisitTimeBegin.HasValue && criteria.VisitTimeEnd.HasValue)
                queryable =
                    queryable.Where(
                        m => m.Date >= criteria.VisitTimeBegin.Value.Date && m.Date <= criteria.VisitTimeEnd.Value.Date);

            if (!string.IsNullOrWhiteSpace(criteria.HotelId))
                queryable = queryable.Where(m => m.HotelId.Equals(criteria.HotelId));

            if (criteria.HotelList != null)
            {
                queryable = queryable.Where(m => criteria.HotelList.Contains(m.HotelId));
            }

            var usedTime = new Dictionary<string, double>();

            queryable.GroupBy(m => m.MovieId)
                .OrderByDescending(m => m.Sum(s => s.VodCount))
                .Take(10)
                .Select(m => new { m.Key, Sum = m.Sum(s => s.VodCount) }).ToList().ForEach(m => usedTime.Add(m.Key, m.Sum));

            return usedTime;
        }
        public void RefreshHotelMovieVodOfDay()
        {
            var startPercentage = GetStatisticsInfoFromCache(RedisKey.DashBoard_HotelMovieVodOfDay, () => GetHotelMovieVodOfDay());

            var lastElement = startPercentage.Select(m => m.Date).OrderByDescending(m => m).FirstOrDefault();
            int? days = int.Parse((DateTime.Now.AddDays(-1).Date - lastElement.Date).TotalDays.ToString());

            AddRangToCache(RedisKey.DashBoard_HotelMovieVodOfDay, GetHotelMovieVodOfDay(days), () => GetHotelMovieVodOfDay());
        }
        private List<HotelMovieVodOfDay> GetHotelMovieVodOfDay(int? days = null)
        {
            var hotelMovieVodOfDays = new List<HotelMovieVodOfDay>();
            var date = DateTime.Now;

            var criteria = new LogCriteria() { BehaviorType = BehaviorType.MovieVod };

            if (days.HasValue)
            {
                criteria.CompleteBeginTime = date.AddDays(days.Value).Date;
                criteria.CompleteEndTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }

            var logs = _repertory.Search(criteria) ?? new List<BehaviorLog>();
            var movieVodDic = new Dictionary<BehaviorLog, MovieVodItem>();

            logs.ForEach(log =>
            {
                var movieVodItem = log.BehaviorInfo.ToLower().JsonStringToObj<MovieVodItem>();
                if (movieVodItem != null)
                {
                    movieVodDic.Add(log, movieVodItem);
                }
            });

            movieVodDic.GroupBy(m => new { m.Key.HotelId, m.Key.CreateTime.Date, m.Value.MovieId })
                .ToList()
                .ForEach(m =>
                {
                    hotelMovieVodOfDays.Add(new HotelMovieVodOfDay
                    {
                        MovieId = m.Key.MovieId,
                        MovieName = m.Key.MovieId,
                        HotelId = m.Key.HotelId,
                        Date = m.Key.Date,
                        VodCount = m.Count()
                    });
                });

            return hotelMovieVodOfDays;
        }

        #endregion

        #region DashBoard_HotelChannelUsedTime

        public Dictionary<string, double> GetHotelChannelUsedTime(DashboardCriteria criteria)
        {
            var queryable = GetStatisticsInfoFromCache(RedisKey.DashBoard_HotelChannelUsedTime, () => GetHotelChannelUsedTime()).AsQueryable();
            if (criteria.VisitTimeBegin.HasValue && criteria.VisitTimeEnd.HasValue)
                queryable =
                    queryable.Where(
                        m => m.Date >= criteria.VisitTimeBegin.Value.Date && m.Date <= criteria.VisitTimeEnd.Value.Date);

            if (!string.IsNullOrWhiteSpace(criteria.HotelId))
                queryable = queryable.Where(m => m.HotelId.Equals(criteria.HotelId));

            var usedTime = new Dictionary<string, double>();

            queryable.GroupBy(m => m.ChannelId)
                .OrderByDescending(m => m.Sum(s => s.UsedTime))
                .Take(10)
                .Select(m => new { m.Key, Sum = m.Sum(s => s.UsedTime) }).ToList().ForEach(m => usedTime.Add(m.Key, Math.Round(m.Sum / 1000 / 60 / 60, 2)));

            return usedTime;
        }
        public void RefreshHotelChannelUsedTime()
        {
            var startPercentage = GetStatisticsInfoFromCache(RedisKey.DashBoard_HotelChannelUsedTime, () => GetHotelChannelUsedTime());

            var lastElement = startPercentage.Select(m => m.Date).OrderByDescending(m => m).FirstOrDefault();
            int? days = int.Parse((DateTime.Now.AddDays(-1).Date - lastElement.Date).TotalDays.ToString());

            AddRangToCache(RedisKey.DashBoard_HotelChannelUsedTime, GetHotelChannelUsedTime(days), () => GetHotelChannelUsedTime());
        }


        private List<HotelChannelUsedTime> GetHotelChannelUsedTime(int? days = null)
        {
            var hotelChannelUsedTime = new List<HotelChannelUsedTime>();
            var date = DateTime.Now;

            var criteria = new LogCriteria() { BehaviorType = BehaviorType.ChannelUsed };

            if (days.HasValue)
            {
                criteria.CompleteBeginTime = date.AddDays(days.Value).Date;
                criteria.CompleteEndTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
            }

            var logs = _repertory.Search(criteria) ?? new List<BehaviorLog>();
            var channelUsedItems = new Dictionary<BehaviorLog, ChannelUsedItem>();

            logs.ForEach(log =>
            {
                var channelUsedItem = log.BehaviorInfo.ToLower().JsonStringToObj<ChannelUsedItem>();
                if (channelUsedItem != null)
                {
                    channelUsedItems.Add(log, channelUsedItem);
                }
            });

            channelUsedItems.GroupBy(m => new { m.Key.HotelId, m.Key.CreateTime.Date, m.Value.ChannelId })
                .ToList()
                .ForEach(m =>
                {
                    hotelChannelUsedTime.Add(new HotelChannelUsedTime
                    {
                        ChannelId = m.Key.ChannelId,
                        UsedTime = m.Sum(used => used.Value.UsedTime),
                        HotelId = m.Key.HotelId,
                        Date = m.Key.Date
                    });
                });

            return hotelChannelUsedTime;
        }

        #endregion
    }
}