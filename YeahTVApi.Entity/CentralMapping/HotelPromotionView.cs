using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class HotelPromotionView
    {
        public int ID { get; set; }
        public int ShowLocationID { get; set; }
        public string Title { get; set; }
        public string Descript { get; set; }
        public string ImgName { get; set; }
        public string Link { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFullTime { get; set; }
        public bool IsNational { get; set; }
        public string CityCode { get; set; }
        public List<string> HotelIdList { get; set; }
        public bool IsShowIndex { get; set; }
        public bool IsShowLeftColumn { get; set; }
        public bool IsShowResvSuccess { get; set; }
        public bool InUse { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime OnlineDate { get; set; }
        public string Type { get; set; }
        public string BrandShow { get; set; }
        public bool IsShowbrandDefault { get; set; }
        public string AppDescript { get; set; }
        public string AppImgBanner { get; set; }
        public string AppLink { get; set; }
        public bool? AppIsShowInTopNews { get; set; }
        public string AppCityDescript { get; set; }
    }

    //public static class HotelPromotionViewExtension
    //{
    //    public static HotelPromotion ToHotelPromotion(this HotelPromotionView promotionView)
    //    {
    //        var hotelPromotion = new HotelPromotion()
    //            {
    //                ID = promotionView.ID,
    //                ShowLocationID = promotionView.ShowLocationID,
    //                Title = promotionView.Title,
    //                Descript = promotionView.Descript,
    //                ImgName = promotionView.ImgName,
    //                Link = promotionView.Link,
    //                StartDate = promotionView.StartDate,
    //                EndDate = promotionView.EndDate,
    //                IsFullTime = promotionView.IsFullTime,
    //                IsNational = promotionView.IsNational,
    //                CityCode = promotionView.CityCode,
    //                IsShowIndex = promotionView.IsShowIndex,
    //                IsShowLeftColumn = promotionView.IsShowLeftColumn,
    //                IsShowResvSuccess = promotionView.IsShowResvSuccess,
    //                InUse = promotionView.InUse,
    //                OrderTime = promotionView.OrderTime,
    //                OnlineDate = promotionView.OnlineDate,
    //                Type = promotionView.Type,
    //                BrandShow = promotionView.BrandShow,
    //                IsShowbrandDefault = promotionView.IsShowbrandDefault,
    //                AppDescript = promotionView.AppDescript,
    //                AppImgBanner = promotionView.AppImgBanner,
    //                AppLink = promotionView.AppLink,
    //                AppIsShowInTopNews = promotionView.AppIsShowInTopNews,
    //                AppCityDescript = promotionView.AppCityDescript
    //            };

    //        return hotelPromotion;
    //    }
    //}
}
