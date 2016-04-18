namespace YeahAppCentre.Controllers
{
    using System;
    using System.Web.Mvc;
    using YeahAppCentre.Web.Utility;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahCenter.Infrastructure;
    using YeahTVApiLibrary.Infrastructure;
    using System.Linq;
    using System.Collections.Generic;
    using YeahTVApi.DomainModel;

    public class HotelTVChannelController : BaseController
    {
        private ITVChannelManager tVChannelManager;
        private IHotelTVChannelManager hotelTVChannelManager;
        private IHotelManager hotelManager;
        private ILogManager logmanager;
        private IAppLibraryManager appManager;
        private IHttpContextService httpContextService;

        public HotelTVChannelController(ITVChannelManager tVChannelManager,
            ILogManager logmanager,
            IAppLibraryManager appManager,
            IHotelTVChannelManager hotelTVChannelManager,
            IHotelManager hotelManager,
            IHttpContextService httpContextService)
            : base(logmanager, httpContextService)
        {
            this.tVChannelManager = tVChannelManager;
            this.logmanager = logmanager;
            this.appManager = appManager;
            this.httpContextService = httpContextService;
            this.hotelTVChannelManager = hotelTVChannelManager;
            this.hotelManager = hotelManager;
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
            return PartialView();

        }
        public ActionResult DefaultTvTemplate()
        {
            ViewBag.OpType = OpType.View;
            return PartialView();
        }
        // GET: TVChanel
        [HttpGet]
        [AjaxOnly]
        public JsonResult GetTVChanels(HotelTVChannelCriteria hotelTVChannelCriteria)
        {
            return base.ExecutionMethod(() =>
            {
                hotelTVChannelCriteria.NeedPaging = true;

                var channels = hotelTVChannelManager.SearchHotelTVChannels(hotelTVChannelCriteria);

                channels.ForEach(c =>
                {
                    c.HotelName = hotelManager.GetHotel(c.HotelId).HotelName;
                });

                var pageedChannels = new PagedViewList<HotelTVChannel>
                {
                    PageIndex = hotelTVChannelCriteria.Page,
                    PageSize = hotelTVChannelCriteria.PageSize,
                    Source = channels,
                    TotalCount = hotelTVChannelCriteria.TotalCount,
                    TotalPages = hotelTVChannelCriteria.TotalPages
                };

                return Json(pageedChannels, JsonRequestBehavior.AllowGet);
            });
        }
        //[HttpGet]
        //[AjaxOnly]
        //public JsonResult TVChanelList(HotelTVChannelCriteria hotelTVChannelCriteria)
        //{
        //    var list = new PagedViewList<string[]>();
        //    hotelTVChannelCriteria.NeedPaging = true;
        //    list.PageIndex = hotelTVChannelCriteria.Page;
        //    list.PageSize = hotelTVChannelCriteria.PageSize;
        //    var tempIds = hotelTVChannelManager.SearchOnlyHotelId(hotelTVChannelCriteria);
        //    list.TotalCount = hotelTVChannelCriteria.TotalCount;
        //    list.Source = new List<string[]>();
        //    tempIds.ForEach(id => list.Source.Add(new string[] { id, GetHotelNameById(id) }));


        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        [AjaxOnly]
        public JsonResult AddTVChannel(HotelTVChannel tVChannel)
        {
            return base.ExecutionMethod(() =>
            {
                string errMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(tVChannel.HotelId))
                {
                    errMessage = "请选择酒店！";
                }
                else if (string.IsNullOrWhiteSpace(tVChannel.ChannelId))
                {
                    errMessage = "请选择频道！";
                }
                else
                {
                    var isExist = hotelTVChannelManager.Search(new HotelTVChannelCriteria {HotelId = tVChannel.HotelId,ChannelId = tVChannel.ChannelId});

                if (isExist.Any())
                {
                        errMessage = "酒店频道配置已经存在！";
                }
                }

                if (string.IsNullOrWhiteSpace(errMessage))
                {
                    var channle = tVChannelManager.GetById(tVChannel.ChannelId);

                tVChannel.Category = channle.Category;
                tVChannel.CategoryEn = channle.CategoryEn;
                tVChannel.ChannelCode = channle.DefaultCode;
                tVChannel.Icon = channle.Icon;
                tVChannel.Name = channle.Name;
                tVChannel.NameEn = channle.NameEn;
                tVChannel.LastUpdateUser = CurrentUser.ChineseName;
                tVChannel.LastUpdateTime = DateTime.Now;
                tVChannel.HostAddress = Constant.DefaultHostAddress + tVChannel.HostAddress;

                hotelTVChannelManager.AddHotelTVChannel(tVChannel);
                }
                return Json(string.IsNullOrWhiteSpace(errMessage) ? "Success" : errMessage);
            });
        }


        [HttpPost]
        [AjaxOnly]
        public JsonResult UpdateTVChanel(HotelTVChannel tVChannel)
        {
            return base.ExecutionMethod(() =>
            {
                tVChannel.HostAddress = Constant.DefaultHostAddress + tVChannel.HostAddress;
                tVChannel.LastUpdateTime = DateTime.Now;
                tVChannel.LastUpdateUser = CurrentUser.ChineseName;
                hotelTVChannelManager.UpdateHotelTVChannel(tVChannel);

                return Json("Success");
            });
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult DeleteTVChanel(HotelTVChannel tVChannel)
        {
            return base.ExecutionMethod(() =>
            {
                hotelTVChannelManager.DeleteHotelTVChannel(tVChannel);

                return Json("Success");
            });
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult AddTVChanelsByTeamplate(string hotelId)
        {
            return base.ExecutionMethod(() =>
            {
                if (string.IsNullOrEmpty(hotelId))
                    return Json("酒店ID不能为空");

                hotelTVChannelManager.AddHotelTVChannel(hotelId, CurrentUser.ChineseName);

                return Json("Success");
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
    }
}