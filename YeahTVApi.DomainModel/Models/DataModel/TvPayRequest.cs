using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public class TvPayRequest
    {
        public string OrderId { get; set; }

        public string PayPaymentModel { get; set; }
    }
}