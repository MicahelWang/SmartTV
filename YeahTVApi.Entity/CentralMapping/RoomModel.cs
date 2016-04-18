using System.Diagnostics;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;


namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 酒店房间对象
    /// </summary>
    public class RoomModel
    {
      
        private HotelModel hotelHost { get; set; }

       

        // <summary>
        /// 默认PMS每日房量
        /// </summary>
        internal List<RoomStockEntity> DefaultRoomStocks { get; private set; }

        public List<RoomDetailModel> Details
        {
            get;
            set;
        }

        /// <summary>
        /// 最便宜的
        /// </summary>
        public RoomDetailModel Cheapest { get; private set; }

        /// <summary>
        /// 酒店ID
        /// </summary>
        public string HotelID { get; private set; }

        /// <summary>
        /// 房型ID
        /// </summary>

        public string RoomTypeID { get; private set; }

        /// <summary>
        /// 房型名称
        /// </summary>
       
        public string RoomTypeName { get; private set; }

        /// <summary>
        /// 房型是否被锁定(通过SpecialBlockedControl表的配置), 该属性在类外部被赋值
        /// </summary>
        public bool IsBlocked { get; set; }

        public RoomTypeModel Info
        {
           get;set;
        }
       /// <summary>
        /// 积分兑换免房的总积分
        /// </summary>
        public int TotalExchangePoint { get; set; }

       
       
     }

    /// <summary>
    /// 房间详细对象
    /// </summary>
    public class RoomTypeModel
    {
        /// <summary>
        /// 床型
        /// </summary>
        public string BedType { get; set; }

        /// <summary>
        /// 床宽
        /// </summary>
        public string BedSize { get; set; }

        /// <summary>
        /// 房间面积
        /// </summary>
        public string RoomArea { get; set; }

        /// <summary>
        /// 楼层
        /// </summary>
        public string Floor { get; set; }

        /// <summary>
        /// 窗户
        /// </summary>
        public string Window { get; set; }

        /// <summary>
        /// 无烟楼层
        /// </summary>
        public string NoSmokingFloor { get; set; }

        /// <summary>
        /// 上网方式
        /// </summary>
        public string SupportNetwork { get; set; }

        /// <summary>
        /// 入住人数
        /// </summary>
        public string MaxCheckInPeopleNum { get; set; }

        /// <summary>
        /// 房型图片
        /// </summary>
        public string RoomPhotoUrl { get; set; }

        /// <summary>
        /// 房型描述
        /// </summary>
        public string Summary { get; set; }
    }
}
