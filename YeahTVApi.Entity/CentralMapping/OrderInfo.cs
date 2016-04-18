using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace YeahTVApi.Entity.CentralMapping
{
    public enum InvoiceType
    {
        /// <summary>
        /// 未设置
        /// </summary>
        NoSet = 0,
        /// <summary>
        /// 个人发票(发票内容固定为住宿费)
        /// </summary>
        Personal = 1,
        /// <summary>
        /// 公司会员
        /// </summary>
        Company = 2


      
    }

    /// <summary>
    /// 订单信息
    /// </summary>
    [Serializable]
    public class OrderInfo
    {
        /// <summary>
        /// 到达时间
        /// </summary>
        public DateTime? ArrTime { get; set; }



        public Boolean IsCanCancelBooking { get; set; }

        public Boolean IsCanChoiceRoom { get; set; }

        public String BookingSuccessText { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
       
        public string ResNo { get; set; }

        public BookingType BookingType { get; set; }
        /// <summary>
        /// 酒店ID
        /// </summary>
       
        public string HotelID { get; set; }
        /// <summary>
        /// 酒店中文名称
        /// </summary>
       
        public string HotelName { get; set; }
        /// <summary>
        /// 酒店地址
        /// </summary>
       
        public string Address { get; set; }
        /// <summary>
        /// 酒店电话
        /// </summary>
       
        public string Telephone { get; set; }

        /// <summary>
        /// 酒店品牌类型
        /// </summary>
       
        public HotelStyle HotelStyle { get; set; }
        /// <summary>
        /// 最晚可取消时间
        /// </summary>
       
        public string LastCancelTime { get; set; }
        /// <summary>
        /// 城市编号
        /// </summary>
       
        public int CityID { get; set; }


        /// <summary>
        /// 房间数
        /// </summary>
       
        public int RoomCount { get; set; }
        /// <summary>
        /// 房型
        /// </summary>
       
        public string RoomType { get; set; }
        /// <summary>
        /// 房型中文名称
        /// </summary>
       
        public string RoomTypeName { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>       
       
        public string Status { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
       
        public string PayStatus { get; set; }
        /// <summary>
        /// A 首夜 B 全额
        /// </summary>
       
        public string AssureType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
       
        public string Remark { get; set; }
        /// <summary>
        /// 预订渠道
        /// </summary>
       
        public string SrcCode { get; set; }
        /// <summary>
        /// 市场活动码  PMS直接返回
        /// </summary>
       
        public string ActivityID { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
       
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 客历号 下单人
        /// </summary>
       
        public string MemberID { set; get; }
        /// <summary>
        /// 常用联系人 邮箱(住房的人 联系人) 常用的信息都是从数据库读出来, 并非接口返回的
        /// </summary>
       
        public string ContactEMail { get; set; }
        /// <summary>
        /// 常用联系人 手机
        /// </summary>
       
        public string ContactMobile { get; set; }
        /// <summary>
        /// 常用联系人 联系人姓名
        /// </summary>
       
        public string ContacttName { get; set; }
        /// <summary>
        /// 发票类型
        /// </summary>

        public InvoiceType InvoiceType { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
       
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
       
        public string OperatorID { get; set; }
        /// <summary>
        /// 预订所使用客户端
        /// </summary>
       
        public string OnlineSrc { get; set; }

        /// <summary>
        /// 入住日期
        /// </summary>
       
        public DateTime CheckInDate { get; set; }
        /// <summary>
        /// 离店日期
        /// </summary>
       
        public DateTime CheckOutDate { get; set; }
        /// <summary>
        /// 保留时间
        /// </summary>
       
        public String Holdtime { get; set; }
        /// <summary>
        /// 预订时间
        /// </summary>
       
        public DateTime Resvtime { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
       
        public String Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
       
        public String Mobile { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
       
        public String Email { get; set; }
        /// <summary>
        /// V卡号
        /// </summary>
       
        public String Vno { get; set; }
        /// <summary>
        /// 首夜房费
        /// </summary>
       
        public decimal FirstNightPrice { get; set; }

        /// <summary>
        /// 账务编号
        /// </summary>
       
        public String BillID { get; set; }
        /// <summary>
        /// 担保类型
        /// </summary>
       
        public String GuaranteeType { get; set; }
        /// <summary>
        /// 支付方式（CRS），1银行卡，2优惠券，3前台，4储值卡，5支付宝，6财付通,9积分免房
        /// todo:赋值规则细化
        /// </summary>
       
        public String PayType { get; set; }
        /// <summary>
        /// 中央订单号
        /// </summary>
       
        public String CenterResno { get; set; }
        /// <summary>
        /// 担保金额
        /// </summary>
       
        public Decimal GuaranteeAmount { get; set; }
        /// <summary>
        /// 折扣价 PMS直接获取
        /// </summary>
       
        public String DiscountAmount { get; set; }
        /// <summary>
        /// 取消时间 PMS直接获取
        /// </summary>
       
        public String CancelTime { get; set; }
        /// <summary>
        /// 订单标识信息 PMS获取
        /// </summary>
       
        public String ActivityInfo { get; set; }
        /// <summary>
        /// 入账的账务ID PMS获取
        /// </summary>
       
        public string PayCreditID { get; set; }
        /// <summary>
        /// 是否已支付成功
        /// </summary>
       
        public bool payOK { get; set; }
        /// <summary>
        /// 网站独享的活动标记，记录在网站数据库中，用于担保超时的判断
        /// </summary>
       
        public bool mustOnlinePay { set; get; }

        public List<HotelNotice> HotelNotice { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

        public string HeaderPhoto { get; set; }




    }
}
