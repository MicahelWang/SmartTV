using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 支付宝令牌数据
    /// </summary>
    public class AlipayTokenData
    {
        /// <summary>
        /// 支付宝存续期间的短Token
        /// </summary>
        public string AccessToken;
        /// <summary>
        /// 支付宝存续期间的长Token
        /// </summary>
        public String RefreshToken;
        /// <summary>
        /// 支付宝长Token的有效时间，格式为YYYYMMDDHHMM，如果超过这个时间则需要重新调用接口进行刷新
        /// </summary>
        public String ExpiresInTime;
    }
}
