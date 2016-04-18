using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum StoreEnum
    {
        [Description("成功")]
        Success = 000,
        [Description("参数校验失败")]
        ParameterInvalid = 101,
        [Description("验签失败")]
        ValidationFailer = 102,
        [Description("该订单已存在")]
        OrderExist = 103,
        [Description("无此用户")]
        NotExistUser = 104,
        [Description("积分不够")]
        ScoreLack = 401,
        [Description("积分扣减异常")]
        ScoreAbnomal = 402,
        [Description("系统异常")]
        System = 599

    }
}
