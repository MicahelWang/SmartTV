using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public class OrderProducts : BaseEntity<string>
    {
        public string OrderId { get; set; }
        [JsonConverter(typeof(StringDateTimeConverter))]
        public DateTime CreateTime { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public string ProductInfo { get; set; }

        [JsonIgnore]
        public virtual StoreOrder StoreOrder { get; set; }
    }
}