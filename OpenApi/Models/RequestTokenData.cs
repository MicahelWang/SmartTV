using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenApi.Models
{
    /// <summary>
    /// 根据签名颁发token的请求参数实体
    /// </summary>
    public class RequestTokenData
    {
        public string authTicket { get; set; }
        public string code { get; set; }
        public int type { get; set; }
        public string sign { get; set; }
        public string signTime { get; set; }
        public int expiredMinus { get; set; }
    }
}