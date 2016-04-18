using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using YeahTvHcsApi.Common.HttpParameterBinding;
using YeahTVApi.Common;

namespace YeahTvHcsApi.ViewModels
{
    public class PostParameters<T>
    {
       
        [JsonProperty("sign")]
        public string Sign { get; set; }
        [JsonProperty("server_id")]
        public string Server_Id { get; set; }
        [JsonProperty("deviceseries")]
        public string DeviceSeries { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        /// <summary>
        /// 利用key对内容进行加密操作并返回Base64
        /// </summary>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string EncryptContent(string key, string content)
        {
            var keyBype = Encoding.UTF8.GetBytes(key);
            var contentBype = Encoding.UTF8.GetBytes(content);

            var encryptValue = ARC4Managed.Transform(contentBype, keyBype);

            return Convert.ToBase64String(encryptValue);
        }
    }
}