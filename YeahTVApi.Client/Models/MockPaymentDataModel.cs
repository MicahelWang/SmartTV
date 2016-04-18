
using System.Security.Claims;
using System.Threading.Tasks;

namespace YeahTVApi.Client.Models
{
    public class MockPaymentDataModel
    {
        public string RequestUrl { get; set; }
        public string RequestData { get; set; }
        public string Pid { get; set; }
        public string PrivateKey { get; set; }
        public string ResponseData { get; set; }
    }

    public class MockPaymentCallBackData : MockPaymentDataModel
    {
        public string Message { get; set; }
        public string ResultCode { get; set; }
    }
}