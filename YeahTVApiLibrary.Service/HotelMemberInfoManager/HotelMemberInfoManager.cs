using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;

namespace YeahTVApiLibrary.Service.HotelMemberInfoManager
{
    public class HotelJjMemberInfoManager : IHotelMemberInfoManager
    {
        private static Dictionary<string, string> _wxToken = new Dictionary<string, string>();
        private IGlobalConfigManager gloabalConfigManager;
        private IWeiXinService weiXinService;
        private IVODOrderManager vODOrderManager;
        private IOrderQRCodeRecordManager orderQRCodeRecordManager;
        private ILogManager logManager;
        private IRedisCacheManager cacheManager;

        public HotelJjMemberInfoManager(IGlobalConfigManager gloabalConfigManager,
            IWeiXinService weiXinService, IVODOrderManager vODOrderManager,
            IOrderQRCodeRecordManager orderQRCodeRecordManager, ILogManager logManager, IRedisCacheManager cacheManager)
        {
            this.gloabalConfigManager = gloabalConfigManager;
            this.weiXinService = weiXinService;
            this.vODOrderManager = vODOrderManager;
            this.orderQRCodeRecordManager = orderQRCodeRecordManager;
            this.logManager = logManager;
            this.cacheManager = cacheManager;
        }
        public ScorePointResult GetTheUserScore(string hotelId, params string[] parameters)
        {
            string url = gloabalConfigManager.GetHotelScoreGateWayUrl(hotelId);

            string jjUrl = string.Format("{0}/v1/member/score", url);
            var requestJj = new RequestJjDataBase
            {
                McCode = parameters[0],
                PartnerId = gloabalConfigManager.GetHotelScorePartnerId(hotelId),
                Reqtime = DateTime.Now.ToString("yyyyMMddHHmmss"),
            };
            requestJj.Sign = CreateSign(requestJj, hotelId);
            string returnStrMessage = (new HttpHelper() { ContentType = "application/json;charset=UTF-8" }).
                Post(jjUrl, JsonConvert.SerializeObject(requestJj));
            var returnMessage = JsonConvert.DeserializeObject<ScorePointResult>(returnStrMessage);
            return returnMessage;
        }
        private string CreateSign(RequestJjDataBase requestJj, string hotelId)
        {
            string key = gloabalConfigManager.GetHotelScoreGateWayBackSignKey(hotelId);//新建数据库获取
            string sortObj = PubFun.GetSortedObjectProperty(requestJj, new string[] { "sign" });
            string md5Sign = string.Format("{0}{1}", sortObj, key).StringToMd5();
            return md5Sign;
        }

        public PaymentResponseInfo GetScoreQRCode(string orderId, string hotelId)
        {
            //var hotelBusinessCode = gloabalConfigManager.GetHotelScoreBusinessCode(hotelId);
            //var sceneId = string.Format("{0}{1}", hotelBusinessCode, orderId);
            string sceneId = vODOrderManager.GetScoreOrderId(orderId, hotelId);
            string key = RedisKey.HotelToken + hotelId;

            Func<string, PaymentResponseInfo> createQrcodeFun = s =>
            {
                string token = string.Empty;

                if (cacheManager.IsSet(key))
                {
                    token = cacheManager.Get<string>(key);
                }
                else
                {
                    token = GetWxToken(hotelId, Guid.NewGuid().ToString("N"));
                    cacheManager.Set<string>(key, token);
                }


                var result = weiXinService.CreateQrcode(token, sceneId);

                if (result.Item1.ResultCode == PayMentOrderState.Success.GetValueStr())
                {
                    orderQRCodeRecordManager.Add(new OrderQRCodeRecord()
                    {
                        CreateTime = DateTime.Now,
                        OrderId = orderId,
                        OrderType = OrderType.VOD.ToString(),
                        Ticket = result.Item2
                    });
                }

                logManager.SaveInfo(string.Format("订单号：{0} 酒店ID：{1} 结果：{2}", orderId, hotelId, result != null ? result.ToJsonString() : ""), "获取微信二维码", AppType.CommonFramework);

                return result.Item1;
            };

            var paymentResponseInfo = createQrcodeFun(sceneId);

            if (paymentResponseInfo != null && paymentResponseInfo.ResultCode == PayMentOrderState.Fail.GetValueStr())
            {
                cacheManager.Remove(key);
                paymentResponseInfo = createQrcodeFun(sceneId);

                logManager.SaveInfo("", "获取微信二维码失败，重置WxToken", AppType.CommonFramework);
            }

            return paymentResponseInfo;
        }

        private string GetWxToken(string hotelId, string userId)
        {
            string token = string.Empty;
            string scoreGetTokenUrl = gloabalConfigManager.GetHotelScoreGetTokenUrl(hotelId);
            string signKey = gloabalConfigManager.GetHotelScoreGateWayFrontSignKey(hotelId);

            string postData = string.Format("userId={0}&sign={1}", userId, (userId + signKey).StringToMd5().ToUpper());
            var response = (new HttpClient()).PostAsync(scoreGetTokenUrl, new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                token = response.Content.ReadAsStringAsync().Result;
            }

            return token;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestScore"></param>
        /// <param name="vodOrder"></param>
        /// <param name="userScore"></param>
        /// <returns></returns>
        public Tuple<ScorePointResult, RequestJjData> CommitOrderRequest(RequestScore requestScore, VODOrder vodOrder, decimal userScore)
        {
            string url = gloabalConfigManager.GetHotelScoreGateWayUrl(vodOrder.HotelId);
            string jjUrl = string.Format("{0}/v1/score/pay", url);
            var requestJj = CreateEnsureOrderPost(requestScore, vodOrder, userScore);
            string returnStrMessage = (new HttpHelper() { ContentType = "application/json;charset=UTF-8" }).
                Post(jjUrl, JsonConvert.SerializeObject(requestJj));
            var returnMessage = JsonConvert.DeserializeObject<ScorePointResult>(returnStrMessage);
            return new Tuple<ScorePointResult, RequestJjData>(returnMessage, requestJj);
        }
        /// <summary>
        /// 生成请求订单接口的数据
        /// </summary>
        /// <param name="requestScore"></param>
        /// <param name="vodOrder"></param>
        /// <param name="userScore"></param>
        /// <returns></returns>
        private RequestJjData CreateEnsureOrderPost(RequestScore requestScore, VODOrder vodOrder, decimal userScore)
        {
            var requestJjSign = new RequestJjDataBase
            {
                McCode = requestScore.UserId,
                PartnerId = gloabalConfigManager.GetHotelScorePartnerId(vodOrder.HotelId),
                Reqtime = DateTime.Now.ToString("yyyyMMddHHmmss"),
            };
            var hotelBusinessCode = gloabalConfigManager.GetHotelScoreBusinessCode(vodOrder.HotelId);
            string channelSource = gloabalConfigManager.GetHotelScoreChannelsource(vodOrder.HotelId);
            var sceneId = string.Format("{0}{1}", hotelBusinessCode, vodOrder.Id);
            var requestJj = new RequestJjData
            {

                McCode = requestScore.UserId,
                Reqtime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                PartnerId = gloabalConfigManager.GetHotelScorePartnerId(vodOrder.HotelId),
                ChannelSource = string.IsNullOrWhiteSpace(channelSource) ? "" : channelSource,
                Amount = "0",
                ProductId = vODOrderManager.GetPorductId(vodOrder),
                OrderId = sceneId,
                ProductName = vodOrder.GoodsName,
                Score = Convert.ToInt32(userScore).ToString(),
                Ext1 = "",
                Ext2 = ""
            };
            requestJj.Sign = CreateSign(requestJj, vodOrder.HotelId);
            return requestJj;
        }
    }
}
