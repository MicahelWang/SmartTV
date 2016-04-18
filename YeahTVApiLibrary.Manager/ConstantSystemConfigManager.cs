using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Manager
{
    public class ConstantSystemConfigManager : IConstantSystemConfigManager
    {
        private ISystemConfigRepertory systemConfigRepertory;
        private IRedisCacheManager redisCacheManager;

        public ConstantSystemConfigManager(ISystemConfigRepertory systemConfigRepertory, IRedisCacheManager redisCacheManager)
        {
            this.systemConfigRepertory = systemConfigRepertory;
            this.redisCacheManager = redisCacheManager;

            if (!redisCacheManager.IsSet(Constant.SystemConfigKey))
            {
                var configs = systemConfigRepertory.GetAll();
                redisCacheManager.Set(Constant.SystemConfigKey, configs);
            }
        }

        public string ResourceSiteAddress
        {
            get
            {
                return GetConfigValue("ResourceSiteAddress");
            }
        }

        public string AppCenterUrl
        {
            get
            {
                return GetConfigValue("AppCenterUrl");
            }
        }


        public string QinuiAk
        {
            get
            {
                return GetConfigValue("qiniuak");
            }
        }

        public string QinuiSk
        {
            get
            {
                return GetConfigValue("qiniusk");
            }
        }

        public string QinuiBucket
        {
            get
            {
                return GetConfigValue("qiniubucket");
            }
        }

        public string OpenApiAddress
        {
            get
            {
                return GetConfigValue("OpenApiAddress");
            }
        }

        public uint QiniuUploadTimeExpires
        {
            get { return uint.Parse(GetConfigValue("QiniuUploadTimeExpires")); }
        }
        public int VodOrderExpires
        {
            get { return int.Parse(GetConfigValue("VodOrderExpires")); }
        }
        public int VodDailyOrderExpires
        {
            get { return int.Parse(GetConfigValue("VodDailyOrderExpires")); }
        }
        public string VodPaymentSignKey
        {
            get { return GetConfigValue("VodPaymentSignKey"); }
        }
        public string VodPaymentPid
        {
            get { return GetConfigValue("VodPaymentPid"); }
        }

        public string VodDefaultPayInfo 
        {
            get { return GetConfigValue("VodDefaultPayInfo"); }
        }
        public string VodPaymentRequestUrl
        {
            get { return GetConfigValue("VodPaymentRequestUrl"); }
        }
        public string VodPaymentNotifyUrl
        {
            get { return GetConfigValue("VodPaymentNotifyUrl"); }
        }

        public string PaymentNotifyUrl
        {
            get { return GetConfigValue("PaymentNotifyUrl"); }
        }
        public string HSCPublicKey
        {
            get { return GetConfigValue("HCSPublicKey"); }
        }
		
        public string HotelPayment
        {
            get { return GetConfigValue("HotelPayment"); }
        }
        public string HCSTaskDefaultConfig
        {
            get { return GetConfigValue("HCSTaskDefaultConfig"); }
        }
        public string HCSGlobalDefaultConfig
        {
            get { return GetConfigValue("HCSGlobalDefaultConfig"); }
        }

        public string RADIUS
        {
            get { return GetConfigValue("RADIUS"); }
        }
        public string AppKey
        {
            get { return GetConfigValue("AppKey"); }
        }
        public string AppSecret
        {
            get { return GetConfigValue("AppSecret"); }
        }
        public string VodBackground
        {
            get { return GetConfigValue("VodBackground"); }
        }


        public string VodColor
        {
            get { return GetConfigValue("VodColor"); }
        }
        public int DashBoardValidityDays
        {
            get { return int.Parse(GetConfigValue("DashBoardValidityDays")); }
        }

        public string StoreSignPublicKey
        {
            get { return GetConfigValue("StoreSignPublicKey"); }
        }
        public string StoreSignPrivateKey
        {
            get { return GetConfigValue("StoreSignPrivateKey"); }
        }
        public string OpenAPIAuthSignPrivateKey
        {
            get { return GetConfigValue("OpenAPIAuthSignPrivateKey"); }
        }

        public string ShoppingMallUrl
        {
            get { return GetConfigValue("ShoppingMallUrl"); }
        }


        public int ExpirationDate
        {
            get { return int.Parse(GetConfigValue("ExpirationDate")); }
        }


        public string ShoppingOrderAddress
        {
            get { return GetConfigValue("ShoppingOrderAddress"); }
        }

        public string YeahInfoResourceSiteAddress
        {
            get { return GetConfigValue("YeahInfoResourceSiteAddress"); }
        }
        private string GetConfigValue(string configName)
        {
            return redisCacheManager.Get<List<SystemConfig>>(Constant.SystemConfigKey).Where(r => r.ConfigName.Equals(configName)).FirstOrDefault().ConfigValue;
        }
       
    }
}
