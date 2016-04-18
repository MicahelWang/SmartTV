using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum BackState
    {
        [Description("成功")]
        Success = 1,
        [Description("参数错误")]
        PostDataError = -1,
        [Description("订单不存在")]
        NotFind = -2
    }
}
  

