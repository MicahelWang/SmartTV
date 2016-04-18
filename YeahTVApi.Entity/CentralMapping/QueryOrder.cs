using System.Collections.Generic;


using System;
using YeahTVApi.Entity.CentralMapping;

namespace YeahTVApi.Entity.CentralMapping
{
  

    #region Result
    /// <summary>
    /// 订单查询结果对象
    /// </summary>
    public class QueryOrderResult : OperationResult
    {
        /// <summary>
        /// 查询结果包含的订单集合
        /// </summary>
        public List<OrderModelForQueryOrder> OrderList { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }


    }

    public class QuerOrderDetailResult : OperationResult
    {
        public OrderInfo Order { get; set; }
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class OrderModelForQueryOrder
    {
        
        /// <summary>
        /// 市场活动ID
        /// </summary>
        public string ActivityID { get; set; }
        /// <summary>
        /// 市场活动名称
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelName { get; set; }
        /// <summary>
        /// 酒店ID
        /// </summary>
        public string HotelID { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 酒店电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 经纬度
        /// </summary>
        public decimal Lat { get; set; }
        /// <summary>
        /// 经纬度
        /// </summary>
        public decimal Lng { get; set; }
        /// <summary>
        /// 是否开放预定
        /// </summary>
        public bool IsOpenCheckIn { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 预定类型
        /// </summary>
        public BookingType BookingType { get; set; }
        /// <summary>
        /// 房间数量 
        /// </summary>
        public int RoomCount { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string Resno { get; set; }
        /// <summary>
        /// 中央库订单号(支付订单时需要)
        /// </summary>
        public string CenterResno { get; set; }
        /// <summary>
        /// 入住日期
        /// </summary>
        public DateTime CheckInDate { get; set; }
        /// <summary>
        /// 离店日期
        /// </summary>
        public DateTime CheckOutDate { get; set; }
        /// <summary>
        /// 共X天
        /// </summary>
        public int DayCount { get; set; }
        /// <summary>
        /// 图标图片地址
        /// </summary>
        public string IconUrl { get; set; }
        /// <summary>
        /// 房型名称
        /// </summary>
        public string RoomTypeName { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }
        /// <summary>
        /// 订单状态描述
        /// </summary>
        public string OrderStatusDesc { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalPrice { get; set; }
         /// <summary>
        /// 支付状态描述
        /// </summary>
        public string PayStatusDesc { get; set; }
        /// <summary>
        /// 支付方式描述
        /// </summary>
        public string PayTypeDesc { get; set; }
        /// <summary>
        /// 是否必须在线预付
        /// </summary>
        public bool IsMustOnlinePay { get; set; }

        /// <summary>
        /// 根据市场活动配置确定订单是否可取消
        /// </summary>
        public bool IsCanCancelBooking { get; set; }

        /// <summary>
        /// 是否支持选房
        /// </summary>
        public bool IsCanChoiceRoom { get; set; }

        public string GuaranteeType { get; set; }
    }

  
}
