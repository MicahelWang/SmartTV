using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum Transactionstate
    {
        [Description("请选择交易状态")]
        All = -1,
        [Description("支付完成,待配送")]
        Paid = 1,
        [Description("等待前台支付")]
        Waiting = 2,
        [Description("未支付")]
        Unpaid = 3,
        [Description("已完成")]
        Transactionscomplete = 4,
        [Description("已取消")]
        Cancel = 5
    }
}
