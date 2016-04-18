using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum DeliveryState
    {
        [Description("已配送")]
        Delivery = 1,
        [Description("未配送")]
        UnDelivery = 0
     
    }
}
