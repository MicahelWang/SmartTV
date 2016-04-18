using System.Runtime.Remoting.Contexts;
using HTWebApi.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HZTVApi.Common;
using HZTVApi.Entity.Payment;
using Vidar.Common;

namespace HTWebApi
{
    /// <summary>
    /// Summary description for notity
    /// </summary>
    public class notity : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
             
            var handlerResult = string.Empty;
            var ht = context.Request;
            context.Response.ContentType = "text/plain";
            const string call_er = "website";
            string call_key = PubFun.GetAppSetting("htpcall_website");
            var ppd = new PayParameterDto();
            ppd.Result = ht.QueryString["result"].IsNullOrEmptyError();
            ppd.notify_id = ht.QueryString["notify_id"].IsNullOrEmptyError();

            string valid_result = string.Empty;
            if (HTPayCenter.VarifyPayNotifyRequest(ht.QueryString, call_er, call_key, ref valid_result))  //验证支付发过来的消息，签名是否正确。
            {
                ppd.Result = ht["result"];
                //支付成功
                ppd.pay_id = ht["pay_id"]; //支付交易号
                ppd.pay_uniqueid = ht["pay_uniqueid"]; //支付请求号
                ppd.cal_notifydata = ht["cal_notifydata"]; //订单号  
                ppd.pay_paygate = ht["pay_paygate"]; //支付方式 A=支付宝，T=财付通 
                ppd.pay_netway = ht["pay_netway"];

                //PMS订单信息
                ppd.res_resno = ht["res_resno"];
                ppd.res_cresno = ht["res_cresno"];
                ppd.res_amount = ht["res_amount"];
                ppd.res_assurtype = ht["res_assurtype"];
                ppd.res_vno = ht["res_vno"];
                ppd.res_hotelid = ht["res_hotelid"];
                ppd.pay_amount = ht["pay_amount"];
                ppd.laststep = int.Parse(ht["laststep"]); //上次异步通知页返回的步骤号，内容为数字，因为9代表成功，所以不成功时候，数字的范围值是0-8，等于零时表示第一次调用
                HTOutputLog.SaveInfo("支付回调-notity-ProcessRequest1", ppd.Result+"#"+ ppd.Serializer());
                context.Response.Redirect("SelfService/Index");
                //todo ,更新pms
                handlerResult =new PayNotityManager().PayAfterHandler(ppd);
            }
            HTOutputLog.SaveInfo("支付回调-notity-ProcessRequest2",valid_result+"#"+ handlerResult);
            context.Response.Write(handlerResult);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}