using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using YeahTVApi.Common;
//using YeahTVApiLibrary.Infrastructure;
using YeahTVIntegralExchange.Models;

namespace YeahTVIntegralExchange.Controllers
{
    public class JinJiangController : Controller
    {
        // GET: JinJiang

        private string YeahTVApi = "";
        private int TokenExpiredMinus = 60;
        private string orderid1 = "15120717482700000326";
        private string memberid1 = "112233";
        private string jjKey = "c4c88cb703454209bd0fcd5ee7d51055";
        public JinJiangController()
        {
            YeahTVApi = ConfigurationManager.AppSettings["YeahTVApi"].ToString();
            TokenExpiredMinus = int.Parse(ConfigurationManager.AppSettings["TokenExpiredMinus"].ToString());
        }
        [HttpGet]
        public ActionResult Index(OrderModels order)
        {
            order.CurScore = order.score;
            //var sign1 = (orderid1 + memberid1 + jjKey).StringToMd5();
            //order = new OrderModels() { orderid = orderid1, memberid = memberid1, sign = sign1, score = 0 };
            return View(order);
        }
        [HttpGet]
        public JsonResult GetToken(OrderModels order)
        {
            order.ExpiredMinus = TokenExpiredMinus;
            var postData = JsonConvert.SerializeObject(order);
            var url = YeahTVApi + "/api/ScoreExchange/PromulgateToken";
            HttpCommon http = new HttpCommon() { ContentType = "application/json" };
            var result = http.HttpPost(url, postData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetScore(OrderModels order)
        {
            var postData = JsonConvert.SerializeObject(order);
            HttpCommon http = new HttpCommon() { ContentType = "application/json" };
            var orderUrl = YeahTVApi + "/api/ScoreExchange/GetOrder";
            var result = http.HttpPost(orderUrl, postData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ExchangeScore(OrderModels order)
        {
            var postData = JsonConvert.SerializeObject(order);
            HttpCommon http = new HttpCommon() { ContentType = "application/json" };
            var orderUrl = YeahTVApi + "/api/ScoreExchange/EnsureOrder";
            var result = http.HttpPost(orderUrl, postData);
            return Json(result);
        }

        public ActionResult IntegralExchange_Order(OrderModels order)
        {
            return View();
        }
        public ActionResult IntegralExchange_Success(OrderModels order)
        {
            return View();
        }
        public ActionResult IntegralExchange_Fail()
        {
            return View();
        }
        public ActionResult IntegralExchange_Error()
        {
            return View();
        }
    }
     
}