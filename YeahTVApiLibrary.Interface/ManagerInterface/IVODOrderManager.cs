using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IVODOrderManager
    {
        List<VODOrder> SearchOrders(VODOrderCriteria orderCriteria);
        List<VODOrder> SearchNotExpiresOrders(PayType payType);
        List<VODOrder> GetOrdersFromCache();
        void UpdateVODOrder(VODOrder vOdOrder);
        void UpdateOrderCache();
        void UpdateDailyOrderCache();
        void Add(VODOrder vOdOrder);
        VODOrder PayMentCallBack(PaymentInfo paymentInfo, PaymentCallBackData returnInfo);
        VODOrder GetSuccessOrderByMovieId(RequestHeader header, string movieId);
        VODOrder GetSuccessDailyOrder(RequestHeader header);

        VODOrder CreateNewOrder(TVVODRequest tvVODRequest, HotelMovieTraceNoTemplate movieInfo,
            HotelPayment hotelPayment, RequestHeader header, OrderState orderState = OrderState.Unpaid);

        List<HotelMovieDailyIncome> GetHotelMovieDailyIncome(DashboardCriteria criteria);
        List<HotelMovieIncomeRanking> GetHotelMovieIncomeRanking(DashboardCriteria criteria);
        List<KeyValuePair<string, decimal>> GetHotelsMovieIncomeRanking(DashboardCriteria criteria);
        void RefreshHotelMovieIncome();

        decimal GetScoresbyOderId(string OrderId);

        string GetPorductId(VODOrder vOdOrder);
        void UpdateOrderCache(VODOrder order);
        VODOrder OrderPaySuccess(PaymentInfo paymentInfo, PaymentCallBackData paymentCallBackData);
        VODOrder GetScoreOrderInfo(string orderId);
        int GetIntScoresbyOderId(string OrderId);
        string GetScoreOrderId(string orderId, string hotelId);
    }
}
