namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity.CentralMapping;

    public interface IPriceService : ICentralGetwayServiceBase
    {
        ReceiveOrders GetOrderPrice(string hotelId, string vNumber, string receiveOrderId);
        ReceiveOrders GetOrderPriceByRoomId(string hotelId, string roomId, string receiveOrderId);
    }
}
