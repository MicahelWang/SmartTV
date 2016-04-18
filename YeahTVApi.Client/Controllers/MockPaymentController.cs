using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Newtonsoft.Json;
using YeahTVApi.Client.Models;
using YeahTVApi.Common;

namespace YeahTVApi.Client.Controllers
{
    [ValidateInput(false)]
    public class MockPaymentController : Controller
    {

        #region Payment

        // GET: MockRequest
        public ActionResult Payment()
        {
            var model = new MockPaymentDataModel
            {
                RequestUrl = "https://115.239.196.15:8080/PayGateway/charge.do",
                Pid = "1d4b534e-f0f3-493b-9142-f778973b19e2",
                PrivateKey = "sstyxmh3erotgsjpbgukdz2y5ddedfg3",
                RequestData =
                    "{\"orderInfo\":{\"goodsId\":\"1\",\"bizHotle\":\"1\",\"bizRoom\":\"1\",\"memo\":\"1\",\"payInfo\":{\"ALIPAY\":\"0.01\"},\"orderAmount\":\"0.01\",\"bizMember\":\"1\",\"notifyUrl\":\"" + ("http://192.168.8.66:8003/PayMent/VodPaymentCallBack") + "\",\"bizDevice\":\"1\",\"goodsDesc\":\"1\",\"goodsName\":\"1\",\"orderId\":\"1\"}}"
            };
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Payment(MockPaymentDataModel model)
        {
            try
            {
                var sign =
                    new StringBuilder().Append(model.RequestData)
                        .Append(model.PrivateKey)
                        .ToString()
                        .StringToMd5();


                StringToUnicode("http://192.168.8.66:8003/PayMent/VodPaymentCallBack");

                model.RequestData = string.Format("pid={0}&sign={1}&data={2}", model.Pid, sign, model.RequestData);

                var apiResult = (new HttpClient()).Get(model.RequestUrl, "", "", "", model.RequestData, "Post");


                StringToUnicode("http://192.168.8.66:8003/PayMent/VodPaymentCallBack");

                var paymentResponseModel = JsonConvert.DeserializeObject<PaymentResponseModel>(apiResult);

                var resultSign =
                    ByteToHexStr(SecurityManager.GetHash(SecurityManager.HashType.MD5,
                        paymentResponseModel.Data.ToJsonString() + model.PrivateKey));

                var qrcodeUrl = Regex.Unescape(paymentResponseModel.Data.ReturnInfo.QrcodeUrl); //转义支付宝二维码地址

                model.ResponseData = apiResult;
            }
            catch (Exception ex)
            {
                model.ResponseData = ex.Message;
            }


            return View(model);
        }

        #endregion

        #region PaymentCallBack

        public ActionResult PaymentCallBack()
        {
            var model = new MockPaymentCallBackData
            {
                RequestUrl = "http://{host}/api/PayMent/VodPaymentCallBack",
                Pid = "1d4b534e-f0f3-493b-9142-f778973b19e2",
                Message = "TRADE_SUCCESS",
                ResultCode = "1",
                PrivateKey = "sstyxmh3erotgsjpbgukdz2y5ddedfg3",
                RequestData =
                    "{\"returnInfo\":{\"orderId\":\"\",\"notifyTime\":\"\"}}"
            };
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult PaymentCallBack(MockPaymentCallBackData model)
        {
            try
            {
                var sign =
                    new StringBuilder().Append(model.RequestData)
                        .Append(model.PrivateKey)
                        .ToString()
                        .StringToMd5();

                model.RequestData = string.Format("pid={0}&sign={1}&data={2}&resultCode={3}&message={4}", model.Pid, sign, model.RequestData, model.ResultCode, model.Message);

                var apiResult = (new HttpClient()).Get(model.RequestUrl, "", "", "", model.RequestData, "Post");

                model.ResponseData = apiResult;
            }
            catch (Exception ex)
            {
                model.ResponseData = ex.Message;
            }


            return View(model);
        }

        #endregion

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