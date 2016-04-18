namespace HZTVApi.Manager
{
    using HZTVApi.Common;
    using HZTVApi.Entity;
    using HZTVApi.Infrastructure;
    using System.Collections.Generic;

    public class DianPingManager : IDianPingManager
    {
        public List<HTCategory> GetHTCategory()
        {
            List<HTCategory> rst = new List<HTCategory>();
            string info = PubFun.GetAppSetting("dianping.category.info");
            string[] infos = info.Split('|');

            foreach (string str in infos)
            {
                int idx = str.IndexOf(".");
                if (idx > 0)
                {
                    rst.Add(new HTCategory { Code = str.Substring(0, idx), Name = str.Substring(idx + 1) });
                }
            }
            return rst;
        }

        public List<DShopEntity> GetCityShopForApp(string categoryType, string latitude, string longitude)
        {
          return DianPingHelper.GetShopS(categoryType, latitude, longitude, "11");
        }
    
    }
}
