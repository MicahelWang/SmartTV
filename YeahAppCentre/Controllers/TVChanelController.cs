namespace YeahAppCentre.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Xml;
    using YeahAppCentre.Web.Utility;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;

    public class TVChanelController : BaseController
    {
        private ITVChannelManager tVChannelManager;
        private ILogManager logmanager;
        private IAppLibraryManager appManager;
        private IHttpContextService httpContextService;
        private ISysAttachmentManager sysAttachmentManager;
        private IConstantSystemConfigManager constantSystemConfigManager;

        public TVChanelController(ITVChannelManager tVChannelManager, 
            ILogManager logmanager, 
            IAppLibraryManager appManager,
            IHttpContextService httpContextService,
            ISysAttachmentManager sysAttachmentManager,
            IConstantSystemConfigManager constantSystemConfigManager)
            : base(logmanager, httpContextService)
        {
            this.tVChannelManager = tVChannelManager;
            this.logmanager = logmanager;
            this.appManager = appManager;
            this.httpContextService = httpContextService;
            this.sysAttachmentManager = sysAttachmentManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
        }


        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AjaxOnly]
        public ActionResult Edit(string id, OpType type)
        {
            ViewBag.OpType = type;
            ViewBag.SelectList=GetSelectList();
            return PartialView();

        }

        // GET: TVChanel
        [HttpGet]
        [AjaxOnly]
        public JsonResult GetTVChanels(TVChannelCriteria tVChannelCriteria)
        {
            return base.ExecutionMethod(() =>
            {
                tVChannelCriteria.NeedPaging = true;

                var channels = tVChannelManager.SearchTVChannels(tVChannelCriteria);
                channels.ForEach(t =>
                  {
                      var model = sysAttachmentManager.GetById(int.Parse(t.Icon));
                      t.IconPath = model == null ? "" : constantSystemConfigManager.ResourceSiteAddress + model.FilePath;
                  });

                var pageedChannels = new PagedViewList<TVChannel>
                {
                    PageIndex = tVChannelCriteria.Page,
                    PageSize = tVChannelCriteria.PageSize,
                    Source = channels,
                    TotalCount = tVChannelCriteria.TotalCount,
                    TotalPages = tVChannelCriteria.TotalPages
                };

                return Json(pageedChannels, JsonRequestBehavior.AllowGet);
            });
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult AddTVChannel(TVChannel tVChannel)
        {
            DealObj(tVChannel);
            return base.ExecutionMethod(() =>
            {
                tVChannel.LastUpdateTime = DateTime.Now;

                tVChannelManager.AddTVChannels(tVChannel);
                return Json("Success");
            });
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult DeleteTVChanel(TVChannel tVChannel)
        {
            return base.ExecutionMethod(() =>
              {
                  tVChannelManager.Delete(tVChannel);

                  return Json("Success");
              },false);
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult UpdateTVChanel(TVChannel tVChannel)
        {
            DealObj(tVChannel);
            return base.ExecutionMethod(() =>
            {
               tVChannel.LastUpdateTime = DateTime.Now;
               tVChannelManager.Update(tVChannel);

               return Json("Success");
            });
        }


        public  List<SelectListItem> GetSelectList()
        {
            List<SelectListItem> modelTypeList = new List<SelectListItem>();
            XMLPubFun fun = new XMLPubFun();
            XmlDocument doc=fun.GetXmlDoucument(Server.MapPath("~/Content/SiteXml/Constant.xml"));
            XmlNode categoryNodeList = doc.SelectSingleNode("/constant/Category");
            foreach (XmlNode node in categoryNodeList.ChildNodes)
            {
                string value = node.Attributes["value"].Value.Trim();
                string name = node.InnerText.Trim();
                modelTypeList.Add(new SelectListItem() {  Text=name, Value=value});
            }
            modelTypeList.Insert(0, new SelectListItem() { Value = "", Text = "==请选择==" });
            return modelTypeList;
        }

        public TVChannel DealObj(TVChannel tVChannel)
        {
            if(!string.IsNullOrEmpty(tVChannel.Category))
            { 
                string[] zhi = tVChannel.Category.Split('|');
                tVChannel.Category = zhi[1];
                tVChannel.CategoryEn = zhi[0];
            }
             return tVChannel;
        }

    }
}