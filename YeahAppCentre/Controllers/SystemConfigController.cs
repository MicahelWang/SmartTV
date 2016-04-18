using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using System.Web.Helpers;
using Newtonsoft.Json;
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahAppCentre.Controllers
{
    public class SystemConfigController : BaseController
    {
        private ISystemConfigManager systemConfigManager;
        private ILogManager logManager;
        private IHotelManager hotelManager;

        public SystemConfigController(
           ISystemConfigManager systemConfigManager,
            ILogManager logManager,
              IHotelManager hotelManager,
            IHttpContextService httpContextService)
            : base(logManager, httpContextService)
        {
            this.systemConfigManager = systemConfigManager;
            this.logManager = logManager;
            this.hotelManager = hotelManager;
        }
        // GET: SystemConfig
        public ActionResult Index(SystemConfigCriteria systemConfigCriteria = null)
        {
            if (systemConfigCriteria == null)
            {
                systemConfigCriteria = new SystemConfigCriteria();
            }
            var partialViewResult = this.List(systemConfigCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            ViewBag.SystemConfigCriteria = systemConfigCriteria;
            return View();
        }


        public ActionResult List(SystemConfigCriteria systemConfigCriteria)
        {
            var list = new PagedViewList<SystemConfig>();

            systemConfigCriteria.NeedPaging = true;
            list.PageIndex = systemConfigCriteria.Page;
            list.PageSize = systemConfigCriteria.PageSize;
            list.Source = systemConfigManager.Search(systemConfigCriteria);
            list.TotalCount = systemConfigCriteria.TotalCount;

            return this.PartialView("List", list);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult AddOrEdit(int id, OpType type)
        {
            var systemConfigs = new List<SelectListItem>();

            var allTypeList = systemConfigManager.GetAllSysType();
            
            foreach (var item in allTypeList)
            {
                systemConfigs.Add(new SelectListItem() { Value = item.Key, Text = item.Value });
            }
            systemConfigs.Insert(0, new SelectListItem() { Value = "", Text = "==请选择==" });

            ViewBag.DefaultSelect = systemConfigs;

            //var newSystemConfigs = systemConfigManager.Search(new SystemConfigCriteria { });
            //ViewBag.DefaultSelect = newSystemConfigs.Select( n => new List<SelectListItem> 
            //{
            //    new SelectListItem
            //    {
                    
            //    }
            //});

            ViewBag.OpType = type;

            var systemconfig = new SystemConfig();
            switch (type)
            {

                case OpType.Add:
                    break;
                case OpType.View:
                case OpType.Update:
                    systemconfig = systemConfigManager.FindByKey(id);
                    break;
                default:
                    break;
            }

            return PartialView(systemconfig);
        }

        [HttpPost]
        public ActionResult AddOrEdit(OpType optype, SystemConfig systemconfig)
        {
            return base.ExecutionMethod(() =>
            {
                string errorMsg = string.Empty;
                try
                {
                   
                    systemconfig.LastUpdateTime = DateTime.Now;
                    if (optype == OpType.Add)
                    {
                        systemConfigManager.AddSystemConfig(systemconfig);
                    }
                    else if (optype == OpType.Update)
                    {
                        systemConfigManager.UpdateSystemConfig(systemconfig);
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
                
                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }


        #region EditHotelPayment


        [HttpGet]
        [AjaxOnly]
        public ActionResult EditHotelPayment(int id, OpType type)
        {
            ViewBag.OpType = type;

            var globalPayment = new GlobalPayment();
            switch (type)
            {
                case OpType.View:
                case OpType.Update:
                    var globalPaymentConfig = systemConfigManager.FindByKey(id);
                    globalPayment = JsonConvert.DeserializeObject<GlobalPayment>(globalPaymentConfig.ConfigValue);

                    ViewBag.PayPaymentModels = EnumExtensions.GetItems(typeof(PayPaymentModel)).Where(m => m.Value != (int)PayPaymentModel.QTPAY).Select(m => new SelectListItem()
                    {
                        Text = m.Description,
                        Value = m.Text,
                        Selected = globalPayment.PaymentModels.Any(p => p.Trim().ToLower().Equals(m.Text.Trim().ToLower()))
                    });

                    break;
            }

            return PartialView(globalPayment);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult EditHotelPayment(OpType optype, GlobalPayment hotelPayment)
        {
            return base.ExecutionMethod(() =>
            {
                string errorMsg = string.Empty;

                hotelPayment.PaymentModels =
                    hotelPayment.PaymentModels == null ? new List<string>() : hotelPayment.PaymentModels.Where(m => !m.ToLower().Equals("false")).ToList();

                if (hotelPayment.PaymentModels.Count == 0)
                    errorMsg = "请选择支付方式！";

                if (!hotelPayment.PaymentModels.All(m => Enum.GetNames(typeof(PayPaymentModel)).Any(p => p.ToLower().Equals(m.ToLower()))))
                    errorMsg = "支付方式错误！";

                if (hotelPayment.PayType == null)
                    hotelPayment.PayType = "";

                if (!string.IsNullOrWhiteSpace(hotelPayment.PayType) && !Enum.GetNames(typeof(PayType)).Any(m => m.ToLower().Equals(hotelPayment.PayType.ToLower())))
                    errorMsg = "支付类型错误！";

                if (!hotelPayment.PayType.ToLower().Equals(PayType.Daily.ToString().ToLower()))
                {
                    hotelPayment.PriceOfDay = 0.00m;
                }


                if (string.IsNullOrWhiteSpace(errorMsg) && optype == OpType.Update)
                {

                    var globalPaymentConfig = systemConfigManager.FindByKey(hotelPayment.Id);

                    globalPaymentConfig.LastUpdateTime = DateTime.Now;

                    globalPaymentConfig.ConfigValue = JsonConvert.SerializeObject(hotelPayment);

                    systemConfigManager.UpdateSystemConfig(globalPaymentConfig);
                }

                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }

        #endregion


    }
}