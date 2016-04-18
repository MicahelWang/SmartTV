namespace YeahTVApi.DomainModel
{
    using System.Collections.Generic;
    using System.Configuration;

    public static partial class Constant
    {
        public static class SessionKey
        {
            public const string CurrentUser = "YeahCurrentUser";
        }
        public static List<string> NeedTransactionMethodNames
        {
            get { return new List<string> { "Insert", "Update", "Delete", "LogDeviceTrace" }; }
        }

        public static string BaiduWeatherURL
        {
            get { return ConfigurationManager.AppSettings["BaiduWeatherURL"]; }
        }

        public static int CacheInterval
        {
            get { return int.Parse(ConfigurationManager.AppSettings["CacheInterval"]); }
        }

        public static int ExpiresMinutes
        {
            get { return int.Parse(ConfigurationManager.AppSettings["ExpiresMinutes"]); }
        }

        public static int HttpPort
        {
            get { return int.Parse(ConfigurationManager.AppSettings["httpPort"]); }
        }

        public static int HttpsPort
        {
            get { return int.Parse(ConfigurationManager.AppSettings["httpsPort"]); }
        }

        public static string ResourceSiteAddress
        {
            get { return ConfigurationManager.AppSettings["ResourceSiteAddress"]; }
        }

        public static bool IsDebugModel
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["IsDebug"]); }
        }

        public static string ResourceTemplateWeatherSiteAddress
        {
            get { return ConfigurationManager.AppSettings["ResourceSiteAddress"] + "Template/{0}/Weather/{1}"; }
        }

        public const string MongoConnectionStringName = "MongoConnectionString";
        public const string MongoDbNameString = "MongoDbName";
        public const string ColumnMembersCacheModelKey = "YeahTV_ColumnMembersCacheModel";

        public const string HtotelCacheKey = "YeahTV_HOTELDETAIL_";

        public const string NameOrConnectionString = "Name=YeahApi";

        public const string CitiesKey = "YeahTV_cities";

        public const int PageSize = 10;

        public const string AppsListKey = "YeahTV_AppVersionData";

        public const string SystemConfigKey = "YeahTV_Config_";
        public const string SystemWeatherKey = "YeahTV_Weather_";

        public const string CommonBreakfastServiceUrlKey = "CommonFramework_BreakfastService";
        public const string CommonBreakfastServiceExpiredDateKey = "CommonFramework_BreakfastServiceExpiredDate";
        public const string CommonAllPbulishApp = "ALL";
        public const string CommonAllPbulishAppKey = "YeahTV_AllPbulishAppKey";
        public const string CommonSystemConfigHeader = "YeahTV_SystemConfig_";
        //public const Dictionary<string, string> CommmonTVHotelConfig = new Dictionary<string, string>
        //{
             
        //};

        /// <summary>
        /// 入住人保存sessionkey的前缀
        /// </summary>
        public static string RoomServiceSessionGuestPre = "_ROOM_GUEST_LIST";
        /// <summary>
        /// 接待单sessionkey的前缀
        /// </summary>
        public static string RoomServiceSessionOrderPre = "_ROOM_RECEIVE_ORDER";
        /// <summary>
        /// 接待单会员信息
        /// </summary>
        public static string RoomServiceSessionMemberPre = "_ROOM_MEMBER";
        /// <summary>
        /// 操作跳转提示字符
        /// </summary>
        public static string RedirectMessage = "redirectMessage";

        #region 目录
        /// <summary>
        /// 虚拟路径
        /// </summary>
        private static readonly string VirtualPath = ConfigurationManager.AppSettings["VirtualPath"] ?? string.Empty;

        /// <summary>
        /// 游戏目录
        /// </summary>
        private static readonly string GameDir = ConfigurationManager.AppSettings["GameDir"] ?? string.Empty;
        /// <summary>
        /// 游戏apk目录
        /// </summary>
        private static readonly string GameApkDir = ConfigurationManager.AppSettings["GameApkDir"] ?? string.Empty;
        /// <summary>
        /// 游戏Image目录
        /// </summary>
        private static readonly string GameImageDir = ConfigurationManager.AppSettings["GameImageDir"] ?? string.Empty;

        /// <summary>
        /// 首页目录
        /// </summary>
        private static readonly string HomePageDir = ConfigurationManager.AppSettings["HomePageDir"] ?? string.Empty;
        /// <summary>
        /// 小商品目录
        /// </summary>
        private static readonly string CommodityImageDir = ConfigurationManager.AppSettings["CommodityDir"] ?? string.Empty;
        /// <summary>
        /// 游戏apk虚拟路径
        /// </summary>
        public static readonly string GameApkVirtualPath = VirtualPath + GameDir.Replace(@"\", "/") + GameApkDir.Replace(@"\", "/");

        /// <summary>
        /// 游戏image虚拟路径
        /// </summary>
        public static readonly string GameImageVirtualPath = VirtualPath + GameDir.Replace(@"\", "/") + GameImageDir.Replace(@"\", "/");

        /// <summary>
        /// 小商品image虚拟路径
        /// </summary>
        public static readonly string CommodityImageVirtualPath = VirtualPath + CommodityImageDir.Replace(@"\", "/");

        public static readonly string UploadPhotoPhysicalPath = ConfigurationManager.AppSettings["UploadPhotoPhysicalPath"] ?? string.Empty;

        public static readonly string UploadVoicePhysicalPath = ConfigurationManager.AppSettings["UploadVoicePhysicalPath"] ?? string.Empty;
        #endregion


    }
}
