using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZTVApi.Entity;
using HZTVApi.Entity.CentralMapping;
using HZTVApi.Common;
using HZ.Web.Authorization;


namespace HZTVApi.Business.BusinessAttributes
{
    /// <summary>
    /// 酒店详情转换类
    /// </summary>
    public class GetOrderDetailAttribute:BusinessAttribute
    {
         /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public GetOrderDetailAttribute()
            : base(typeof(CentralApiResult<HZTVApi.Entity.CentralMapping.QuerOrderDetailResult>))
        {

        }

        public override Object ConvertTo(string json, Guest guest)
        {
            var result = base.ConvertTo(json, guest);
            if (result == null)
                return null;
            var data = result as CentralApiResult<QuerOrderDetailResult>;
            if (data == null)
                return null;
            if (data.ResultType != Entity.CentralMapping.OperationResultType.Successed)
            {
                throw new ApiException("查无此订单");
            }
            ResvInfo info = new ResvInfo();
            var order = data.Data.Order;
            info.activityCode = order.ActivityID;
            info.hotelAddr = order.Address;
            info.guaranteeType = order.AssureType;
            info.cancelTime = order.CancelTime;
            info.centerResno = order.CenterResno;
            info.startDate = order.CheckInDate.ToString("yyyy-MM-dd");
            info.endDate = order.CheckOutDate.ToString("yyyy-MM-dd");
            info.email = order.ContactEMail;
            info.mobile = order.ContactMobile;
            info.name = order.ContacttName;
            info.discountAmount = order.DiscountAmount;
            info.firstNightPrice = order.FirstNightPrice;
            info.guaranteeAmount = order.GuaranteeAmount;
            info.holdTime= order.Holdtime;
            info.hotelID = order.HotelID;
            info.hotelName = order.HotelName;
            info.InvoiceTitle = order.InvoiceTitle;
            info.InvoiceType = order.InvoiceType == InvoiceType.Company ? 1 : order.InvoiceType == InvoiceType.Personal? 0:-1;
            info.isMustOnlinePay =order.mustOnlinePay? 1:0;
            info.payOK= order.payOK? 1:0;
            info.payStatus = order.PayStatus;
            info.payType= order.PayType;
            info.remark = order.Remark;
            info.resno = order.ResNo;
            info.resvTime = order.Resvtime.ToString("yyyy-MM-dd");
            info.ShareHotelURL = PubFun.GetAppSetting("CouponRules_CALLBACK") + info.hotelID;
            info.roomNum = order.RoomCount;
            info.roomType = order.RoomType;
            info.roomTypeName = order.RoomTypeName;
            info.status = order.Status;
            info.hotelTel= order.Telephone;
            info.totalPrice = order.TotalPrice;
            info.lastCancelTime= order.LastCancelTime;
            info.type = (int)order.BookingType;
            info.geo = order.Lat + "|" + order.Lng;
            info.isCanCancel = order.IsCanCancelBooking ?1:0;
            info.IsOpenCheckIn = order.IsCanChoiceRoom;
            switch (order.Status)
            {
                case "R":
                    info.statusMsg = "预订成功";
                    if (order.payOK)
                    {
                        if (order.GuaranteeType == "A")
                        {
                            info.statusMsg += "(首夜已预付)";
                        }
                        else if (order.GuaranteeType == "B")
                        {
                            info.statusMsg += "(全额已预付)";
                        }
                    }
                    break;
                case "N": info.statusMsg = "已处理"; break;
                case "X": info.statusMsg = "预订取消"; info.IsOpenCheckIn = false; info.isCanCancel = 0; break;
                case "O": info.statusMsg = "已入住"; info.IsOpenCheckIn = false; info.isCanCancel = 0; break;
                case "E": info.statusMsg = "已退房"; info.IsOpenCheckIn = false; info.isCanCancel = 0; break;
                case "W": info.statusMsg = "排队中"; break;
                default: info.statusMsg = "未知"; break;
            }

            
            
            info.BookingDays = (int)(order.CheckOutDate - order.CheckInDate).TotalDays;
            if (info.totalPrice == 0 && info.payOK == 0)
            {
                info.payOK = 1;
                info.guaranteeType = "B";
            }
          
            info.HotelImage = order.HeaderPhoto;
            if (order.HotelNotice != null)
            {
                foreach (var notice in order.HotelNotice)
                {
                    //if (notice.IsAboutResv)
                    info.hotelBookingTips += notice.Description;
                }
            }
            //暂时关闭自助选房功能
            info.ShareField1 = PubFun.GetAppSetting("Share_Booking");
            if (!String.IsNullOrEmpty(info.ShareField1))
            {
               
                //需要增加酒店电话
                //我预定了[{hotelname}]{roomType}{roomNum}1间,{CheckinDate}入住（住{days}晚），酒店地址：{address},酒店电话：{tel},http:/m.huazhu.com/hotelinfo/Hotel.aspx?hotelID={hotelid}
                info.ShareField1 = info.ShareField1.Replace("{hotelname}", info.hotelName).Replace("{tel}", order.Telephone).Replace("{address}", info.hotelAddr);
                info.ShareField1 = info.ShareField1.Replace("{roomType}", info.roomTypeName).Replace("{roomNum}", info.roomNum.ToString()).Replace("{CheckinDate}", info.startDate);
                info.ShareField1 = info.ShareField1.Replace("{days}", info.BookingDays.ToString()).Replace("{hotelid}", info.hotelID);
            }
            info.BookingSuccessHint = order.BookingSuccessText;

            if (info.activityCode != null && info.type==0)
                info.BookingSuccessHint+="\r\n入住当日18:00前可以取消预付房费订单，18:00后下单1小时内可以取消预付房费订单，储值金取消后立即返回，其他支付方式预计3-7个工作日内退款。";
           
            return info;
        }

        public ResvInfo Query(BaseRequestData data, String ResNo, Guest guest)
        {
            String sign = PubFun.String2Md5(ResNo + privateKey);
            Dictionary<String, String> pams = new Dictionary<string, string>();
            pams.Add("ResNo", ResNo);
            pams.Add("sign", sign);
            //完成用户在大促销环境中的登录操作。
            var response = CentralApi.GetResponse(APICallFactory.CallAction(APICallFactory.APICallType.GetOrderDetail), pams, data.TOKEN, data.language);
            var responseJson = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            response = null;
            ResvInfo info = ConvertTo(responseJson, guest) as ResvInfo;
            //查询该订单的支付策略
          

          
            if (DateTime.Parse(info.startDate) < DateTime.Now.Date && info.status=="R")
            {
                info.isCanCancel = 1;
                info.IsCanPayALL = false;
                info.IsCanPayFirstNight = false;
            }
            info.isCanCancel = 1;
            if (info.hotelBookingTips!= null)
                info.hotelBookingTips= info.hotelBookingTips.TrimStart('\r', '\n');
            if (info.UnpaidHint != null)
                info.UnpaidHint = info.UnpaidHint.TrimStart('\r', '\n');
            if (info.PaidHint != null)
                info.PaidHint = info.PaidHint.TrimStart('\r', '\n');
            return info;
        }
        
    }
}
