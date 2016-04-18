using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;

using YeahTVApi.Entity;
using YeahTVApi.Common;
using Newtonsoft.Json.Linq;
/// <summary>
/// Summary description for dianping
/// </summary>
public class DianpingOpen
{
    //public const string APIKEY = "959894442";
    //public const string AppSecret = "2844226f5584402484226e244388ded0";
    //public const string SERVER_NAME = "http://api.dianping.com/";
    /// <summary>
    /// URL请求参数UTF8编码
    /// </summary>
    /// <param name="value">源字符串</param>
    /// <returns>编码后的字符串</returns> 
    private static string Utf8Encode(string value)
    {
        return HttpUtility.UrlEncode(value, System.Text.Encoding.UTF8);
    }
    /// <summary>
    /// 加密参数
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string SHA1(string source)
    {
        byte[] value = Encoding.UTF8.GetBytes(source);
        SHA1 sha = new SHA1CryptoServiceProvider();
        byte[] result = sha.ComputeHash(value);

        string delimitedHexHash = BitConverter.ToString(result);
        string hexHash = delimitedHexHash.Replace("-", "");

        return hexHash;
    }
    /// <summary>
    /// 请求信息
    /// </summary>
    /// <param name="MethodName"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string MakeRequest(string MethodName, SortedList<string, string> list)
    {

        //组装url
        string url =YeahTVApi.Common.PubFun.GetAppSetting("dianping.apiurl") + MethodName;
        //组装参数
        if (list == null)
        {
            list = new SortedList<string, string>();
        }

        //编码
        string[] pairs = new string[list.Count];

        var value = "";
        var queryString = "";
        for (int i = 0; i < list.Count; i++)
        {
            // pairs[i] = list.Keys[i] + "=" + System.Web.HttpUtility.UrlEncode(list.Values[i]);
            value += list.Keys[i] + list.Values[i].ToString();
            queryString += "&" + list.Keys[i] + "=" + Utf8Encode(list.Values[i].ToString());

        }
        String apiKey = YeahTVApi.Common.PubFun.GetAppSetting("dianping.apikey");
        StringBuilder sb = new StringBuilder();
        sb.Append(apiKey);
        sb.Append(value);
        sb.Append(YeahTVApi.Common.PubFun.GetAppSetting("dianping.apiSecret"));
        value = sb.ToString();
        try
        {
            System.Net.WebRequest wReq = System.Net.WebRequest.Create(url + "?sign=" + SHA1(value) + "&appkey=" + apiKey + queryString);
            // Get the response instance.
            System.Net.WebResponse wResp = wReq.GetResponse();
            System.IO.Stream respStream = wResp.GetResponseStream();
            // Dim reader As StreamReader = New StreamReader(respStream)
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding("utf-8")))
            {
                return reader.ReadToEnd();
            }
        }
        catch (System.Exception ex)
        {
            //errorMsg = ex.Message;
        }
        return "";




    }
    /// <summary>
    /// 按照指定条件搜索并获取商户信息列表
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cityid">城市ID</param>
    /// <param name="regionid">区域ID（行政区或商区）</param>
    /// <param name="categoryid">分类ID（频道或各级分类）</param>
    /// <param name="keyword">搜索关键字</param>
    /// <param name="price">价格区间(¥): 1-[0, 20], 2-[21, 50], 3-[51, 80],4-[81, 120], 5-[121, 200], 6-200以上</param>
    /// <param name="hascoupon">是否有优惠券：1代表有，0代表没有</param>
    /// <param name="latitude">纬度坐标</param>
    /// <param name="longitude">经度坐标</param>
    /// <param name="radius">半径：单位米，最小值1，最大值5000</param>
    /// <param name="sort">结果排序方式：1-默认，2-热门度，3-星级，4-综合分，5-口味/产品/效果分，6-环境分，7-服务分，8-人均价格，9-优惠券，10-点评数，11-中心点距离</param>
    /// <param name="desc">排序升降序（默认降序）：0-降序，1-升序</param>
    /// <param name="limit">返回的商户结果条目数量：默认为20，最小值为1，最大值为50</param>
    /// <returns></returns>
    public static IList<DShopEntity> GetShops( string regionid, string categoryid, string keyword, string price, string hascoupon, string latitude, string longitude, string radius, string sort, string desc, string limit)
    {
        SortedList<string, string> list = new SortedList<string, string>();
        list["latitude"] = latitude;
        list["longitude"] = longitude;
        list["category"] = YeahTVApi.Common.PubFun.GetAppSetting("dianping.category." + categoryid);
        list["limit"] = limit;
        list["format"] = "json";
        list["offset_type"] = "1";
        String json = MakeRequest("v1/business/find_businesses", list);
        JObject obj = JObject.Parse(json);
       
        //JsonArrayObjects shops = JsonArrayObjects.Parse();
        DShopEntity shopEntity;
        IList<DShopEntity> objshops = new List<DShopEntity>();
        JToken status = null;
        Boolean success = obj.TryGetValue("status", out status);
        if (success && ((JValue)status).Value.Equals("OK"))
        {
            JToken token;
            success = obj.TryGetValue("businesses", out token);
            JArray array = (JArray)token;
            //JsonArrayObjects shoplist = JsonArrayObjects.Parse(shops[0]["businesses"]);
            foreach (var item in array)
            {
                shopEntity = new DShopEntity();
                shopEntity.shopName = item["name"].ToString();
                shopEntity.shopID = item["business_id"].ToString();
                shopEntity.address = item["address"].ToString();
                shopEntity.AvgPrice = Convert.ToInt32(item["avg_price"].ToString());
                shopEntity.categoryName = item["categories"].ToString();
                shopEntity.latitude = item["latitude"].ToString();
                shopEntity.longitude = item["longitude"].ToString();
                shopEntity.RedLen = double.Parse(item["distance"].ToString());
                shopEntity.PhotoURL = item["s_photo_url"].ToString();
                //shopEntity.starRating = item["avg_rating"].ToString();
               // shopEntity.RatingsImgUrl = item["rating_s_img_url"].ToString();
                shopEntity.urlWap = item["business_url"].ToString();
                shopEntity.distance = shopEntity.RedLen.ToString();
                objshops.Add(shopEntity);
            }
            return objshops;
        }
        return null;
    }
    /// <summary>
    /// 获取指定城市指定分类的下一级分类
    /// </summary>
    /// <param name="request"></param>
    /// <param name="city">城市名称</param>
    /// <returns></returns>
    public static IList<DShoptypeEntity> GetCategories()
    {
        SortedList<string, string> list = new SortedList<string, string>();
        String json = MakeRequest("v1/metadata/get_categories_with_businesses", list);
        JObject obj = JObject.Parse(json);

        //JsonArrayObjects shops = JsonArrayObjects.Parse();
        IList<DShoptypeEntity> listTypes = new List<DShoptypeEntity>();
        JToken status = null;
        Boolean success = obj.TryGetValue("status", out status);
        if (success && ((JValue)status).Value.Equals("OK"))
        {
            JToken token;
            success = obj.TryGetValue("categories", out token);
            JArray array = (JArray)token;

            foreach (var item in array)
            {
                DShoptypeEntity shoptype = new DShoptypeEntity();
                shoptype.CategoryID = item["category_name"].ToString();
                shoptype.CategoryName = item["category_name"].ToString();
                listTypes.Add(shoptype);
            }
            return listTypes;
        }
        return null;
    }


    /// <summary>
    /// 计算两点的距离
    /// </summary>
    /// <param name="latitudeA"></param>
    /// <param name="longitudeA"></param>
    /// <param name="latitudeB"></param>
    /// <param name="longitudeB"></param>
    /// <returns></returns>
    public static double RadLen(double latitudeA, double longitudeA, double latitudeB, double longitudeB)
    {
        try
        {
            double latRadians1 = latitudeA * Math.PI / 180;
            double latRadians2 = latitudeB * Math.PI / 180;
            double latRadians = latRadians1 - latRadians2;
            double lngRadians = longitudeA * Math.PI / 180 - longitudeB * Math.PI / 180;
            double f = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(latRadians / 2), 2) + Math.Cos(latRadians1) * Math.Cos(latRadians2) * Math.Pow(Math.Sin(lngRadians / 2), 2)));
            return Math.Round(f * 6378137);
        }
        catch
        {
            return 0;
        }

    }
}

