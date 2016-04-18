namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity;
    using System.Collections.Generic;

    public interface ICheckInService
    {
        List<CardData> GetCardData(string hotelId, string roomNumber, string CardSNO, List<Guest> guests);
    }
}
