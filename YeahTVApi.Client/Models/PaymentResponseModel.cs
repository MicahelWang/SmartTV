
using System.Security.Claims;
using System.Threading.Tasks;

namespace YeahTVApi.Client.Models
{
    public class PaymentResponseModel
    {
        public string Sign { get; set; }
        public string Message { get; set; }
        public string ResultCode { get; set; }
        public PaymentResponseData Data { get; set; }
    }
    public class PaymentResponseData
    {
        public PaymentResponseReturnInfo ReturnInfo { get; set; }
    }
    public class PaymentResponseReturnInfo
    {
        public string QrcodeUrl { get; set; }
    }
}