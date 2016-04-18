using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahCenter.Infrastructure;
using YeahTVApiLibrary.Infrastructure;
using System.Xml.Linq;
using Newtonsoft.Json;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahAppCentre.Controllers
{
    public class TVHotelConfigController : BaseController
    {
        private ITVHotelConfigManager tVHotelConfigManager;
        private ILogManager logManager;
        private IHotelManager hotelManager;

        public TVHotelConfigController(
            ITVHotelConfigManager tVHotelConfigManager,
            ILogManager logManager,
              IHotelManager hotelManager,
            IHttpContextService httpContextService)
            : base(logManager, httpContextService)
        {
            this.tVHotelConfigManager = tVHotelConfigManager;
            this.logManager = logManager;
            this.hotelManager = hotelManager;
        }

        // GET: TVHotelConfig
        public ActionResult Index(HotelConfigCriteria hotelConfigCriteria = null)
        {
            if (hotelConfigCriteria == null)
            {
                hotelConfigCriteria = new HotelConfigCriteria();
                hotelConfigCriteria.NeedPaging = true;

            }
            var partialViewResult = this.HotelList(hotelConfigCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            ViewBag.hotelConfigCriteria = hotelConfigCriteria;
            if (!string.IsNullOrEmpty(hotelConfigCriteria.HotelId))
            {
                var hotelName = hotelManager.GetHotel(hotelConfigCriteria.HotelId).HotelName;

                var hotelcode = hotelManager.GetHotel(hotelConfigCriteria.HotelId).HotelCode;
                ViewBag.HotelName = hotelName + "(" + hotelcode + ")";
            }




            return View();
        }
        [AjaxOnly]
        public ActionResult List(HotelConfigCriteria hotelConfigCriteria)
        {
            var list = new PagedViewList<TVHotelConfig>();
            hotelConfigCriteria.NeedPaging = false;
            if (hotelConfigCriteria.SortFiled.Equals("Id"))
            {
                hotelConfigCriteria.SortFiled = "LastUpdateTime";
                hotelConfigCriteria.OrderAsc = false;
            }
            list.PageIndex = hotelConfigCriteria.Page;
            list.PageSize = hotelConfigCriteria.PageSize;
            list.Source = tVHotelConfigManager.Search(hotelConfigCriteria);
            list.TotalCount = hotelConfigCriteria.TotalCount;

            return this.PartialView("List", list);
        }

        [AjaxOnly]
        public ActionResult HotelList(HotelConfigCriteria hotelConfigCriteria)
        {
            var list = new PagedViewList<string>();
            hotelConfigCriteria.NeedPaging = true;
            //if (hotelConfigCriteria.SortFiled.Equals("Id"))
            //{
            //    hotelConfigCriteria.SortFiled = "LastUpdateTime";
            //    hotelConfigCriteria.OrderAsc = false;
            //}
            list.PageIndex = hotelConfigCriteria.Page;
            list.PageSize = hotelConfigCriteria.PageSize;
            list.Source = tVHotelConfigManager.SearchOnlyHotelId(hotelConfigCriteria);
            list.TotalCount = hotelConfigCriteria.TotalCount;

            return this.PartialView("HotelList", list);
        }
        [HttpGet]
        [AjaxOnly]
        public ActionResult AddOrEdit(int id, OpType type)
        {
            ViewBag.OpType = type;
            GetTvhotelconfigTypeList();
            var tVHotelConfig = new TVHotelConfig { StorePayConfig = new StorePayConfig() };

            var payPaymentModels = EnumExtensions.GetItems(typeof(PayPaymentModel)).Where(m => m.Value != (int)PayPaymentModel.FZPAY).Select(m => new SelectListItem()
            {
                Text = m.Description,
                Value = m.Text,
            });

            switch (type)
            {

                case OpType.Add:
                    tVHotelConfig.Active = true;
                    break;
                case OpType.View:
                case OpType.Update:
                    tVHotelConfig = tVHotelConfigManager.GetEntity(id);
                    if (tVHotelConfig.ConfigCode.ToLower() == "storepaymentmodel")
                    {
                        var storePayConfig = JsonConvert.DeserializeObject<StorePayConfig>(tVHotelConfig.ConfigValue ?? "");
                        tVHotelConfig.StorePayConfig = new StorePayConfig()
                        {
                            PaymentModels = storePayConfig.PaymentModels,
                            SentRoom = storePayConfig.SentRoom
                        };


                        payPaymentModels = payPaymentModels.ToList().Select(m => new SelectListItem()
                        {
                            Text = m.Text,
                            Value = m.Value,
                            Selected = tVHotelConfig.StorePayConfig.PaymentModels.Contains(m.Value)
                        });
                    }
                    else if (tVHotelConfig.ConfigCode.ToLower() == "enbleumeng")
                    {
                        tVHotelConfig.IsEnableMeng =Convert.ToBoolean(tVHotelConfig.ConfigValue);
                    }
                    break;
                default:
                    break;
            }

            ViewBag.PayPaymentModels = payPaymentModels;

            return PartialView(tVHotelConfig);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult AddOrEdit(OpType optype, TVHotelConfig tVHotelConfig)
        {
            return base.ExecutionMethod(() =>
            {
                string errorMsg = string.Empty;

                tVHotelConfig.StorePayConfig.PaymentModels = tVHotelConfig.StorePayConfig.PaymentModels == null ? new List<string>() : tVHotelConfig.StorePayConfig.PaymentModels.Where(m => !m.ToLower().Equals("false")).ToList();

                if (tVHotelConfig.ConfigCode.ToLower() == "storepaymentmodel")
                {
                    if (tVHotelConfig.StorePayConfig == null || tVHotelConfig.StorePayConfig.PaymentModels.Count == 0)
                        errorMsg = "支付方式配置不能为空！";
                    else
                    {
                        tVHotelConfig.ConfigValue =
                            JsonConvert.SerializeObject(new StorePayConfig()
                            {
                                PaymentModels = tVHotelConfig.StorePayConfig.PaymentModels,
                                SentRoom = tVHotelConfig.StorePayConfig.SentRoom
                            });
                        tVHotelConfig.Active = true;
                    }
                }
                else if (tVHotelConfig.ConfigCode.ToLower() == "enbleumeng")
                {
                    tVHotelConfig.ConfigValue = tVHotelConfig.IsEnableMeng.ToString();
                }

                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    if (optype == OpType.Add)
                    {
                        var exittVHotelConfig =
                            tVHotelConfigManager.Search(new HotelConfigCriteria
                            {
                                HotelId = tVHotelConfig.HotelId,
                                ConfigCodes = tVHotelConfig.ConfigCode
                            });
                        if (exittVHotelConfig != null &&
                            exittVHotelConfig.Any(e => e.ConfigCode == tVHotelConfig.ConfigCode))
                        {
                            errorMsg = "该酒店配置已存在";

                        }
                        else
                        {
                            tVHotelConfig.CreateTime = DateTime.Now;
                            tVHotelConfig.LastUpdateTime = DateTime.Now;
                            tVHotelConfig.LastUpdater = CurrentUser.ChineseName;
                            tVHotelConfigManager.AddTVHotelConfig(tVHotelConfig);
                        }

                    }
                    else
                    {
                        tVHotelConfig.CreateTime = DateTime.Now;
                        tVHotelConfig.LastUpdateTime = DateTime.Now;
                        tVHotelConfig.LastUpdater = CurrentUser.ChineseName;
                        tVHotelConfigManager.UpdateTVHotelConfig(tVHotelConfig);
                    }
                }

                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }

        public string GetHotelNameById(string hotelId)
        {
            if (hotelManager.GetHotelObject(hotelId).Hotel != null)
            {
                return hotelManager.GetHotelObject(hotelId).Hotel.HotelName;
            }
            return "";
        }

        private List<SelectListItem> GetTvhotelconfigTypeList()
        {
            var tvhotelconfigTypeList = new List<SelectListItem>();

            XElement element = XElement.Load(this.Server.MapPath("~/Content/SiteXml/Constant.xml"));
            element.Element("tvhotelconfigs").Elements("option").ToList().
            ForEach(n => tvhotelconfigTypeList.Add(new SelectListItem() { Value = n.Attribute("name").Value, Text = n.Value }));
            ViewBag.tvhotelconfigTypeList = tvhotelconfigTypeList;

            return tvhotelconfigTypeList;
        }

        #region EditHotelPayment


        [HttpGet]
        [AjaxOnly]
        public ActionResult EditHotelPayment(int id, OpType type)
        {
            ViewBag.OpType = type;
            GetTvhotelconfigTypeList();

            var hotelPayment = new HotelPayment();
            switch (type)
            {
                case OpType.View:
                case OpType.Update:
                    var tVHotelConfig = tVHotelConfigManager.GetEntity(id);
                    hotelPayment = JsonConvert.DeserializeObject<HotelPayment>(tVHotelConfig.ConfigValue);
                    hotelPayment.Id = tVHotelConfig.Id;

                    ViewBag.PayPaymentModels = EnumExtensions.GetItems(typeof(PayPaymentModel)).Where(m => m.Value != (int)PayPaymentModel.QTPAY).Select(m => new SelectListItem()
                    {
                        Text = m.Description,
                        Value = m.Text,
                        Selected = hotelPayment.PaymentModels.Any(p => p.Trim().ToLower().Equals(m.Text.Trim().ToLower()))
                    });

                    break;
            }

            return PartialView(hotelPayment);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult EditHotelPayment(OpType optype, HotelPayment hotelPayment)
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
                    var tVHotelConfig = tVHotelConfigManager.GetEntity(hotelPayment.Id);
                    tVHotelConfig.CreateTime = DateTime.Now;
                    tVHotelConfig.LastUpdateTime = DateTime.Now;
                    tVHotelConfig.LastUpdater = CurrentUser.ChineseName;

                    tVHotelConfig.ConfigValue = JsonConvert.SerializeObject(hotelPayment);

                    tVHotelConfigManager.UpdateTVHotelConfig(tVHotelConfig);
                }

                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }

        #endregion
    }
}