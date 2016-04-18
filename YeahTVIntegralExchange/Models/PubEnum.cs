using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace YeahTVIntegralExchange.Models
{ 
    public enum ApiErrorType
    {
        Success = 1,
        System = 10,
        Default = 100,
        SignError = 110,
        Parameter = 111,
        TokenError = 112,
        [Description("票据过期")]
        TicketExpired = 113,
        /// <summary>
        /// 不存在的APP版本或者
        /// </summary>
        NotExistVersion = 30001,
        NotLogin = 1001,
        NotLogin1 = 1002,
        MobileIsExist = 1003,

        //之分新添加枚举
        [Description("参数校验失败")]
        ParameterInvalid = 201,
        [Description("验签失败")]
        ValidationFailer = 202,
        [Description("该订单已存在")]
        OrderExist = 203,
        [Description("无此用户")]
        NotExistUser = 204,
        [Description("积分不够")]
        ScoreLack = 205,
        [Description("积分扣减异常")]
        ScoreAbnomal = 206,

    }
}