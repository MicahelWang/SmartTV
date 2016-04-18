namespace YeahTVApi.ServiceProvider
{
    using HZ.Web.Authorization;
    using YeahTVApi.Common;
    using YeahTVApi.Entity;
    using YeahTVApi.Entity.CentralMapping;
    using YeahTVApi.Infrastructure;
    using System;
    using System.Collections.Generic;

    public class PriceService : CentralGetwayServiceBase, IPriceService
    {
         /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public PriceService()
            : base(typeof(CentralApiResult<YeahTVApi.Entity.CentralMapping.ReceiveOrdersResult>))
        {

        }

        /// <summary>
        /// 此方法必须被重载以实现相应转换功能
        /// </summary>
        /// <returns></returns>
        public override Object ConvertTo(String json, Guest guest)
        {

            var result = base.ConvertTo(json, guest);
            if (result == null)
                return null;
            var data = result as CentralApiResult<YeahTVApi.Entity.CentralMapping.ReceiveOrdersResult>;
            if (data == null)
                return null;
            if (data.ResultType != Entity.CentralMapping.OperationResultType.Successed)
            {
                throw new ApiException(data.Message);
            }
            if (null != data.Data && data.Data.ReceiveOrders != null && data.Data.ReceiveOrders.Count > 0)
                return data.Data.ReceiveOrders[0];
            else
                return null;
             
        }


        /// <summary>
        /// 获取当前接待单的房价-中枢
        /// </summary>
        /// <param name="vNumberCode"></param>
        /// <param name="hotelId"></param>
        /// <param name="receiveOrderId"></param>
        public ReceiveOrders GetOrderPrice(string hotelId, string vNumber, string receiveOrderId)
        {
            ReceiveOrders orders = null;
            String action = APICallFactory.CallAction(APICallFactory.APICallType.GetReceiveOrderPrice);
            List<KeyValuePair<String, String>> pams = new List<KeyValuePair<String, String>>();
            pams.Add(new KeyValuePair<string, string>("OrderType", ReceiveOrderType.MemeberCardId.ToString()));
            // 请求时的必要参数，中枢根据请求类型来调用PMS不同的方法 
            pams.Add(new KeyValuePair<string, string>("VNumberCode", vNumber));
            // 酒店Id,PMS系统要求必传，但将来可能废弃。
            pams.Add(new KeyValuePair<string, string>("HotelId", hotelId));
            pams.Add(new KeyValuePair<string, string>("ReceiveOrderId", receiveOrderId));
            //完成用户在大促销环境中的登录操作。
            var response = CentralApi.GetResponse(action, pams, null, null);
            var responseJson = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            response = null;
            orders = ConvertTo(responseJson, null) as ReceiveOrders;
            return orders;
        }


        /// <summary>
        /// 获取当前接待单的房价-中枢
        /// </summary>
        /// <param name="vNumberCode"></param>
        /// <param name="hotelId"></param>
        /// <param name="receiveOrderId"></param>
        public ReceiveOrders GetOrderPriceByRoomId(string hotelId, string roomId, string receiveOrderId)
        {
            ReceiveOrders orders = null;
            String action = APICallFactory.CallAction(APICallFactory.APICallType.GetReceiveOrderPrice);
            List<KeyValuePair<String, String>> pams = new List<KeyValuePair<String, String>>();
            pams.Add(new KeyValuePair<string, string>("OrderType", ReceiveOrderType.HotelAndRoomId.ToString()));
            // 请求时的必要参数，中枢根据请求类型来调用PMS不同的方法 
            pams.Add(new KeyValuePair<string, string>("sRoomNo", roomId));
            // 酒店Id,PMS系统要求必传，但将来可能废弃。
            pams.Add(new KeyValuePair<string, string>("HotelID", hotelId)); 
            //完成用户在大促销环境中的登录操作。
            var response = CentralApi.GetResponse(action, pams, null, null);
            var responseJson = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            response = null;
            orders = ConvertTo(responseJson, null) as ReceiveOrders;
            return orders;
        }

        public enum ReceiveOrderType
        {
            /// <summary>
            /// 未知
            /// </summary>
            Unknow = 0,
            /// <summary>
            /// 身份证
            /// </summary>
            ID = 1,
            /// <summary>
            /// 会员ID
            /// </summary>
            MemeberCardId = 2,
            /// <summary>
            /// 酒店ID
            /// </summary>
            HotelAndRoomId = 3,
            /// <summary>
            /// 接待单号
            /// </summary>
            ReceiveOrder = 4,

        }

    }
}
