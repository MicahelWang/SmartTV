using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class ReceiveOrdersResult : OperationResult
    {


        /// <summary>
        /// 查询ReceiveOrders
        /// </summary>

        public List<ReceiveOrders> ReceiveOrders { get; set; }
    }
}
