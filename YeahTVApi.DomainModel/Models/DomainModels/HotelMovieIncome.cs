using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class HotelMovieIncome
    {
        public string OrderId { get; set; }
        public string HotelId { get; set; }
        public string MovieId { get; set; }
        public string MovieName { get; set; }
        public decimal Income { get; set; }
        public DateTime Date { get; set; }
        public PayType PayType { get; set; }
    }
    public class HotelMovieDailyIncome
    {
        public decimal Income { get; set; }
        public DateTime Date { get; set; }
    }

    public class HotelMovieIncomeRanking
    {
        public string MovieId { get; set; }
        public string MovieName { get; set; }
        public decimal Income { get; set; }
    }
}
