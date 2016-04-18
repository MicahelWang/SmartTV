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
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class HotelMovieController : Controller
    {
        private IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceWrapperFacade;

        public HotelMovieController(IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceWrapperFacade)
        {
            this.hotelMovieTraceWrapperFacade = hotelMovieTraceWrapperFacade;
        }

        // GET: HotelMovie
        public ActionResult Index(HotelMovieTraceNoTemplateCriteria hotelMovieTraceCriteria)
        {
            if (string.IsNullOrEmpty(hotelMovieTraceCriteria.HotelId))
                return View();

            var partialViewResult = this.List(hotelMovieTraceCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            ViewBag.hotelMovieTraceCriteria = hotelMovieTraceCriteria;

            return View();  
        }

        public ActionResult List(HotelMovieTraceNoTemplateCriteria hotelMovieTraceCriteria)
        {
            var list = new PagedViewList<HotelMovieTraceNoTemplate>();

            hotelMovieTraceCriteria.NeedPaging = true;
            list.PageIndex = hotelMovieTraceCriteria.Page;
            list.PageSize = hotelMovieTraceCriteria.PageSize;
            list.Source = hotelMovieTraceWrapperFacade.SearchHotelMovieTraceNoTemplates(hotelMovieTraceCriteria);
            list.TotalCount = hotelMovieTraceCriteria.TotalCount;

            return this.PartialView("List", list);
        }
    }
}