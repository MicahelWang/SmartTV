namespace YeahTVApi.ServiceProvider
{
    using YeahTVApi.Common;
    using YeahTVApi.Entity;
    using YeahTVApi.Infrastructure;
    using Newtonsoft.Json;
    using System;
    using YeahTVApi.DomainModel.Enum;

    public class CentralGetwayServiceBase : ICentralGetwayServiceBase
    {
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
        public CentralGetwayServiceBase(Type sourceType)
        {
            this.sourceType = sourceType;
        }

        /// <summary>
        /// 此方法必须被重载以实现相应转换功能
        /// </summary>
        /// <returns></returns>
        public virtual Object ConvertTo(String json, Guest guest)
        {
            try
            {
                return json.JsonStringToObj(sourceType);
            }
            catch (Exception err)
            {
              
            }

            return null;
        }
    }
}
