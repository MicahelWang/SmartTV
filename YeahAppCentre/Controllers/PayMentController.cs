using System.Web.Mvc;
using Newtonsoft.Json;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class PayMentController : Controller
    {
        private readonly IVODOrderManager vODOrderManager;

        public PayMentController(IVODOrderManager vOdOrderManager)
        {
            this.vODOrderManager = vOdOrderManager;
        }

        /// <summary>
        /// 1.查找订单是否存在，如果订单存在，更新数据库中的订单表,然后再将请求支付结果信息入库；否则返回Null。
        /// </summary>
        /// <param name="paymentInfo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [CheckSignFilter]
        public ActionResult VodPaymentCallBack(PaymentInfo paymentInfo)
        {
            var errorMessage = string.Empty;
            var paymentCallBackData = JsonConvert.DeserializeObject<PaymentCallBackData>(paymentInfo.Data);
            if (paymentCallBackData != null)
                vODOrderManager.PayMentCallBack(paymentInfo, paymentCallBackData);
            else
                errorMessage = "参数错误！";
            if (string.IsNullOrWhiteSpace(errorMessage))
                vODOrderManager.UpdateOrderCache();
            return Content(string.IsNullOrWhiteSpace(errorMessage) ? "Success" : errorMessage);
        }
    }
}