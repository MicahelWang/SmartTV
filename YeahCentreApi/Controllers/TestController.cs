using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentreApi.Controllers
{
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        [Route("GetToken")]
        [HttpPost]
        public HttpResponseMessage GetToken()
        {
            var url ="https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxdda85f16fad5513f&secret=cef5793842f73b5241f89365dcbb86fc";

            string resultHtml = (new HttpClient()).GetStringAsync(url).Result;
            var ticketObj = JsonConvert.DeserializeObject<TokenObject>(resultHtml);

            return new HttpResponseMessage { Content = new StringContent(ticketObj.AccessToken) };
        }
    }
    public class TokenObject
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
