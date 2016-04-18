namespace HZTVApi.Business.BusinessAttributes
{
    using HZTVApi.Common;
    using HZTVApi.Entity;
    using HZTVApi.Infrastructure;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// 业务处理属性类
    /// </summary>
    public class BusinessAttribute
    {
        public IApiDBManager ApiDBManager;

        /// <summary>
        ///数据签名密钥
        /// </summary>
        protected static readonly String privateKey = PubFun.GetAppSetting("ApiPrivateKey");
        private Type sourceType;
        protected Type targetType;
        public String JsonString;

        public Type SourceType
        {
            get
            {
                return sourceType;
            }
        }

        /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public BusinessAttribute(Type sourceType)
        {
            this.sourceType = sourceType;
            this.ApiDBManager = new ApiDBManager();
        }

        /// <summary>
        /// 此方法必须被重载以实现相应转换功能
        /// </summary>
        /// <returns></returns>
        public virtual Object ConvertTo(String json, Guest guest)
        {
            try
            {
                return JsonConvert.DeserializeObject(json, sourceType);
            }
            catch (Exception err)
            {
                HTOutputLog.SaveError(this.GetType().Name, err, json);
            }

            return null;
        }
    }
}
