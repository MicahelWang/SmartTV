using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class ReceiveOrderPriceResult : OperationResult
    {
        public Dictionary<DateTime, decimal> ReceiveOrderPrice { get; set; }
    }
}
