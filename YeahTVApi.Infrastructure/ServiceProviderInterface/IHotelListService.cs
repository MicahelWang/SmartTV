namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity;
    using System;
    using System.Collections.Generic;

    public interface IHotelListService : ICentralGetwayServiceBase
    {
        List<Hotel> Query(List<string> HotelIDs, String CheckInDate, String CheckOutDate);
    }
}
