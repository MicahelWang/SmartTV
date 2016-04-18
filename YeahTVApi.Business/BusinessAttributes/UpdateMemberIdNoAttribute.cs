using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZTVApi.Entity;
using HZTVApi.Entity.CentralMapping;
using HZTVApi.Common;
using HZ.Web.Authorization;

namespace HZTVApi.Business.BusinessAttributes
{
    /// <summary>
    /// 酒店详情转换类
    /// </summary>
    public class UpdateMemberIdNoAttribute:BusinessAttribute
    {
        /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public UpdateMemberIdNoAttribute()
            : base(typeof(CentralApiResult<String>))
        {
            
        }

        public override Object ConvertTo(string json, Guest guest)
        {
            var result = base.ConvertTo(json, guest);
            if (result == null)
                return false;
            var data = result as CentralApiResult<String>;
            if (data == null)
                return false;
            if (data.ResultType != Entity.CentralMapping.OperationResultType.Successed)
            {
                //HTinns.HTLog4net.Log(this.ToString(), data.Message + "", json);
                throw new ApiException(data.Message);
            }
            return true;
        }



        public Boolean Update(BaseRequestData data, string pIDType, string pIDNo)
        {
            Dictionary<String, String> pams = new Dictionary<string, string>();
            pams.Add("pIDType", pIDType);
            pams.Add("pIDNo", pIDNo);
            //完成用户在大促销环境中的登录操作。
            var response = CentralApi.GetResponse(APICallFactory.CallAction(APICallFactory.APICallType.UpdateMemberIdNo), pams, data.TOKEN, data.language);
            var responseJson = response.Content.ReadAsStringAsync().Result;
            response.Dispose();
            response = null;
            return (Boolean)ConvertTo(responseJson,null);
        }


    }
}
