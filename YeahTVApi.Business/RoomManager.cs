namespace HZTVApi.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using HZTVApi.Entity.Model;
    using HZTVApi.Entity;
    using HZTVApi.Infrastructure;

    public class RoomManager : IRoomManager
    {
        private IRoomManagerService roomManagerService;

        public RoomManager(IRoomManagerService roomManagerService)
        {
            this.roomManagerService = roomManagerService;
        }

        //
        // GET: /Room/
        /// <summary>
        /// 获取酒店的房间列表
        /// </summary>
        /// <returns></returns>
        public List<RoomInfo> GetRoomList(string hotelID, string statues, string types, string roomNum, string sortBy = "ROOMASC")
        {
            return roomManagerService.GetRoomList(hotelID,statues,types,roomNum,sortBy);
        }

        public RoomInfo ModifyRoomStatus(string hotelID, string RoomID, string OperatorID, string status, out string message)
        {
            
            return roomManagerService.ModifyRoomStatus(hotelID,RoomID,OperatorID,status,out message);
        }
    }

}
