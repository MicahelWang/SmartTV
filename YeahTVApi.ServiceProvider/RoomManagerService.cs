namespace YeahTVApi.ServiceProvider
{
    using YeahTVApi.Entity;
    using YeahTVApi.Infrastructure;
    using System;
    using System.Collections.Generic;

    public class RoomManagerService : IRoomManagerService
    {
        //
        // GET: /Room/
        /// <summary>
        /// 获取酒店的房间列表
        /// </summary>
        /// <returns></returns>
        public List<RoomInfo> GetRoomList(String hotelID, String statues, String types, String roomNum, String sortBy = "ROOMASC")
        {
            var client = new PMSRoomStatusService.RoomStatusModifySoapClient();
            var rooms = client.GetRoomStatusSearchByCondition(hotelID, statues, types, roomNum);
            var list = new List<RoomInfo>();

            if (rooms == null)
                return null;
            foreach (var room in rooms)
            {
                var item = new RoomInfo();
                item.roomName = room.RoomNumber;
                item.roomStatus = room.Status;
                item.roomType = room.RoomTypeID;
                item.HallID = room.HallId;
                item.ModifyTime = room.ModifyTime.ToString("hh:mm");
                list.Add(item);
            }


            return list;
        }

        public RoomInfo ModifyRoomStatus(String hotelID, String RoomID, String OperatorID, String status, out String message)
        {
            message = null;
            var list = GetRoomList(hotelID, null, null, RoomID, null);

            if (list == null || list.Count != 1)
                throw new Exception("房间号检错出现问题，不存在" + RoomID + "该房号");

            RoomInfo info = list[0];

            if (info.roomStatus.Equals(status))
            {
                message = String.Format("{0}房间已经是{1},不允许重复修改房态", info.roomName, info.roomStatus);
                return info;
            }

            var client = new PMSRoomStatusService.RoomStatusModifySoapClient();
            if (info != null && (info.roomStatus.Equals("OC") || info.roomStatus.Equals("VC")))
            {
                try
                {

                    client.SetCheckRoom(hotelID, RoomID, OperatorID, out message);
                    if (!String.IsNullOrEmpty(message))
                        return null;
                }
                catch (Exception err)
                {
                    message = err.Message;
                    return null;
                }
                info.roomStatus = "OK";

            }
            else if (info != null && (info.roomStatus.Equals("OD") || info.roomStatus.Equals("VD") || info.roomStatus.Equals("O_D") || info.roomStatus.Equals("V_C")))
            {
                //client.SetCleanRoom(hotelID, RoomID, OperatorID);
                try
                {
                    client.SetCleanRoom(hotelID, RoomID, OperatorID, out message);
                    if (!String.IsNullOrEmpty(message))
                        return null;
                }
                catch (Exception err)
                {
                    message = err.Message;
                    return null;
                }
            }
            info.ModifyTime = DateTime.Now.ToString("HH:mm");
            return info;
        }
    }
}
