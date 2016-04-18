using Qiniu.IO;
using Qiniu.IO.Resumable;
using Qiniu.RS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahTVApiLibrary.Service
{
    public class WeiXinService : IWeiXinService
    {
        public Tuple<PaymentResponseInfo, string> CreateQrcode(string accessToken, string sceneId)
        {
            var paymentResponseInfo = new PaymentResponseInfo();
            var ticket = "";

            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(sceneId))
            {
                paymentResponseInfo.ResultCode = PayMentOrderState.Fail.GetValueStr();
                paymentResponseInfo.Message = string.Format("CreateQrcode参数错误,accessToken:{0} sceneId：{1}", accessToken, sceneId);
            }
            else
            {
                var url = string.Format("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}", accessToken);
                var postData = "{\"expire_seconds\": 604800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + sceneId + "}}}";

                var result = (new HttpClient()).PostAsync(url, new StringContent(postData, Encoding.UTF8, "application/json"))
                        .Result.Content.ReadAsStringAsync()
                        .Result;

                if (result.ToLower().Contains("\"ticket\":"))
                {
                    var ticketObj = Newtonsoft.Json.JsonConvert.DeserializeObject<WxTicket>(result);
                    ticket = ticketObj.TicketStr;
                    paymentResponseInfo.ResultCode = PayMentOrderState.Success.GetValueStr();
                    paymentResponseInfo.Data = new PaymentResponseData()
                    {
                        ReturnInfo = new PaymentResponseReturnInfo()
                        {
                            QrcodeUrl = string.Format(
                                   "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}",
                                   ticketObj.TicketStr)
                        }
                    };
                }
                else
                {
                    paymentResponseInfo.ResultCode = PayMentOrderState.Fail.GetValueStr();
                    paymentResponseInfo.Message = "获取微信二维码错误：" + result;
                }
            }

            return new Tuple<PaymentResponseInfo, string>(paymentResponseInfo, ticket);
        }
    }
}
