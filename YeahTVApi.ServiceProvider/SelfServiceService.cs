using Newtonsoft.Json;

namespace YeahTVApi.ServiceProvider
{
    using HZ.Web.Authorization;
    using YeahTVApi.Common;
    using YeahTVApi.Entity;
    using YeahTVApi.Entity.CentralMapping;
    using YeahTVApi.Infrastructure;
    using YeahTVApi.ServiceProvider.PMSICRS;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SelfServiceService : CentralGetwayServiceBase, ISelfServiceService
    {
        /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public SelfServiceService()
            : base(typeof(CentralApiResult<List<HotelInfo>>))
        {

        }


        public bool IsCheckOutByReceiveID(string sReceiveID)
        {
            return false;
        }

        /// <summary>
        /// 续住
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="vNumber"></param>
        /// <param name="receiveOrderId"></param>
        /// <param name="continueDays"></param>
        /// <returns></returns>
        public bool GetReceiveOrderRequestResult(string hotelId, string vNumber, string receiveOrderId, int continueDays, out string message)
        {
            bool flag = false;
            message = "";
            try
            {
                String action = APICallFactory.CallAction(APICallFactory.APICallType.GetReceiveOrderRequestResult);
                var pams = new List<KeyValuePair<String, String>>();
                pams.Add(new KeyValuePair<string, string>("ReceiveOrderId", receiveOrderId));
                pams.Add(new KeyValuePair<string, string>("VNumberCode", vNumber));
                // 酒店Id,PMS系统要求必传，但将来可能废弃。
                pams.Add(new KeyValuePair<string, string>("HotelId", hotelId));
                // 续住天数，至少值和最大值由接待单详情返回。 0.5天当1天处理
                pams.Add(new KeyValuePair<string, string>("ContinueDays", continueDays.ToString()));
                //完成用户在大促销环境中的登录操作。
                var response = CentralApi.GetResponse(action, pams, null, null);
                var responseJson = response.Content.ReadAsStringAsync().Result;
                response.Dispose();
                response = null;
                var obj = (CentralApiResult<Object>)JsonConvert.DeserializeObject(responseJson, typeof(CentralApiResult<Object>));
                if (null != obj)
                {
                    if (obj.ResultType == OperationResultType.Successed)
                    {
                        flag = true;
                    }
                    message = obj.Message;
                }
            }
            catch (Exception ex)
            {

                flag = false;
                message = ex.Message;
            }

            return flag;
        }

        /// <summary>
        /// 退房
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="vNumber"></param>
        /// <param name="receiveOrderId"></param>
        /// <param name="continueDays"></param>
        /// <returns></returns>
        public bool GetReceiveOrderSelfCheckout(string hotelId, string vNumber, string receiveOrderId, out string message)
        {
            bool flag = false;
            message = "";
            try
            {
                String action = APICallFactory.CallAction(APICallFactory.APICallType.GetReceiveOrderSelfCheckout);
                var pams = new List<KeyValuePair<String, String>>();
                pams.Add(new KeyValuePair<string, string>("ReceiveOrderId", receiveOrderId));
                pams.Add(new KeyValuePair<string, string>("VNumberCode", vNumber));
                // 酒店Id,PMS系统要求必传，但将来可能废弃。
                pams.Add(new KeyValuePair<string, string>("HotelId", hotelId));
                //完成用户在大促销环境中的登录操作。
                var response = CentralApi.GetResponse(action, pams, null, null);
                var responseJson = response.Content.ReadAsStringAsync().Result;
                response.Dispose();
                response = null;
                var obj = (CentralApiResult<Object>)JsonConvert.DeserializeObject(responseJson, typeof(CentralApiResult<Object>));
                if (null != obj)
                {
                    if (obj.ResultType == OperationResultType.Successed)
                    {
                        flag = true;
                    }
                    message = obj.Message;
                }
            }
            catch (Exception ex)
            {

                flag = false;
                message = ex.Message;
            }

            return flag;
        }


    }
}
