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

    public class MovieController : BaseController
    {
        private IMovieManager movieManager;
        private ILogManager logManager;

        public MovieController(
            IMovieManager movieManager,
            ILogManager logManager, IHttpContextService httpContextService)
            : base(logManager, httpContextService)
        {
            this.logManager = logManager;
            this.movieManager = movieManager;
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
        // GET: Movie
        [HttpGet]
        [AjaxOnly]
        public JsonResult GetMovies(MovieCriteria movieCriteria)
        {
            return base.ExecutionMethod(() =>
            {
                movieCriteria.NeedPaging = true;

                var movies = movieManager.SearchMovies(movieCriteria);

                var pageedMovies = new PagedViewList<Movie>
                {
                    PageIndex = movieCriteria.Page,
                    PageSize = movieCriteria.TotalCount,
                    Source = movies,
                    TotalCount = movieCriteria.TotalCount,
                    TotalPages = movieCriteria.TotalPages
                };

                return Json(pageedMovies, JsonRequestBehavior.AllowGet);
            });
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        public JsonResult AddMovie(Movie movie)
        {
            return base.ExecutionMethod(() =>
            {
                movie.LastUpdateTime = DateTime.Now;
                if (int.Parse(CheckMovieName(movie.Name)) > 0)
                    return Json("电影名称已存在");
                if(int.Parse(CheckMovieEnName(movie.NameEn))>0)
                    return Json("电影英文名称已存在");
                movieManager.AddMovies(movie);
                return Json("Success");
            });
        }

        public string CheckMovieName(string name)
        {
            return movieManager.GetAllFromCache().Count(m => m.Name.Equals(name.Trim())).ToString();
        }

        public string CheckMovieEnName(string name)
        {
            return movieManager.GetAllFromCache().Count(m => m.NameEn.Equals(name.Trim())).ToString();
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult DeleteMovie(Movie movie)
        {
            return base.ExecutionMethod(() =>
            {
                if (movieManager.Delete(movie))
                    return Json("Success");
                else
                    return Json("电影已经存在模板中不能删除！");
            }, false);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        public JsonResult UpdateMovie(Movie movie)
        {
            return base.ExecutionMethod(() =>
            {
                movie.LastUpdateTime = DateTime.Now;
                movieManager.Update(movie);

                return Json("Success");
            });
        }

    }
}