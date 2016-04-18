namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity;
    using System.Collections.Generic;

    public interface IGetGuestInfoService
    {
        Guest GetInfo(string HotelId, string RoomNo);

        List<Guest> GetInfoList(string HotelId, string RoomNo);

        string GetMemberLevelName(string memberLevel);

        string NextLevelHint(string memberID);

        bool MobileIsMember(string mobile);
    }
}
