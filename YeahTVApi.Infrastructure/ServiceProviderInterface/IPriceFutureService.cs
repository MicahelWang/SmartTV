namespace YeahTVApi.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public interface IPriceFutureService : ICentralGetwayServiceBase
    {
        Dictionary<DateTime, decimal> GetFutureOrderPrice(
            string hotelId, 
            string vNumber, 
            string receiveOrderId, 
            int continueDays, 
            bool isHalfDay, 
            string nationalId);
    }
}
