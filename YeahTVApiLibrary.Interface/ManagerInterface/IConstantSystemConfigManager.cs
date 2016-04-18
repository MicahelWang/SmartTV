using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IConstantSystemConfigManager
    {
        string ResourceSiteAddress { get; }

        string AppCenterUrl { get; }

        string OpenApiAddress { get; }
        string QinuiAk { get; }
        string QinuiSk { get; }
        string QinuiBucket { get; }
        uint QiniuUploadTimeExpires { get; }
        int VodOrderExpires { get; }
        int VodDailyOrderExpires { get; }
        string VodPaymentSignKey { get; }
        string VodPaymentPid { get; }
        string VodDefaultPayInfo { get; }
        string VodPaymentRequestUrl { get; }
        string VodPaymentNotifyUrl { get; }
        string PaymentNotifyUrl { get; }
        string HSCPublicKey { get; }
        string VodBackground { get; }
        string VodColor { get; }

        string HotelPayment { get; }
        string HCSTaskDefaultConfig { get; }
        string HCSGlobalDefaultConfig { get; }
        int DashBoardValidityDays { get; }
        string RADIUS { get; }
        string AppKey { get; }
        string AppSecret { get; }
        string StoreSignPublicKey { get; }
        string StoreSignPrivateKey { get; }
        string OpenAPIAuthSignPrivateKey { get; }
        string ShoppingMallUrl { get; }
        int ExpirationDate { get; }
        string ShoppingOrderAddress { get; }
        string YeahInfoResourceSiteAddress { get; }
    }
}
