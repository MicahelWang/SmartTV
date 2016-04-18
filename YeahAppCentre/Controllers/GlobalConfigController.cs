using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using YeahAppCentre.Web.Utility;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;

namespace YeahAppCentre.Controllers
{
    public class GlobalConfigController : BaseController
    {
        private IGlobalConfigManager globalConfigManager;
        public GlobalConfigController(IGlobalConfigManager globalConfigManager)
        {
            this.globalConfigManager = globalConfigManager;

        }
        // GET: GlobalConfig
        public ActionResult Index(GlobalConfigCriteria globalConfigCriteria)
        {
            if (globalConfigCriteria == null)
            {
                globalConfigCriteria = new GlobalConfigCriteria();
            }
            var partialViewResult = this.List(globalConfigCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            return View(new GlobalConfig() { TypeId = globalConfigCriteria.TypeId, PermitionType = globalConfigCriteria.PermitionType });
        }

        public ActionResult List(GlobalConfigCriteria globalConfigCriteria)
        {
            var globalConfigList = globalConfigManager.Search(globalConfigCriteria);

            return this.PartialView("List", globalConfigList);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult AddOrEdit(string id, OpType type, string typeId, string permitionType)
        {
            GetGlobalConfigDescribe();
            ViewBag.OpType = type;

            var globalConfig = new GlobalConfig();

            globalConfig.TypeId = typeId;
            globalConfig.PermitionType = permitionType;

            switch (type)
            {

                case OpType.Add:
                    break;
                case OpType.View:
                case OpType.Update:

                    globalConfig = globalConfigManager.GetGlobalConfig(new GlobalConfig { Id = id });
                    break;
                default:
                    break;
            }

            return PartialView(globalConfig);
        }
        [HttpPost]
        [AjaxOnly]
        public ActionResult AddOrEdit(GlobalConfig globalConfig, OpType type)
        {
            return base.ExecutionMethod(() =>
            {
                string errorMsg = string.Empty;


                XElement elment = XElement.Load(this.Server.MapPath("~/Content/SiteXml/Constant.xml"));

                var value = elment.Element("GlobalConfigs").Elements("option").ToList().FirstOrDefault(n => n.Value == globalConfig.ConfigName).Attribute("rule").Value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    bool configNameregex = Regex.IsMatch(globalConfig.ConfigValue, value);
                    if (configNameregex != true)
                    {
                        errorMsg = "请输入正确格式的配置值！";
                    }
                }

                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    if (type == OpType.Add)
                    {
                        var exitglobalconfig = globalConfigManager.GetGlobalConfig(new GlobalConfig
                        {
                            Id = null,
                            PermitionType = globalConfig.PermitionType,
                            TypeId = globalConfig.TypeId,
                            ConfigName = globalConfig.ConfigName
                        });
                        if (exitglobalconfig != null)
                        {
                            errorMsg = "该配置已存在";
                        }
                        else
                        {
                            globalConfig.Active = true;
                            globalConfig.LastUpdateTime = DateTime.Now;
                            globalConfig.PriorityLevel = 5;
                            globalConfig.LastUpdateUser = CurrentUser.ChineseName;
                            globalConfigManager.AddGlobalConfig(globalConfig);

                        }
                    }
                    else
                    {
                        globalConfig.LastUpdateTime = DateTime.Now;
                        globalConfig.LastUpdateUser = CurrentUser.ChineseName;
                        globalConfigManager.UpdateGlobalConfig(globalConfig);

                    }
                }

                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });
        }
        public string DeleteGlobalConfig(string id)
        {
            var message = "";
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    globalConfigManager.DeleteGlobalConfigById(id);
                    message = "Success";
                }
            }
            catch (Exception ex)
            {
                message = ex.ToString();

            }

            return message;
        }
        public List<SelectListItem> GetGlobalConfigDescribe()
        {

            var GlobalConfigDescribe = new List<SelectListItem>();
            XElement elment = XElement.Load(this.Server.MapPath("~/Content/SiteXml/Constant.xml"));
            elment.Element("GlobalConfigs").Elements("option").ToList().ForEach(n => GlobalConfigDescribe.Add(new SelectListItem() { Value = n.Value, Text = n.Attribute("name").Value }));
            ViewBag.GlobalConfigDescribe = GlobalConfigDescribe;
            return GlobalConfigDescribe;
        }

    }
}