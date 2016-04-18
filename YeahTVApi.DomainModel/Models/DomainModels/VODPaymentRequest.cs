using System;

namespace YeahTVApi.DomainModel.Models
{
    public class VODPaymentRequest : BaseEntity<string>
    {
        public DateTime CreateTime { get; set; }
        public string OrderId { get; set; }
        public string OrderAmount { get; set; }
        public string PayInfo { get; set; }
        public string GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsDesc { get; set; }
        public string BizHotle { get; set; }
        public string BizRoom { get; set; }
        public string BizDevice { get; set; }
        public string BizMember { get; set; }
        public string NotifyUrl { get; set; }
        public string Memo { get; set; }
        public string Pid { get; set; }
        public string RequestSign { get; set; }
        public string ResultSign { get; set; }
        public string ResultMessage { get; set; }
        public string ResultCode { get; set; }
        public string ResultQrcodeUrl { get; set; }
    }
}