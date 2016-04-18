namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity;
    using System;
    using System.Collections.Generic;

    public interface IRoomManagerService
    {
        List<RoomInfo> GetRoomList(String hotelID, String statues, String types, String roomNum, String sortBy = "ROOMASC");

        RoomInfo ModifyRoomStatus(String hotelID, String RoomID, String OperatorID, String status, out String message);
    }
}

