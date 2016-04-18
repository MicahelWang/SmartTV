namespace YeahAppCentre.Controllers
{
    using System;
    using System.Web.Mvc;
    using YeahAppCentre.Web.Utility;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Linq;
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models.ViewModels;

    public class HotelMovieTraceController : BaseController
    {
        private IHotelMovieTraceManager hotelMovieTraceManager;
        private IMovieTemplateManager movieTemplateManager;
        private ILogManager logManager;
        private IHttpContextService httpContextService;

        public HotelMovieTraceController(
            IHotelMovieTraceManager hotelMovieTraceManager,
            ILogManager logManager,
            IHttpContextService httpContextService,
            IMovieTemplateManager movieTemplateManager)
            : base(logManager, httpContextService)
        {
            this.logManager = logManager;
            this.hotelMovieTraceManager = hotelMovieTraceManager;
            this.httpContextService = httpContextService;
            this.movieTemplateManager = movieTemplateManager;
        }

        // GET: Movie
        public ActionResult Index(HotelMovieTraceCriteria hotelMovieTraceCriteria)
        {
            hotelMovieTraceCriteria.NeedPaging = true;
            ViewBag.HotelMovieTraceCriteria = hotelMovieTraceCriteria;

            var partialViewResult = this.List(hotelMovieTraceCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            ViewBag.keyWord = "";
            ViewBag.HotelId = hotelMovieTraceCriteria.HotelId;
            ViewBag.MoiveTemplateName = hotelMovieTraceCriteria.MoiveTemplateName;

            return View();
        }

        [AjaxOnly]
        public ActionResult Edit(string id, string movietemplateId, OpType type)
        {
            ViewBag.OpType = type;
            ViewBag.MoiveTemplates = movieTemplateManager.GetAllFromCache().Where(s => s.MovieCount > 0)
                .Select(m => new SelectListItem
                {
                    Text = m.Title,
                    Value = m.Id,
                });

            switch (type)
            {
                case OpType.View:
                case OpType.Update:
                    var model = hotelMovieTraceManager.GetAllFromCache().FirstOrDefault(m => m.HotelId.Equals(id) && (string.IsNullOrEmpty(movietemplateId) || m.MoiveTemplateId.Equals(movietemplateId)));
                    return PartialView(model);
            }
            return PartialView();

        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Edit(OpType opType, HotelMovieTrace dto)
        {
            return base.ExecutionMethod(() =>
            {
                var errorMsg = string.Empty;
                string currentUser = CurrentUser.ChineseName;
                dto.Active = true;
                switch (opType)
                {
                    case OpType.Add:
                        errorMsg = AddHotelMovieTrace(dto);
                        break;
                    case OpType.Update:

                        hotelMovieTraceManager.UpdateMovieTraceManager(dto);
                        break;
                }

                return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
            });

        }

        [AjaxOnly]
        public ActionResult List(HotelMovieTraceCriteria hotelMovieTraceCriteria)
        {
            var list = new PagedViewList<HotelMovieTraceViewModel>();

            hotelMovieTraceCriteria.NeedPaging = true;
            list.PageIndex = hotelMovieTraceCriteria.Page;
            list.PageSize = hotelMovieTraceCriteria.PageSize;
            list.Source = hotelMovieTraceManager.SearchForHotelTemplate(hotelMovieTraceCriteria);
            list.TotalCount = hotelMovieTraceCriteria.TotalCount;

            return this.PartialView("List", list);
        }

        [AjaxOnly]
        public ActionResult MovieList(string hotelId)
        {
            var list = new List<HotelMovieTrace>();
            list = hotelMovieTraceManager.GetAllFromCache().Where(m => m.HotelId.Equals(hotelId)).ToList();
            return this.PartialView("MovieList", list);
        }

        [AjaxOnly]
        public ActionResult MovieUploadStatus(string id, OpType type, string hotelId)
        {
            var list = new HotelMovieTrace();
            ViewBag.OpType = type;
            list = hotelMovieTraceManager.GetAllFromCache().Where(m => m.HotelId.Equals(hotelId) && m.MovieId.Equals(id)).ToList().FirstOrDefault();
            return this.PartialView("MovieUploadStatus", list);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult EditMovieDownLoad(OpType opType, HotelMovieTrace dto)
        {

            return base.ExecutionMethod(() =>
            {
                bool result = hotelMovieTraceManager.UpdateMoviveTraceDownStatus(dto);
                return this.Content(result ? "Success" : "更新失败");
            });

        }

        public bool UpdateTemlate(string hotelId, string movietemplateId)
        {
            //模板最新集合

            //酒店现在模板集合

            return true;
        }

        private string AddHotelMovieTrace(HotelMovieTrace dto)
        {
            if (string.IsNullOrWhiteSpace(dto.HotelId)) return "请选择酒店！";
            var exitHotelMovieTrace = hotelMovieTraceManager.Search(new HotelMovieTraceCriteria { HotelId = dto.HotelId });

            if (exitHotelMovieTrace != null && exitHotelMovieTrace.Any())
            {
                return "酒店已配置！";
            }
            else
            {
                dto.LastViewTime = DateTime.Now;
                dto.IsDownload = false;
                dto.ViewCount = 0;
            }

            hotelMovieTraceManager.AddMovieTraceManager(dto);

            return string.Empty;
        }

        [AjaxOnly]
        public ActionResult MovieDownStatus(string hotelId, string isDown)
        {
            var list = new List<HotelMovieTrace>();
            list = hotelMovieTraceManager.GetAllFromCache().Where(m => m.HotelId.Equals(hotelId)).ToList();
            switch (isDown)
            {
                case "cancel":
                    {
                        hotelMovieTraceManager.UpdateMoviveTraceList(list, false);
                        break;
                    }

                case "down":
                    {
                        hotelMovieTraceManager.UpdateMoviveTraceList(list, true);
                        break;
                    }
                default:
                    break;
            }
            list = hotelMovieTraceManager.GetAllFromCache().Where(m => m.HotelId.Equals(hotelId)).ToList();
            return this.PartialView("MovieList", list);

        }


    }
}