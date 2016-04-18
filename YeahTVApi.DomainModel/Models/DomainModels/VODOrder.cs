using System;
using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models
{
    public class VODOrder : BaseEntity<string>
    {
        public DateTime CreateTime { get; set; }
        public decimal Price { get; set; }
        public int State { get; set; }
        public DateTime? CompleteTime { get; set; }
        public string VodRequestId { get; set; }
        public string MovieId { get; set; }
        public string SeriseCode { get; set; }
        public string RoomNo { get; set; }
        public string HotelId { get; set; }
        public string GoodsName { get; set; }
        public string GoodsDesc { get; set; }
        public string PayInfo { get; set; }
        public string PayType { get; set; }
        public bool IsDelete { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayInvoicesTitle_Name")]
        public string InvoicesTitle { get; set; }
    }
}