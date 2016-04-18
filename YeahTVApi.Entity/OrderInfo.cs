using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 订单实体
    /// </summary>
    public class OrderInfo
    {

        /// <summary>
        /// 续住天数
        /// </summary>
        public int continueLiveDay;

        public int type;

        [JsonIgnore]
        public String MemberLevel;
        [JsonIgnore]
        public String cusCategory;
        [JsonIgnore]
        public String RateCode;
        [JsonIgnore]
        public decimal MarketPrice;

        /// <summary>
        /// V卡号
        /// </summary>
        public string vno { get; set; }
        /// <summary>
        /// 储值卡余额
        /// </summary>
        public decimal exCardCreditValue;
        /// <summary>
        /// 积分余额
        /// </summary>
        public int exPoint;
        /// <summary>
        /// 提前立减金额
        /// </summary>
        public String AdvanceDiscountPrice { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 酒店经纬度
        /// </summary>
        public string geo { get; set; }
        /// 手机
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 性别 1 男 2 女
        /// </summary>
        public int sex { get; set; }

        /// <summary>
        /// 酒店ID
        /// </summary>
        public string hotelID { get; set; }

        /// <summary>
        /// 酒店中文名称
        /// </summary>
        public string hotelName { get; set; }

        /// <summary>
        /// 酒店品牌
        /// </summary>
        public string hotelStyle { get; set; }

        /// <summary>
        /// 酒店地址
        /// </summary>
        public string hotelAddr { get; set; }

        /// <summary>
        /// 酒店电话
        /// </summary>
        public string hotelTel { get; set; }

        /// <summary>
        ///自助入住状态
        /// </summary>
        public string checkInStatus { get; set; }

        /// <summary>
        /// 是否开放自助CheckIn
        /// </summary>
        public bool IsOpenCheckIn { get; set; }

        /// <summary>
        /// 房型
        /// </summary>
        public string roomType { get; set; }

        /// <summary>
        /// 房型中文名称
        /// </summary>
        public string roomTypeName { get; set; }

        /// <summary>
        /// 房间数
        /// </summary>
        public Int32 roomNum { get; set; }

        /// <summary>
        /// 入住日期
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// 离店日期
        /// </summary>
        public string endDate { get; set; }

        /// <summary>
        /// 担保类型
        /// </summary>
        public string guaranteeType { get; set; }

        /// <summary>
        /// 中央订单号
        /// </summary>
        public string centerResno { get; set; }

        /// <summary>
        /// 账务编号
        /// </summary>
        public string billID { get; set; }

        /// <summary>
        /// 订单号（门店）
        /// </summary>
        public string resno { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>       
        public string status { get; set; }

        /// <summary>
        /// 订单状态（界面显示）
        /// </summary>       
        public string statusMsg { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public string payStatus { get; set; }

        /// <summary>
        /// 订单标识码
        /// </summary>
        public string activityCode { get; set; }

        /// <summary>
        /// 订单标识信息
        /// </summary>
        //public string activityInfo { get; set; }

        /// <summary>
        /// 总房价
        /// </summary>
        public decimal totalPrice { get; set; }

        /// <summary>
        /// 总积分
        /// </summary>
        public decimal totalPoint { get; set; }



        /// <summary>
        /// 折扣价
        /// </summary>
        public string discountAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 预订时间
        /// </summary>
        public string resvTime { get; set; }

        /// <summary>
        /// 预订时间
        /// </summary>
        [JsonIgnore]
        public DateTime ResvTime { get; set; }

        /// <summary>
        /// 保留时间
        /// </summary>
        [JsonIgnore]
        public string holdTime { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        [JsonIgnore]
        public string cancelTime { get; set; }


        /// <summary>
        /// 担保金额
        /// </summary>
        public decimal guaranteeAmount { get; set; }

        /// <summary>
        /// 支付方式（CRS），1银行卡，2优惠券，3前台，4储值卡，5支付宝，6财付通
        /// </summary>
        [JsonIgnore]
        public string payType { get; set; }



        /// <summary>
        /// 支付状态修改时间
        /// </summary>
       // public string payStateModifyTime { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public string memberID { get; set; }



        /// <summary>
        /// 是否已支付成功
        /// </summary>
        public Int32 payOK { get; set; }



        /// <summary>
        /// 首夜房费
        /// </summary>
        public decimal firstNightPrice { get; set; }

        /// <summary>
        /// 最晚修改/取消时间（dLastCancelTime）
        /// </summary>
        [JsonIgnore]
        public string lastCancelTime { get; set; }

        /// <summary>
        /// 是否可以取消
        /// </summary>
        public Int32 isCanCancel { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string hotelBookingTips { get; set; }



        /// <summary>
        /// 订单提示
        /// </summary>
        public string orderHint { get; set; }

        /// <summary>
        /// 储值卡余额
        /// </summary>
        public decimal cardCreditValue { get; set; }

        /// <summary>
        /// 已选的房号
        /// </summary>
        public String checkInRoomNumber { get; set; }

        /// <summary>
        /// 选房地址
        /// </summary>
        public String CheckInURL { get; set; }

        /// <summary>
        /// 分享字段1
        /// </summary>
        public String ShareField1 { get; set; }
        /// <summary>
        /// 分享字段2
        /// </summary>
        public String ShareField2 { get; set; }
        public String CheckInPayCall { get; set; }

        /// <summary>
        /// 预订成功页面文案，已通过PayPolicy接口返回
        /// </summary>
        public String BookingSuccessHint;
        /// <summary>
        /// 支付成功页面文案，已通过PayPolicy接口返回
        /// </summary>
        public String PaySuccessHint; 
        public String UnpaidHint;
        public String PaidHint;
        public String PayHintA="得到华住支付成功的确认后，房间将为您保留到{0}；\r\n";

        public String PayHintB="得到华住支付成功的确认后，房间将为您保留到{0}；\r\n";

        public String PointSuccessHint = "\r\n积分兑换免房成功，积分已扣除;\r\n本订单为积分兑换免费房订单，可在入住日当天中午12:00以前取消\r\n积分兑换订单取消，所返还的积分有效期与扣除时一致，如返还积分已过期，则无法继续使用；\r\n积分兑换的订单不含早餐\r\n积分兑换免房，除了享受该会员级别相对应的延迟退房权益外，无法享受房费积分、免费早餐等其他会员权益";

      
        /// <summary>
        /// 允许支付首页房价
        /// </summary>
        public bool IsCanPayFirstNight = true;

        /// <summary>
        /// 允许支付全部房价
        /// </summary>
        public bool IsCanPayALL=true;

        /// <summary>
        /// 允许储值卡支付
        /// </summary>
        public bool IsCanUseCard = true;

        /// <summary>
        /// 允许支付宝支付
        /// </summary>
        public bool IsCanUseAlipay = true;

        public String ShareHotelURL;
    }

}
