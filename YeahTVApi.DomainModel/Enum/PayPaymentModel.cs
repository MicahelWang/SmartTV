using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    public enum PayPaymentModel
    {
        [Description("支付宝")]
        ALIPAY = 0,
        [Description("微信")]
        WXPAY = 1,
        [Description("挂房账")]
        FZPAY = 2,
        [Description("前台支付")]
        QTPAY = 3,
        [Description("积分支付")]
        JFPAY = 4
    }
}