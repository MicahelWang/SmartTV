using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    /// <summary>
    /// 首字母必须小谢
    /// </summary>
    public class Paymentrequestdata
    {
        public OrderInfo orderInfo { get; set; }
    }
    public class OrderInfo
    {
        public OrderInfo()
        {
            payInfo = new Dictionary<string, string>();
        }

        public string orderId { get; set; }
        public string orderAmount { get; set; }
        public string goodsId { get; set; }
        public string goodsName { get; set; }
        public string goodsDesc { get; set; }
        public string bizHotle { get; set; }
        public string bizRoom { get; set; }
        public string bizDevice { get; set; }
        public string bizMember { get; set; }
        public string notifyUrl { get; set; }
        public string memo { get; set; }
        public Dictionary<string, string> payInfo { get; set; }
    }
}
