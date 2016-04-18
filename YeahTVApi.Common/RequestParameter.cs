using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahTVApi.Common
{
    public class RequestParameter
    {

        /// <summary>
        /// 请求头对象
        /// </summary>
        public const String TRACE = "TRACE";

        /// <summary>
        /// 请求头对象
        /// </summary>
        public const String Header = "HEADER";

        /// <summary>
        /// 应用对象
        /// </summary>
        public const String APP = "APP";
        /// <summary>
        /// 用户对象
        /// </summary>
        public const String Guest = "Guest";

        /// <summary>
        /// 签名数据
        /// </summary>
        public const String Sign = "SIGN";

        /// <summary>
        /// 应用编号
        /// </summary>
        public const String APP_ID = "APP_ID";


        /// <summary>
        /// 数据对象
        /// </summary>
        public const String Data = "DATA";

        /// <summary>
        /// 返回密钥
        /// </summary>
        public const String ResultKey = "RESULTKEY";

        public const String HotelId = "hotelId";

        public const string Format = "format";

        public const string MockData = "MockData";
    }
}