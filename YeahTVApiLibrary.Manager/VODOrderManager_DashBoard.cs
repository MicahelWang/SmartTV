using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Entity;

namespace YeahTVApiLibrary.Manager
{

    public partial class VODOrderManager
    {
        #region Redis

        private List<HotelMovieIncome> GetStatisticsInfoFromCache()
        {
            return redisCacheService.GetAllFromCache(RedisKey.DashBoard_HotelMovieIncome, () => GetHotelMovieIncome());
        }
        private void AddRangToCache(List<HotelMovieIncome> items)
        {
            items.AsParallel()
                .ForAll(
                    m =>
                        redisCacheService.AddItemToSet(RedisKey.DashBoard_HotelMovieIncome, m,
                            () => GetHotelMovieIncome()));
        }

        #endregion

        public List<HotelMovieDailyIncome> GetHotelMovieDailyIncome(DashboardCriteria criteria)
        {
            var resultList = new List<HotelMovieDailyIncome>();

            var hotelMovieIncome = GetHotelMovieIncome(criteria);
            var query = from income in hotelMovieIncome
                        group income by income.Date.Date
                            into g
                            select new HotelMovieDailyIncome { Income = g.Sum(m => m.Income), Date = g.Key };

            resultList.AddRange(query.ToList());

            if (criteria.VisitTimeBegin.HasValue && criteria.VisitTimeEnd.HasValue)
            {
                var currentDate = criteria.VisitTimeBegin.Value;
                while (currentDate <= criteria.VisitTimeEnd.Value.Date)
                {
                    if (resultList.All(m => m.Date.Date != currentDate.Date))
                    {
                        resultList.Add(new HotelMovieDailyIncome()
                        {
                            Date = currentDate.Date,
                            Income = 0
                        });
                    }
                    currentDate = currentDate.Date.AddDays(1);
                }
            }

            return resultList;
        }

        public List<HotelMovieIncomeRanking> GetHotelMovieIncomeRanking(DashboardCriteria criteria)
        {
            var hotelMovieIncome = GetHotelMovieIncome(criteria).Where(o => o.PayType == PayType.Movie);
            var query = from income in hotelMovieIncome
                        group income by new { income.MovieId, income.MovieName }
                            into g
                            orderby g.Sum(m => m.Income) ascending
                            select new HotelMovieIncomeRanking { Income = g.Sum(m => m.Income), MovieId = g.Key.MovieId, MovieName = g.Key.MovieName };

            return query.OrderByDescending(m=>m.Income).Take(10).ToList();
        }
        public List<KeyValuePair<string, decimal>> GetHotelsMovieIncomeRanking(DashboardCriteria criteria)
        {
            var hotelMovieIncome = GetHotelMovieIncome(criteria);
            var query = from income in hotelMovieIncome
                        group income by income.HotelId
                            into g
                            orderby g.Sum(m => m.Income) ascending
                            select new KeyValuePair<string, decimal>(g.Key, g.Sum(m => m.Income));

            Dictionary<string, decimal> dic = new Dictionary<string, decimal>();
            query.ToList().ForEach(q => dic.Add(q.Key, q.Value));
            return dic.OrderByDescending(m => m.Value).Take(10).ToList();
        }

        public void RefreshHotelMovieIncome()
        {
            var startPercentage = GetStatisticsInfoFromCache();

            var lastElement = startPercentage.Select(m => m.Date).OrderByDescending(m => m).FirstOrDefault();
            int? days = int.Parse((DateTime.Now.AddDays(-1).Date - lastElement.Date).TotalDays.ToString());

            AddRangToCache(GetHotelMovieIncome(days));
        }

        private List<HotelMovieIncome> GetHotelMovieIncome(DashboardCriteria criteria)
        {
            var queryable = GetStatisticsInfoFromCache().AsQueryable();
            if (criteria.VisitTimeBegin.HasValue && criteria.VisitTimeEnd.HasValue)
                queryable =
                    queryable.Where(
                        m => m.Date >= criteria.VisitTimeBegin.Value && m.Date <= criteria.VisitTimeEnd.Value);
            if (!string.IsNullOrWhiteSpace(criteria.HotelId))
                queryable = queryable.Where(m => m.HotelId.Equals(criteria.HotelId));
            if (criteria.HotelList != null)
            {
                queryable = queryable.Where(m => criteria.HotelList.Contains(m.HotelId));
            }
            return queryable.ToList();
        }

        private List<HotelMovieIncome> GetHotelMovieIncome(int? days = null)
        {
            var hotelMovieIncome = new List<HotelMovieIncome>();
            var date = DateTime.Now;
            var yesterday = date.AddDays(-1);

            var criteria = new VODOrderCriteria() { orderState = OrderState.Success };


            if (days.HasValue)
            {
                criteria.CompleteBeginTime = date.AddDays(-days.Value).Date;
                criteria.CompleteEndTime = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);
            }
            else
            {
                criteria.CompleteBeginTime = date.AddDays(-constantSystemConfigManager.DashBoardValidityDays).Date;
                criteria.CompleteEndTime = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);
            }


            var orders = vodOrderRepertory.Search(criteria) ?? new List<VODOrder>();

            var dateTime = orders.Select(m => m.CompleteTime).OrderBy(m => m).FirstOrDefault();
            if (dateTime != null)
            {
                orders.ForEach(order =>
                    {
                        if (order.CompleteTime != null)
                            hotelMovieIncome.Add(new HotelMovieIncome
                            {
                                OrderId = order.Id,
                                Date = order.CompleteTime.Value,
                                MovieId = order.MovieId,
                                Income = order.Price,
                                MovieName = order.GoodsName,
                                HotelId = order.HotelId,
                                PayType = string.IsNullOrWhiteSpace(order.PayType) || order.PayType.ToLower() == PayType.Movie.ToString().ToLower() ? PayType.Movie : PayType.Daily
                            });
                    });
            }

            return hotelMovieIncome;
        }
    }
}
