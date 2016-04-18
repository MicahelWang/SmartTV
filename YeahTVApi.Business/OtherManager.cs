namespace HZTVApi.Manager
{
    using HZTVApi.Entity;
    using HZTVApi.Infrastructure;
    using System;
    using System.Collections.Generic;

    public class OtherManager : IOtherManager
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneCode">手机号码</param>
        /// <param name="type">类型（模版）</param>
        /// <param name="info">提供模版所需要的数据</param>
        /// <returns></returns>
        public string SendSMS(string phoneCode, string type, Dictionary<string, string> info)
        {
            string strMsg = BusinessFun.LoadSMSContent(type, info);
            return HTSendEngine.Sms.SendSms(phoneCode, strMsg, type, "apiSys");
        }

        public FunResult GetExceptionResult(Exception exp)
        {
            FunResult rst = new FunResult();
            if (exp is ApiException)
            {
                ApiException e = exp as ApiException;
                rst.WithError(e.Message, e.ExceptionCode);
            }
            else
            {
                rst.WithError(exp.Message);
                //rst.WithError("系统忙，请稍后再试");
            }
            return rst;
        }
    }
}
