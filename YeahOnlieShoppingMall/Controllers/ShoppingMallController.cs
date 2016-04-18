using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahOnlieShoppingMall.Common;
using YeahOnlieShoppingMall.ViewModels;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Entity;
using YeahTVApi.DomainModel.Models;
using MallModel = YeahOnlieShoppingMall.ViewModels;
using Newtonsoft.Json;

namespace YeahOnlieShoppingMall.Controllers
{
    public class ShoppingMallController : Controller
    {

        public IDeviceTraceLibraryManager deviceTraceManager;
        public IAppLibraryManager appManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IRequestApiService requestApiService;
        private ILogManager LogManager;
        public ShoppingMallController(IConstantSystemConfigManager constantSystemConfigManager
            , ILogManager _LogManager
            , IRequestApiService requestApiService)
        {
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.LogManager = _LogManager;
            this.requestApiService = requestApiService;
        }
        private string GetSeris()
        {
            //return "00:00:00:00:00:01";
            string userAgent;
            userAgent = HttpContext.Request.UserAgent;
            var agentArray = userAgent.Split(';');
            UserAgentHeader header = JsonConvert.DeserializeObject<UserAgentHeader>(agentArray[agentArray.Length - 1]);
            return header.Device_No;
        }

        //GET: Index
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult GetCategorys()
        {
            string result = "", postData = "";
            try
            {
                CheckSignParameter sign = new CheckSignParameter(constantSystemConfigManager, LogManager);
                PostParameters<string> ps = new PostParameters<string>();
                ps.DeviceSeries = GetSeris();//"b8:88:e3:00:04:8f";
                ps.Data = "";
                ps.Sign = sign.GetSignData(JsonConvert.SerializeObject(ps.Data));
                postData = JsonConvert.SerializeObject(ps);
                var url = constantSystemConfigManager.AppCenterUrl + "/api/CommodityClassification/CategoryAction";
                HttpHelper http = new HttpHelper() { ContentType = "application/json" };
                result = http.Post(url, postData);
                if (result == "")
                {
                    LogManager.SaveError(new Exception("分类返回空值！"), string.Format("shopMall method GetCategorys,postData:{0},requestUrl:{1}", postData, url), YeahTVApi.DomainModel.Enum.AppType.TV, Request.Url.ToString());
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                PostParameters<CategoryReturnData> ret = JsonConvert.DeserializeObject<PostParameters<CategoryReturnData>>(result);
                ret.Data.Categorys.OrderBy(m => m.Index);
                return Json(JsonConvert.SerializeObject(ret), JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                LogManager.SaveError(err, string.Format("shopMall method GetCategorys,postData:{0},ActionResult:{1}", postData, result), YeahTVApi.DomainModel.Enum.AppType.TV, Request.Url.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult GetGoodList(MallModel.QueryByCategory data, string deviceSeries)
        {
            string postData = "", url = "";
            var result = "";
            try
            {
                CheckSignParameter sign = new CheckSignParameter(constantSystemConfigManager, LogManager);
                PostParameters<QueryByCategory> ps = new PostParameters<QueryByCategory>();
                ps.DeviceSeries = GetSeris();
                ps.Data = data;
                ps.Sign = sign.GetSignData(JsonConvert.SerializeObject(data));
                postData = JsonConvert.SerializeObject(ps);
                url = constantSystemConfigManager.AppCenterUrl + "/api/CommodityClassification/CommdityInfoAction";
                HttpHelper http = new HttpHelper() { ContentType = "application/json" };
                result = http.Post(url, postData);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                LogManager.SaveError(err, string.Format("shopMall method GetGoodList,postData:{0},ActionResult:{1},requestUrl:{2}", postData, result, url), YeahTVApi.DomainModel.Enum.AppType.TV, Request.Url.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [AjaxOnly]
        public JsonResult CreateOrder(MallModel.OrderProducts data)
        {
            string postData = "", result = "", url = "";
            try
            {
                foreach (var item in data.Products)
                {
                    if (item.Quantity <= 0)
                    {
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                }
                CheckSignParameter sign = new CheckSignParameter(constantSystemConfigManager, LogManager);
                PostParameters<MallModel.OrderProducts> ps = new PostParameters<MallModel.OrderProducts>();
                ps.DeviceSeries = GetSeris();
                ps.Data = data;
                ps.Sign = sign.GetSignData(JsonConvert.SerializeObject(data));
                postData = JsonConvert.SerializeObject(ps);
                url = constantSystemConfigManager.AppCenterUrl + "/api/CommodityClassification/OrderInfoAction";
                HttpHelper http = new HttpHelper() { ContentType = "application/json" };
                result = http.Post(url, postData);
                return Json(result);
            }
            catch (Exception err)
            {
                LogManager.SaveError(err, string.Format("shopMall method CreateOrder,postData:{0},ActionResult:{1},requestUrl:{2}", postData, result, url), YeahTVApi.DomainModel.Enum.AppType.TV, Request.Url.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult OrderDetail()
        {
            return View();
        }
        [HttpPost]
        [AjaxOnly]
        public JsonResult GetHotelInfo()
        {
            return Json("");
        }


    }
}