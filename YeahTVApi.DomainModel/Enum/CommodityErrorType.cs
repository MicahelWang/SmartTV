using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum CommodityErrorType
    {
        [Description("库存不足")]
        Lack = 0,
        [Description("数据异常")]
        Abnormal = 1,
        [Description("Success")]
        Success = 2,
        [Description("已下架")]
        OffShelf = 3,
        [Description("商品已删除")]
        Deleted = 4,
    }
}
