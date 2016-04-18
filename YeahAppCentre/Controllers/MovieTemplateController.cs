namespace YeahAppCentre.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using YeahAppCentre.Web.Utility;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Linq;

    public class MovieTemplateController : BaseController
    {
        private IMovieTemplateManager movieTemplateManager;
        private ILogManager logManager;
        private IMovieTemplateRelationWrapperFacade movieTemplateRelationWrapperFacade;

        public MovieTemplateController(
            IMovieTemplateManager movieTemplateManager,
            IMovieTemplateRelationWrapperFacade movieTemplateRelationWrapperFacade,
            ILogManager logManager,
            IHttpContextService httpContextService)
            : base(logManager, httpContextService)
        {
            this.logManager = logManager;
            this.movieTemplateManager = movieTemplateManager;
            this.movieTemplateRelationWrapperFacade = movieTemplateRelationWrapperFacade;
        }
        public ActionResult Index()
        {
            return View();
        }

        [AjaxOnly]
        public ActionResult GetMovieById(string movieId,string templateId)
        {
            var listMovieTemplateRelations = movieTemplateRelationWrapperFacade.SearchMovieTemplateRelations(new MovieTemplateRelationCriteria() { MovieTemplateId = templateId, MovieId = movieId });
           return Json(listMovieTemplateRelations);
        }

        public ActionResult ConfigMovie(string id,OpType optype,string templateId,string name)
        {
            ViewBag.Optype = optype;
            if (optype == OpType.Add)
            {
                ViewBag.MovieId = "";
                ViewBag.TemplateId = id;
                ViewBag.MovieName = "";
            }
            if (optype == OpType.Update)
            {
                ViewBag.MovieId = id;
                ViewBag.TemplateId = templateId;
                ViewBag.MovieName = name;
            }
            return PartialView();
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
        public JsonResult GetTemplates(MovieCriteria movieTemplateCriteria)
        {
            return base.ExecutionMethod(() =>
            {
                movieTemplateCriteria.NeedPaging = true;

                var movieTemplates = movieTemplateManager.SearchMovieTemplates(movieTemplateCriteria);

                var pageedMovies = new PagedViewList<MovieTemplate>
                {
                    PageIndex = movieTemplateCriteria.Page,
                    PageSize = movieTemplateCriteria.TotalCount,
                    Source = movieTemplates,
                    TotalCount = movieTemplateCriteria.TotalCount,
                    TotalPages = movieTemplateCriteria.TotalPages
                };

                return Json(pageedMovies, JsonRequestBehavior.AllowGet);
            });
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult AddTemplateRelation(MovieTemplateRelation movieTemplateRelation, OpType optype)
        {
            return base.ExecutionMethod(() =>
            {
                movieTemplateRelation.LastUpdateTime = DateTime.Now;
                movieTemplateRelation.LastUpdateUser = CurrentUser.ChineseName;

                List<MovieTemplateRelation> movie = movieTemplateRelationWrapperFacade.SearchMovieTemplateRelations(new MovieTemplateRelationCriteria()
                {
                    MovieTemplateId = movieTemplateRelation.MovieTemplateId,
                    MovieId = movieTemplateRelation.MovieId
                });

                switch (optype)
                {
                    case OpType.Add:
                        if (movieTemplateRelation.Price == null)
                        {
                            movieTemplateRelation.Price = 0;
                        }
                        if (movie.Count > 0)
                        {
                            return Json("电影已被该模板绑定");
                        }
                        movieTemplateRelationWrapperFacade.AddMovieTemplateRelation(movieTemplateRelation);
                        break;
                    case OpType.Update:
                        //修改价格
                        var obj=movie.FirstOrDefault();
                        if(obj!=null)
                        {
                            obj.Price = movieTemplateRelation.Price;
                            movieTemplateRelationWrapperFacade.UpdateMovieTemplateRelation(obj);
                        }
                        break;
                    default:
                        break;
                }
                return Json("Success");
            });
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult DeleteTemplateRelation(MovieTemplateRelation movieTemplateRelation)
        {
            return base.ExecutionMethod(() =>
            {
                movieTemplateRelationWrapperFacade.DeleteMovieTemplateRelation(movieTemplateRelation);
                return Json("Success");
            }, false);
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult UpdateTemplateRelation(MovieTemplateRelation movieTemplateRelation)
        {
            return base.ExecutionMethod(() =>
            {
                movieTemplateRelation.LastUpdateTime = DateTime.Now;
                movieTemplateRelation.LastUpdateUser = CurrentUser.ChineseName;
                movieTemplateRelationWrapperFacade.UpdateMovieTemplateRelation(movieTemplateRelation);
                return Json("Success");
            });
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult AddTemplate(MovieTemplate movieTemplate)
        {
            return base.ExecutionMethod(() =>
            {
                var exitmovieTemplate = movieTemplateManager.SearchMovieTemplates(new MovieCriteria { TemplateTitle = movieTemplate.Title });
                if (exitmovieTemplate.Count != 0)
                {
                    return Json("该模版已存在！");
                }
                else
                {
                    movieTemplate.LastUpdateUser = CurrentUser.ChineseName;
                    movieTemplate.LastUpdateTime = DateTime.Now;
                    movieTemplate.MovieCount = 0;
                    movieTemplate.HotelCount = 0;

                    movieTemplateManager.AddMovieTemplates(movieTemplate);
                    return Json("Success");
                }


            });
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult DeleteTemplate(MovieTemplate movieTemplate)
        {
            return base.ExecutionMethod(() =>
            {
                if (movieTemplateManager.Delete(movieTemplate))
                    return Json("Success");
                else
                    return Json("模板已经被使用不能删除！");
            }, false);
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult UpdateTemplate(MovieTemplate movieTemplate)
        {
            return base.ExecutionMethod(() =>
            {
                movieTemplate.LastUpdateTime = DateTime.Now;
                movieTemplateManager.Update(movieTemplate);

                return Json("Success");
            });
        }

    }
}