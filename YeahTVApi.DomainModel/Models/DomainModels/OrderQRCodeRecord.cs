using System;
using System.Collections.Generic;

namespace YeahTVApi.DomainModel.Models
{
    public class OrderQRCodeRecord : BaseEntity<string>
    {
        public OrderQRCodeRecord()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }

        public string OrderId { get; set; }

        public string OrderType { get; set; }

        public string Ticket { get; set; }

        public DateTime CreateTime { get; set; }        
    }
}
