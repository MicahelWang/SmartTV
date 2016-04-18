using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 酒店支持信用卡视图
    /// </summary>
    public class HotelSupportCreditCardView
    {
       
        public string HotelID { get; set; }
       
        public int CardID { get; set; }
       
        public string CardName { get; set; }
       
        public string ImagePath { get; set; }
    }
}
