using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class ValidateSignCriteria
    {
        [JsonProperty("ticket")]
        public string Ticket { get; set; }
        [JsonProperty("score")]
        public string Score { get; set; }
        [JsonProperty("memberid")]
        public string MemberId { get; set; }
        [JsonProperty("sign")]
        public string Sign { get; set; }
        [JsonProperty("expiredMinus")]
        public int ExpiredMinus { get; set; }
    }
}
