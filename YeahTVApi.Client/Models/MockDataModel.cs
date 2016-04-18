
using System.Security.Claims;
using System.Threading.Tasks;

namespace YeahTVApi.Client.Models
{
    public class MockRequestModel
    {
        public string RequestUrl { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public string SecureKey { get; set; }
        public string AppKey { get; set; }
        public string APP_ID { get; set; }
        public string Header { get; set; }
        public RequestHeader RequestHeader { get; set; }
    }

}