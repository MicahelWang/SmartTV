namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity;
    using YeahTVApi.Entity.CentralMapping;
    using System.Collections.Generic;

    public interface ISelfServiceService : ICentralGetwayServiceBase
    {
       bool GetReceiveOrderRequestResult(string hotelId, string vNumber, string receiveOrderId, int continueDays,
            out string message);

        bool GetReceiveOrderSelfCheckout(string hotelId, string vNumber, string receiveOrderId, out string message);

        bool IsCheckOutByReceiveID(string sReceiveID);
    }
}
