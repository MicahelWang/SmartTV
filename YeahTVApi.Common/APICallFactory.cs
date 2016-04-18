using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
namespace YeahTVApi.Common
{
    /// <summary>
    /// API调用工厂
    /// </summary>
    public class APICallFactory
    {
        /// <summary>
        /// API调用类型枚举
        /// </summary>
        public enum APICallType
        {
            /// <summary>
            /// 获取城市列表
            /// </summary>
            GetCityList,
            /// <summary>
            /// 查看优惠券详情（使用规则、门店）
            /// </summary>
            GetProjectRules,
            /// <summary>
            /// 获取优惠券详情
            /// </summary>
            QueryEcouponByTicketNo,


            /// <summary>
            /// 根据Vno和优惠券状态获取可用的优惠券 
            /// </summary>
            QueryMyAvailableCoupons,

            /// <summary>
            /// 根据Vno、酒店编号和入住日期状态获取可用的优惠券 
            /// </summary>
            QueryMyAvailableCouponsByHotel,
            /// <summary>
            /// 获取酒店明细 
            /// </summary>
            HotelDetail,

            /// <summary>
            /// 查询酒店商圈视图 
            /// </summary>
            GetHotelAreaViewList,

            /// <summary>
            /// 获取酒店列表 
            /// </summary>
            HotelList,

            /// <summary>
            /// 获取会员相关的发票抬头
            /// </summary>
            GetInvoiceTitles,

            /// <summary>
            /// 删除发票抬头 
            /// </summary>
            DeleteInvoiceTitle,


            /// <summary>
            /// 添加发票抬头 
            /// </summary>
            AddInvoiceTitle,

            /// <summary>
            /// 获取会员的礼品卡  
            /// </summary>
            LotteryList,


            /// <summary>
            /// 新增常住人
            /// </summary>
            AddMemberUsualContact,

            /// <summary>
            /// 新增常住酒店    
            /// </summary>
            AddMemberUsualHotel,


            /// <summary>
            /// 绑定第三方会员账号   
            /// </summary>
            BindMemberThirdParty,


            /// <summary>
            /// 删除常用联系人    
            /// </summary>
            DelMemberUsualContact,


            /// <summary>
            /// 删除会员常住酒店     
            /// </summary>
            DelMemberUsualHotel,


            /// <summary>
            /// 根据会员编号获第三方账号   
            /// </summary>
            GetMemberThirdParies,


            /// <summary>
            /// 会员登录    
            /// </summary>
            Login,

            /// <summary>
            /// 根据卡号获取公司会员基本信息 
            /// </summary>
            QueryCompanyMemberInfo,

            /// <summary>
            /// 根据会员编号获取会员信息    
            /// </summary>
            QueryMemberInfo,

            /// <summary>
            /// 获取常住人列表    
            /// </summary>
            QueryMemberUsualContacts,

            /// <summary>
            /// 根据VNO查询会员常住酒店    
            /// </summary>
            QueryMemberUsualHotels,

            /// <summary>
            /// 解除第三方会员账号     
            /// </summary>
            UnBindThirdParty,

            /// <summary>
            /// 修改会员邮箱     
            /// </summary>
            UpdateMemberEmail,

            /// <summary>
            /// 修改会员证件    
            /// </summary>
            UpdateMemberIdNo,

            /// <summary>
            /// 修改会员基本信息     
            /// </summary>
            UpdateMemberInfo,

            /// <summary>
            /// 修改会员手机    
            /// </summary>
            UpdateMemberMobile,

            /// <summary>
            /// 修改会员密码     
            /// </summary>
            UpdateMemberPassword,

            /// <summary>
            /// 取消订单     
            /// </summary>
            CancelOrder,

            /// <summary>
            /// 获取预定填写表格     
            /// </summary>
            GetBookingForm,

            /// <summary>
            /// 获取单个订单对象   
            /// </summary>
            GetOrderDetail,

            /// <summary>
            /// 获取订单列表     
            /// </summary>
            GetOrderList,

            /// <summary>
            /// 订单支付      
            /// </summary>
            GetPayPolicy,

            /// <summary>
            /// 订单表格提交      
            /// </summary>
            SubmitBookingForm,

            /// <summary>
            /// 绑定优惠券      
            /// </summary>
            BindTicketNoToMember,

            /// <summary>
            /// 注册会员
            /// </summary>
            RegisterMember,

            /// <summary>
            /// 获取积分
            /// </summary>
            GetPoints,

            /// <summary>
            /// 
            /// </summary>
            CheckPromotionTicket,

            /// <summary>
            /// 
            /// </summary>
            GetActivityIDByPromotionTicketNo,

            /// <summary>
            /// 
            /// </summary>
            UpdatePromotionTicket,

            /// <summary>
            /// 
            /// </summary>
            UsePromotionTicket,

            /// <summary>
            /// 会员是否已登录  
            /// </summary>
            IsAuthorized,
            /// <summary>
            /// 无密码登录
            /// </summary>
            LoginWithoutPassword,
            /// <summary>
            /// 获取推荐的市场活动
            /// </summary>
            GetRecommendActivityList,
            /// <summary>
            /// 获取酒店活动的会员价格
            /// </summary>
            QueryRoomPriceWithMemberLevelActivityID,
            /// <summary>
            /// 根据第三方账号获取Memberid
            /// </summary>
            GetMemberIdByThirdParty,
            /// <summary>
            /// 查询酒店交通路线
            /// </summary>
            GetHotelRouteList,
            /// <summary>
            /// 订单支付,不做实际支付的请求，而只是获取支付的相关签名参数
            /// </summary>
            OrderPay,
            /// <summary>
            /// 查询夜宵房
            /// </summary>
            QueryNightSalesRoom,
            /// <summary>
            /// 查询夜宵房城市列表
            /// </summary>
            QueryNightSalesRoomOfCityList,

            /// <summary>
            /// 储值卡充值
            /// </summary>
            StoreCardValue,

            /// <summary>
            /// 获取接待单房价
            /// </summary>
            GetReceiveOrderPrice,
            /// <summary>
            /// 获取续住接待单房价
            /// </summary>
            GeReceiveFutureOrderPrice,
            /// <summary>
            /// 提交续住
            /// </summary>
            GetReceiveOrderRequestResult,
            /// <summary>
            /// 获取退房信息
            /// </summary>
            GetReceiveOrderCheckoutInfoResult,
            /// <summary>
            /// 退房
            /// </summary>
            GetReceiveOrderSelfCheckout,

            /// <summary>
            /// 获取天气预报
            /// </summary>
            GetWeather

        }

        public static Dictionary<APICallType, String> dict = new Dictionary<APICallType, string>();
        //静态构造函数
        static APICallFactory()
        {
            dict.Add(APICallType.StoreCardValue, "api/member/StoreCardValue");
            dict.Add(APICallType.QueryNightSalesRoomOfCityList, "api/hotel/QueryNightSalesRoomOfCityList");
            dict.Add(APICallType.QueryNightSalesRoom, "api/hotel/QueryNightSalesRoom");
            dict.Add(APICallType.GetMemberIdByThirdParty, "api/member/GetMemberIdByThirdParty");
            dict.Add(APICallType.GetHotelRouteList, "api/hotel/GetHotelRouteList");
            dict.Add(APICallType.OrderPay, "api/order/OrderPay");
           
            dict.Add(APICallType.QueryRoomPriceWithMemberLevelActivityID, "api/hotel/QueryRoomPriceWithMemberLevelActivityID");
            dict.Add(APICallType.LoginWithoutPassword, "api/auth/LoginWithoutPassword");
            dict.Add(APICallType.GetRecommendActivityList, "api/activity/GetRecommendActivityList");
            dict.Add(APICallType.GetCityList,"api/city/GetCityList");
            dict.Add(APICallType.IsAuthorized, "api/auth/IsAuthorized");
            dict.Add(APICallType.GetProjectRules,"api/ECoupon/GetProjectRules");
            dict.Add(APICallType.QueryEcouponByTicketNo,"api/ECoupon/QueryEcouponByTicketNo");
            dict.Add(APICallType.QueryMyAvailableCoupons,"api/ECoupon/QueryMyAvailableCoupons");
            dict.Add(APICallType.QueryMyAvailableCouponsByHotel,"api/ECoupon/QueryMyAvailableCouponsByHotel");
            dict.Add(APICallType.HotelDetail,"api/Hotel/Detail");
            dict.Add(APICallType.GetHotelAreaViewList,"api/Hotel/GetHotelAreaViewList");
            dict.Add(APICallType.HotelList,"api/Hotel/List");
            dict.Add(APICallType.GetInvoiceTitles, "api/Invoice/QueryInvoiceTitles");
            dict.Add(APICallType.DeleteInvoiceTitle,"api/Invoice/DeleteInvoiceTitle");
            dict.Add(APICallType.AddInvoiceTitle,"api/Invoice/AddInvoiceTitle");
            dict.Add(APICallType.LotteryList,"api/Lottery/List");
            dict.Add(APICallType.AddMemberUsualContact,"api/Member/AddMemberUsualContact");
            dict.Add(APICallType.AddMemberUsualHotel,"api/Member/AddMemberUsualHotel");
            dict.Add(APICallType.BindMemberThirdParty,"api/Member/BindMemberThirdParty");
            dict.Add(APICallType.DelMemberUsualContact,"api/Member/DelMemberUsualContact");
            dict.Add(APICallType.DelMemberUsualHotel,"api/Member/DelMemberUsualHotel");
            dict.Add(APICallType.GetMemberThirdParies,"api/Member/GetMemberThirdParies");
            dict.Add(APICallType.Login,"api/Auth/Login");
            dict.Add(APICallType.QueryCompanyMemberInfo,"api/Member/QueryCompanyMemberInfo");
            dict.Add(APICallType.QueryMemberInfo,"api/Member/QueryMemberInfo");
            dict.Add(APICallType.QueryMemberUsualContacts,"api/Member/QueryMemberUsualContacts");
            dict.Add(APICallType.QueryMemberUsualHotels,"api/Member/QueryMemberUsualHotels");
            dict.Add(APICallType.RegisterMember, "/api/Member/RegisterMember");
            dict.Add(APICallType.UnBindThirdParty,"api/Member/UnBindThirdParty");
            dict.Add(APICallType.UpdateMemberEmail,"api/Member/UpdateMemberEmail");
            dict.Add(APICallType.UpdateMemberIdNo,"api/Member/UpdateMemberIdNo");
            dict.Add(APICallType.UpdateMemberInfo,"api/Member/UpdateMemberInfo");
            dict.Add(APICallType.UpdateMemberMobile,"api/Member/UpdateMemberMobile");
            dict.Add(APICallType.UpdateMemberPassword,"api/Member/UpdateMemberPassword");
            dict.Add(APICallType.CancelOrder,"api/Order/Cancel");
            dict.Add(APICallType.GetBookingForm,"api/Order/GetBookingForm");
            dict.Add(APICallType.GetOrderDetail,"api/Order/GetOrderDetail");
            dict.Add(APICallType.GetOrderList,"api/Order/GetOrderList");
            dict.Add(APICallType.GetPayPolicy,"api/Order/GetPayPolicy");
            dict.Add(APICallType.BindTicketNoToMember, "api/Order/BindTicketNoToMember");
            dict.Add(APICallType.SubmitBookingForm, "api/Order/SubmitBookingForm");
            dict.Add(APICallType.GetPoints, "api/Point/GetPoints");
            dict.Add(APICallType.CheckPromotionTicket, "api/Promotion/CheckPromotionTicket");
            dict.Add(APICallType.GetActivityIDByPromotionTicketNo, "api/Promotion/GetActivityIDByPromotionTicketNo");
            dict.Add(APICallType.UpdatePromotionTicket, "api/Promotion/UpdatePromotionTicket");
            dict.Add(APICallType.UsePromotionTicket, "api/Promotion/UsePromotionTicket");
            dict.Add(APICallType.GetReceiveOrderPrice, "api/ReceiveOrder/GeReceiveOrdersResult");
            dict.Add(APICallType.GeReceiveFutureOrderPrice, "api/ReceiveOrder/GeReceiveOrderPriceResult");
            dict.Add(APICallType.GetReceiveOrderRequestResult, "api/ReceiveOrder/GetReceiveOrderRequestResult");
            dict.Add(APICallType.GetReceiveOrderCheckoutInfoResult, "/api/ReceiveOrder/GetReceiveOrderCheckoutInfoResult");
            dict.Add(APICallType.GetReceiveOrderSelfCheckout, "api/ReceiveOrder/GetReceiveOrderSelfCheckout");
            dict.Add(APICallType.GetWeather, "api/Business/GetWeatherResult");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlType"></param>
        /// <returns></returns>
        public static String CallAction(APICallType urlType)
        {
            return dict[urlType];

        }

        /// <summary>
        /// 数据字段
        /// </summary>
        public const String DATA_FIELD = "Data";

        /// <summary>
        /// 数据字段
        /// </summary>
        public const String RESULT_TYPE_FIELD = "ResultType";
        /// <summary>
        /// 数据字段
        /// </summary>
        public const String MESSAGE_FIELD = "Messages";
        /// <summary>
        /// 数据字段
        /// </summary>
        public const String MORE_INFO_FIELD = "MoreInfo";

        /// <summary>
        /// 判断执行是否成功
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Boolean IsCallSuccess(JObject obj)
        {
            var data = obj[RESULT_TYPE_FIELD];
            return (data != null && data.ToString().Equals("0"));

        }
    }
}
