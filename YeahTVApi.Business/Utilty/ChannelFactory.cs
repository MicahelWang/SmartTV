using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZTVApi.Common;
using System.Web.Caching;
using System.Data;
using HZTVApi.Entity;

namespace HZTVApi.Business
{
    public class ChannelFactory
    {
        private static Dictionary<String, String> dict = new Dictionary<string, string>();
        private static String Default_VnoHead = PubFun.GetAppSetting("app.guest.register.vnohead");
        private const String Defulat_VnoHead_Type="DEFAULT";
        static ChannelFactory()
        {
            #region old logic
            //dict.Add("Appchina".ToUpper(), "YHQDBVK");//"应用汇
            //dict.Add("91Store".ToUpper(), "QDZSIOS");//91助手
            //dict.Add("hiapk".ToUpper(), "AZSCQDB");//	安卓市场
            //dict.Add("xiaomi".ToUpper(), "XMQDBVK");//	小米
            //dict.Add("QQ".ToUpper(), "TXQDBVK");//	腾讯应用中心
            //dict.Add("Google Play".ToUpper(), "GOOPLYV");//	google Play
            //dict.Add("360".ToUpper(), "SLYYQDB");//	360 应用
            //dict.Add("anzhi".ToUpper(), "AZQDBVK");//	安智
            //dict.Add("Nduo".ToUpper(), "NDQDBVK");//	N多
            //dict.Add("gfan".ToUpper(), "JFQDBVK");//	机锋网
            //dict.Add("alipay".ToUpper(), "MALIPAY");//	机锋网
            //dict.Add("huawei".ToUpper(), "HUAWEIZ");//	华为智汇云
            //dict.Add("wandoujia".ToUpper(), "WDJPKLB");//	豌豆荚
            //dict.Add("MM".ToUpper(), "ZGYDYYV");//	中国移动应用
            //dict.Add("xianxia".ToUpper(), "APPYZQD");//	APP线下预装
            //dict.Add("163".ToUpper(), "SHWYDDB");//	搜狐应用市场,网易应用市场		
            //dict.Add("mumayi".ToUpper(), "MMYDDBV");//	木蚂蚁
            //dict.Add("Shop store".ToUpper(), "YYSDQDB");//	OPPO应用商店,HTC应用商店,联想应用商店,三星应用商店,魅族应用中心		

            //dict.Add("oppo".ToUpper(), "YYSDQDB");//	OPPO应用商店,HTC应用商店,联想应用商店,三星应用商店,魅族应用中心		
            //dict.Add("HTC".ToUpper(), "YYSDQDB");//	OPPO应用商店,HTC应用商店,联想应用商店,三星应用商店,魅族应用中心		
            //dict.Add("Lenovo".ToUpper(), "LENEDBV");//	联想应用商店
            //dict.Add("unicom".ToUpper(), "LTQDBVK");//	联通
            //dict.Add("liantong".ToUpper(), "LTQDBVK");//	联通
            //dict.Add("UC".ToUpper(), "UCYZXQD");//	UC应用中心
            //dict.Add("All Store".ToUpper(), "SLQDVKH");//	可信应用,京东应用,优亿市场,手游天下	
            //dict.Add("baiduStore".ToUpper(), "BDIOSQD");//	可信应用,京东应用,优亿市场,手游天下	
            //dict.Add("KYStore".ToUpper(), "KYIOSQD");//	快用助手(ios)
            //dict.Add("App Store".ToUpper(), "APPSTOR");//	App Store
            //dict.Add("PPStore".ToUpper(), "PPZSQDB");//	pp助手(ios)
            //dict.Add("TongBuStore".ToUpper(), "TBZSQDB");//	同步助手(ios)
            //dict.Add("YZQD".ToUpper(), "APPYZQD");//	APP预装渠道
            //dict.Add("baidu".ToUpper(), "BDQDBTJ");//	APP预装渠道
            //dict.Add("XianxiaStore".ToUpper(), "IOSAPYZ");//	APP预装渠道
            //dict.Add("Samsung Store".ToUpper(), "SXYYAND");//	三星应用
            //dict.Add("baiducpd".ToUpper(), "BDCHVKH");//	APP预装渠道
            //dict.Add("wochacha".ToUpper(), "WOCHCHA");//	APP预装渠道
            //dict.Add("NYX Store".ToUpper(), "NOKIAQD");//	诺亚信应用
            //dict.Add("duowei Store".ToUpper(), "DWYYQDA");//	朵唯应用
            #endregion
        }

        public static String GetVhead(String key)
        {
            if (key == null)
            {
                return Default_VnoHead;
            }
            key= key.ToUpper().Replace(" ","");

            if (dict.ContainsKey(key))
            {
                return dict[key];
            }

            return Default_VnoHead;
        }

    }
}
