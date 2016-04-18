
using System.Security.Claims;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public abstract class PaymentBase
    {
        public string Sign { get; set; }
        public string Message { get; set; }
        public string ResultCode { get; set; }
    }

    /// <summary>
    /// 回调
    /// </summary>
    public class PaymentInfo : PaymentBase
    {
        public string Data { get; set; }
    }

    public class PaymentCallBackData
    {
        public PaymentCallBackReturnInfo ReturnInfo { get; set; }
    }
    public class PaymentCallBackReturnInfo
    {
        public string OrderId { get; set; }
        public string NotifyTime { get; set; }
    }

    /// <summary>
    /// 返回二维码
    /// </summary>
    public class PaymentResponseInfo : PaymentBase
    {
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