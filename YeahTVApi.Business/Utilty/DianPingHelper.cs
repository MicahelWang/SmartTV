using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HZTVApi.Common;
using HZTVApi.Entity;

/// <summary>
/// Summary description for DianPingHelper
/// </summary>
public class DianPingHelper
{






    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cityid"></param>
    /// <param name="categoryid"></param>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    public static List<DShopEntity> GetShopS(string categoryid, string latitude, string longitude, string sort)
    {
        var shopList = new List<DShopEntity>();
        try
        {
            var result = DianpingOpen.GetShops("", categoryid, "", "", "", latitude, longitude, "1000", sort, "1", "15");
            shopList = result.OrderBy(p => p.RedLen).ToList();         
        }
        catch (Exception err)
        {
            HTOutputLog.SaveError("GetShopS", err);
        }
        return shopList;
    }

   
}