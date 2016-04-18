namespace YeahTVApi.ServiceProvider
{
    using HZ.Web.Authorization;
    using YeahTVApi.Common;
    using YeahTVApi.Entity;
    using YeahTVApi.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PriceFutureService : CentralGetwayServiceBase, IPriceFutureService
    {
        public PriceFutureService()
            : base(typeof(CentralApiResult<YeahTVApi.Entity.CentralMapping.ReceiveOrderPriceResult>))
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
            var data = result as CentralApiResult<YeahTVApi.Entity.CentralMapping.ReceiveOrderPriceResult>;
            if (data == null)
                return null;
            if (data.ResultType != Entity.CentralMapping.OperationResultType.Successed)
            {
                throw new ApiException(data.Message);
            }
            if (null != data.Data && data.Data.ReceiveOrderPrice.Count > 0)
                return data.Data.ReceiveOrderPrice;
            else
                return null;

        }

        /// <summary>
        /// 获取未来天数的房价信息-中枢
        /// </summary>
        /// <param name="hotelId"></param>
        /// <param name="vNumber"></param>
        /// <param name="receiveOrderId"></param>
        /// <param name="continueDays"></param>
        /// <param name="isHalfDay"></param>
        /// <param name="nationalId"></param>
        /// <returns></returns>
        public Dictionary<DateTime, decimal> GetFutureOrderPrice(string hotelId, string vNumber, string receiveOrderId, int continueDays, bool isHalfDay, string nationalId)
        {
            Dictionary<DateTime, decimal> dictionary = null;
            String action = APICallFactory.CallAction(APICallFactory.APICallType.GeReceiveFutureOrderPrice);
            List<KeyValuePair<String, String>> pams = new List<KeyValuePair<String, String>>();
            pams.Add(new KeyValuePair<string, string>("ReceiveOrderId", receiveOrderId));
            pams.Add(new KeyValuePair<string, string>("VNumberCode", vNumber));

            // 酒店Id,PMS系统要求必传，但将来可能废弃。
            pams.Add(new KeyValuePair<string, string>("HotelId", hotelId));
            // 续住天数，至少值和最大值由接待单详情返回。 0.5天当1天处理
            pams.Add(new KeyValuePair<string, string>("ContinueDays", continueDays.ToString()));
            // 是否半天续住
            pams.Add(new KeyValuePair<string, string>("IsHalfDay", isHalfDay.ToString()));
            pams.Add(new KeyValuePair<string, string>("NationalId", nationalId));

            //完成用户在大促销环境中的登录操作。
            var response = CentralApi.GetResponse(action, pams, null, null);
            var responseJson = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            response = null;
            dictionary = ConvertTo(responseJson, null) as Dictionary<DateTime, decimal>;
            if (null != dictionary)
                dictionary = dictionary.OrderBy(r => r.Key).ToDictionary(r => r.Key, r => r.Value);
            return dictionary;
        }
    }
}
