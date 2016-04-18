using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YeahTVApi.Entity.CentralMapping;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 房间信息
    /// </summary>
    public class RoomInfo
    {
        /// <summary>
        /// 房间类型
        /// </summary>
        public string roomType { get; set; }
        public String MarketPrice { get; set; }
        public List<HotelImage> RoomImages;
        public RoomDetail Cheapest;
        /// <summary>
        /// 房价所有市场活动房价明细
        /// </summary>
        public List<RoomDetail> Details;

        /// <summary>
        /// 房间名
        /// </summary>
        public string roomName { get; set; }

        public string roomStatus; 
        public string HallID;
        public string ModifyTime;
        /// <summary>
        /// 积分兑换数,如果有值支持积分兑换
        /// </summary>
        public String ExchangeRoomPoint { get; set; }

        /// <summary>
        /// 房型描述
        /// </summary>
        public RoomTypeModel RoomTypeDesc;
    }
}
