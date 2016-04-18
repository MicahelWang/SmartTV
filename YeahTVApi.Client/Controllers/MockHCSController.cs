using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Newtonsoft.Json;
using YeahTVApi.Client.Models;
using YeahTVApi.Common;
using YeahTvHcsApi.ViewModels;
using DictRequest = YeahCentreApi.ViewModels.DictRequest;
using PermitionRequest = YeahCentreApi.ViewModels.PermitionRequest;
using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;
using YeahTVApi.DomainModel.Models.PointsModels;
using YeahTVApi.DomainModel.SearchCriteria;
using OpenApi.Models;
namespace YeahTVApi.Client.Controllers
{
    [ValidateInput(false)]
    public class MockHCSController : Controller
    {
        private const string _IP = "192.168.66";//"192.168.66";

        public ActionResult Index()
        {
            //var model = new MockPaymentDataModel
            //{
            //    RequestUrl = "https://115.239.196.15:8080/PayGateway/charge.do",
            //    Pid = "1d4b534e-f0f3-493b-9142-f778973b19e2",
            //    PrivateKey = "sstyxmh3erotgsjpbgukdz2y5ddedfg3",
            //    RequestData =
            //        "{\"orderInfo\":{\"goodsId\":\"1\",\"bizHotle\":\"1\",\"bizRoom\":\"1\",\"memo\":\"1\",\"payInfo\":{\"ALIPAY\":\"0.01\"},\"orderAmount\":\"0.01\",\"bizMember\":\"1\",\"notifyUrl\":\"" + ("http://192.168.8.66:8003/PayMent/VodPaymentCallBack") + "\",\"bizDevice\":\"1\",\"goodsDesc\":\"1\",\"goodsName\":\"1\",\"orderId\":\"1\"}}"
            //};
            //return View(model);

            MockHCSRequestData model = new MockHCSRequestData();

            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Index(MockHCSRequestData model)
        {
            try
            {
                //var sign =
                //    new StringBuilder().Append(model.RequestData)
                //        .Append(model.PrivateKey)
                //        .ToString()
                //        .StringToMd5();

                string url;
                switch (model.ApiName)
                {
                    case "请求下载任务接口":

                        url = string.Format("http://{0}/YeahTvHcsApi/api/Task/AllTask", _IP);
                        break;

                    case "业务通知接口":

                        url = string.Format("http://{0}/YeahTvHcsApi/api/Task/TaskStatusNotify", _IP);
                        break;

                    case "系统错误通知接口":

                        url = string.Format("http://{0}/YeahTvHcsApi/api/ErrorLogInfo/ErrorNotify", _IP);
                        break;

                    case "测试数据通知接口":

                        url = string.Format("http://{0}/YeahTvHcsApi/api/TestDataNotify/TestDataNotify", _IP);
                        break;

                    case "性能数据传输接口":

                        url = string.Format("http://{0}/YeahTvHcsApi/api/PerformanceDataNotify/PerformanceDataNotify", _IP);
                        break;

                    case "获取HCS全局变量配置接口":

                        url = string.Format("http://{0}/YeahTvHcsApi/api/GlobalConfig/GlobalConfig", _IP);
                        break;

                    default:
                        url = string.Empty;
                        break;
                }

                url = model.Url;

                string apiResult;
                string sign;
                //model.RequestData = string.Format("pid={0}&sign={1}&data={2}", model.Pid, sign, model.RequestData);
                if (model.Url.Contains("AllTask"))
                {

                    PostParameters<PostTaskData> postTaskParameter;
                    postTaskParameter = JsonConvert.DeserializeObject<PostParameters<PostTaskData>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postTaskParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postTaskParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postTaskParameter));
                }
                else if (model.Url.Contains("GlobalConfig"))
                {
                    PostParameters<PostGlobalConfigData> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<PostGlobalConfigData>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("VodPayment"))
                {
                    PostParameters<VODPayment> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<VODPayment>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("DataDictionarySearching"))
                {
                    PostParameters<DictRequest> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<DictRequest>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("OrderStateChanging"))
                {
                    PostParameters<YeahCentreApi.ViewModels.StoreOrderState> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahCentreApi.ViewModels.StoreOrderState>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("OrderSearching"))
                {
                    PostParameters<YeahCentreApi.ViewModels.SearchCondition> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahCentreApi.ViewModels.SearchCondition>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("CategoryAction"))
                {
                    PostParameters<string> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<string>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("GetHotelListByPermition"))
                {
                    PostParameters<PermitionRequest> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<PermitionRequest>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("CommdityInfoAction"))
                {
                    PostParameters<YeahTVApi.DomainModel.Models.ViewModels.CommdityInfo> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahTVApi.DomainModel.Models.ViewModels.CommdityInfo>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("OrderInfoAction"))
                {
                    PostParameters<YeahTVApi.DomainModel.Models.ViewModels.ProductList> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahTVApi.DomainModel.Models.ViewModels.ProductList>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("GetHotelByDeviceId"))
                {
                    PostParameters<YeahCentreApi.ViewModels.PostHotelInfoData> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahCentreApi.ViewModels.PostHotelInfoData>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("GetWeather"))
                {
                    PostParameters<object> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<object>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("AddBehaviorLog"))
                {
                    PostParameters<BehaviorLogParameters> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<BehaviorLogParameters>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("Initialize"))
                {
                    PostParameters<YeahTVApi.DomainModel.Models.YeahHcsApi.RequestHcsHeader> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahTVApi.DomainModel.Models.YeahHcsApi.RequestHcsHeader>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("GetVersion"))
                {
                    PostParameters<YeahTVApi.DomainModel.Models.YeahHcsApi.PostAppsListParameters> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahTVApi.DomainModel.Models.YeahHcsApi.PostAppsListParameters>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("Start"))
                {

                    PostParameters<YeahTVApi.DomainModel.Models.YeahHcsApi.RequestHcsHeader> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahTVApi.DomainModel.Models.YeahHcsApi.RequestHcsHeader>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("GetDeviceApps"))
                {

                    PostParameters<YeahTVApi.DomainModel.Models.YeahHcsApi.PostAppsListParameters> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahTVApi.DomainModel.Models.YeahHcsApi.PostAppsListParameters>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("UploadLog"))
                {

                    PostParameters<YeahInfoLog> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<YeahInfoLog>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("GetFileInfo"))
                {

                    PostParameters<string> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<string>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("GetFilesInfo"))
                {

                    PostParameters<string[]> postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostParameters<string[]>>(model.Body);
                    sign = new StringBuilder().Append(JsonConvert.SerializeObject(postGlobalParameter.Data)).Append(model.PrivateKey).ToString().StringToMd5();
                    postGlobalParameter.Sign = sign;
                    apiResult = (new HttpClient()).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("TokenVerification"))
                {
                    RequestTokenParameter postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<RequestTokenParameter>(model.Body);
                    apiResult = (new HttpClient() { ContentType = "application/json" }).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }

                else if (model.Url.Contains("GetOrder"))
                {

                    PostScore postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<PostScore>(model.Body);
                    apiResult = (new HttpClient() { ContentType = "application/json" }).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("EnsureOrder"))
                {
                    RequestScore postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<RequestScore>(model.Body);
                    apiResult = (new HttpClient() { ContentType = "application/json" }).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("PromulgateToken"))
                {

                    ValidateSignCriteria postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<ValidateSignCriteria>(model.Body);

                    postGlobalParameter.Sign = (postGlobalParameter.Ticket + postGlobalParameter.MemberId + postGlobalParameter.Score + model.PrivateKey).StringToMd5();

                    apiResult = (new HttpClient() { ContentType = "application/json" }).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));
                }
                else if (model.Url.Contains("GetToken"))
                {
                    RequestTokenData postGlobalParameter;
                    postGlobalParameter = JsonConvert.DeserializeObject<RequestTokenData>(model.Body);

                    postGlobalParameter.sign = (postGlobalParameter.authTicket + postGlobalParameter.signTime + model.PrivateKey).StringToMd5();

                    apiResult = (new HttpClient() { ContentType = "application/json" }).PostJson(url, JsonConvert.SerializeObject(postGlobalParameter));

                }
                //else if (model.Url.Contains("GetToken"))
                //{

                //    ValidateSignCriteria postGlobalParameter;
                //    postGlobalParameter = JsonConvert.DeserializeObject<ValidateSignCriteria>(model.Body);
                //    Dictionary<string, string> text = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.Body);
                //    var ticket = (postGlobalParameter.OrderId + postGlobalParameter.MemberId ).StringToMd5();
                //    var sign1 = (ticket + text["signTime"] + model.PrivateKey).StringToMd5();
                //    postGlobalParameter.Sign = (postGlobalParameter.OrderId + postGlobalParameter.MemberId + model.PrivateKey).StringToMd5();
                //    apiResult = (new HttpClient() { ContentType = "application/json" }).PostJson(url, JsonConvert.SerializeObject(
                //        new { authTicket = ticket, code = text["code"].ToInt(), type = text["code"], sign=sign1,
                //            signTime = text["signTime"], expiredMinus = text["expiredMinus"].ToInt() }));
                //}
                else
                {
                    apiResult = (new HttpClient()).PostJson(url, model.Body);
                }


                //StringToUnicode("http://192.168.8.66:8003/PayMent/VodPaymentCallBack");

                //var paymentResponseModel = JsonConvert.DeserializeObject<PaymentResponseModel>(apiResult);

                //var resultSign =
                //    ByteToHexStr(SecurityManager.GetHash(SecurityManager.HashType.MD5,
                //        paymentResponseModel.Data.ToJsonString() + model.PrivateKey));

                //var qrcodeUrl = Regex.Unescape(paymentResponseModel.Data.ReturnInfo.QrcodeUrl); //转义支付宝二维码地址

                model.ResponseData = apiResult;
            }
            catch (Exception ex)
            {
                model.ResponseData = ex.Message;
            }


            return View(model);
        }

        public static string StringToUnicode(string s)
        {
            var charbuffers = s.ToCharArray();
            byte[] buffer;
            var sb = new StringBuilder();
            for (var i = 0; i < charbuffers.Length; i++)
            {
                buffer = Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(string.Format(@"\u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }

        /// <summary>
        ///     字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] bytes)
        {
            var returnStr = "";
            if (bytes != null)
            {
                for (var i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
    }
}
