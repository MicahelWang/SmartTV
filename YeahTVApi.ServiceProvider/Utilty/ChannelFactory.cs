namespace YeahTVApi.ServiceProvider
{
    using YeahTVApi.Common;
    using System;
    using System.Collections.Generic;

    public class ChannelFactory
    {
        private static Dictionary<String, String> dict = new Dictionary<string, string>();
        private static String Default_VnoHead = PubFun.GetAppSetting("app.guest.register.vnohead");
        private const String Defulat_VnoHead_Type="DEFAULT";

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
