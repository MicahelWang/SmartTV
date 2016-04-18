using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    /// <summary>
    /// 中枢订单状态
    /// </summary>
    public enum OrderState
    {
        [Description("未支付")]
        Unpaid = 0,
        [Description("支付中")]
        Paying = 1,
        [Description("支付失败")]
        Fail = 2,
        [Description("支付成功")]
        Success = 3,
        [Description("订单取消")]
        Cancel = 4
    }
    /// <summary>
    /// 网关返回状态
    /// </summary>
    public enum PayMentOrderState
    {
        [Description("成功")]
        Success = 1,
        [Description("失败")]
        Fail = -1,
    }
    /// <summary>
    /// 中枢返回TV视频点播相关状态
    /// </summary>
    public enum OrderResultType
    {
        [Description("支付成功")]
        Success = 0,
        [Description("未支付")]
        Unpaid = 1,
        [Description("错误")]
        Error = -1,
    }
}