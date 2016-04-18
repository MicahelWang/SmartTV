using System;

namespace YeahTVApi.DomainModel.Models
{
    public partial class VODPaymentResult : BaseEntity<string>
    {
        public VODPaymentResult()
        {
            Id = Guid.NewGuid().ToString("N");
        }
        public DateTime CreateTime { get; set; }
        public string ResultSign { get; set; }
        public string ResultMessage { get; set; }
        public string ResultCode { get; set; }
        public string OrderId { get; set; }
        public string NotifyTime { get; set; }
    }
}