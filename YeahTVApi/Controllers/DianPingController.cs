using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApi.Controllers
{
    public class DianPingController : BaseController
    {
        // GET: DianPing
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IRequestApiService requestApiService;
        private readonly IRedisCacheService _cacheService;
        //private static string APP_KEY = "3716066064";
        //private static string APP_SECRET = "92b099f67fde4b439a4295f0de1b1b1d";
        private static string DIANPING_ADDRESS = "http://api.dianping.com/v1/business/find_businesses";
        private static string DIANPING_CATEGORY = "http://api.dianping.com/v1/metadata/get_categories_with_businesses";
        //private string radius = "5000";
        //private static string APP_KEY = constantSystemConfigManager.AppKey;
        //private static string APP_SECRET = constantSystemConfigManager.AppSecret;

        //private string radius = constantSystemConfigManager.RADIUS;

        public DianPingController(IConstantSystemConfigManager constantSystemConfigManager,
            IRequestApiService requestApiService,
            IRedisCacheService _cacheService)
        {
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.requestApiService = requestApiService;
            this._cacheService = _cacheService;
        }
        [HTApiFilter]
        public ApiObjectResult<object> GetDianPingData(string category)
        {
            string url;
            TimeSpan time = new TimeSpan(0, 10, 0);
            int status = 0;
            string resultJson = null;
            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + Header.HotelID;//
            var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();
            GetDianPingData(category, hotel, out url);
            string aroundKey = string.Format("TVLife_Category{0}_HotelId{1}", category, Header.HotelID);
            if (_cacheService.ContainsKey(aroundKey))
            {
                resultJson = _cacheService.Get<string>(aroundKey);
                string str = resultJson.JsonStringToObj<object>().ToString();
                status = JObject.Parse(str).GetValue("status").ToString().Equals("OK") ? 200 : 0;
            }
            else
            {
                resultJson = RequestUrl(url, out status);
                if (resultJson != null)
                {

                    string str = resultJson.JsonStringToObj<object>().ToString();
                    status = JObject.Parse(str).GetValue("status").ToString().Equals("OK") ? 200 : 0;
                    if (status != 0)
                        _cacheService.Add<object>(aroundKey, resultJson, time);
                }

            }
            object result = JsonConvert.DeserializeObject(resultJson);
            return new ApiObjectResult<object> { obj = result, ResultType = status };
        }

        [HTApiFilter]
        public ApiObjectResult<object> GetDianPingCategory()
        {
            int status = 0;
            TimeSpan time = new TimeSpan(0, 10, 0);
            string resultJson = null;
            Hashtable ht = new Hashtable();
            ht.Add("format", "json");
            string url = DIANPING_CATEGORY + "?" + CreateUrlProcess(ht);
            string categoryKey = string.Format("TVLife_AllCategory");
            if (_cacheService.ContainsKey(categoryKey))
            {

                resultJson = _cacheService.Get(categoryKey);
                string str = resultJson.JsonStringToObj<object>().ToString();
                status = JObject.Parse(str).GetValue("status").ToString().Equals("OK") ? 200 : 0;
            }

            else
            {
                resultJson = RequestUrl(url, out status);
                if (resultJson != null)
                {
                    string str = resultJson.JsonStringToObj<object>().ToString();
                    status = JObject.Parse(str).GetValue("status").ToString().Equals("OK") ? 200 : 0;
                    if (status != 0)
                        _cacheService.Set(categoryKey, resultJson, time);
                }
            }
            object result = JsonConvert.DeserializeObject(resultJson);
            return new ApiObjectResult<object> { obj = result, ResultType = status };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category">分类名称</param>
        /// <param name="radius"></param>
        /// <param name="hotel"></param>
        /// <param name="url"></param>
        private void GetDianPingData(string category, HotelEntity hotel, out string url)
        {

            Hashtable ht = new Hashtable();
            ht.Add("longitude", hotel.Longitude);
            ht.Add("latitude", hotel.Latitude);
            ht.Add("category", category);
            ht.Add("radius", constantSystemConfigManager.RADIUS);
            url = DIANPING_ADDRESS + "?" + CreateUrlProcess(ht);
        }

        private string CreateUrlProcess(Hashtable ht)
        {
            string value = "";
            string queryString = "";
            ArrayList akeys = new ArrayList(ht.Keys);
            akeys.Sort();
            //拼接字符串
            foreach (string skey in akeys)
            {
                value += skey + ht[skey].ToString();
                queryString += "&" + skey + "=" + Utf8Encode(ht[skey].ToString());
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(constantSystemConfigManager.AppKey);
            sb.Append(value);
            sb.Append(constantSystemConfigManager.AppSecret);
            value = sb.ToString();
            return string.Format("appkey={0}" + "&sign=" + SHA1(value) + queryString, constantSystemConfigManager.AppKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Utf8Encode(string value)
        {
            return HttpUtility.UrlEncode(value, System.Text.Encoding.UTF8);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SHA1(string source)
        {
            string result = BitConverter.ToString(
                SecurityManager.GetHash(SecurityManager.HashType.SHA1, source));
            string hexHash = result.Replace("-", "");
            return hexHash;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string RequestUrl(string url, out int status)
        {
            string result = null;
            status = 0;
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                }
                status = (int)response.StatusCode;
            }
            catch (WebException wexc1)
            {
                if (wexc1.Status == WebExceptionStatus.ProtocolError)
                {

                    status = (int)((HttpWebResponse)wexc1.Response).StatusCode;
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }
    }
}