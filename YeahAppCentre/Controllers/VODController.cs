using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class VODController : BaseController
    {
        private IMovieForLocalizeWrapperFacade movieFacade;
        private ITagManager tagManager;
        private IAppLibraryManager appManager;
        private IHotelManager hotelManager;
        private IAppLibraryManager appLibraryManager;
        private IHotelMovieTraceNoTemplateManager hotelMovieTraceNoTemplateManager;
        private IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceNoTemplateWrapperFacade;
        private ITVHotelConfigManager tvHotelConfigManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        public VODController(IMovieForLocalizeWrapperFacade movieFacade,
            ITagManager tagManager,
            IAppLibraryManager appManager,
            ILogManager logManager,
            IHotelManager hotelManager,
            IAppLibraryManager appLibraryManager,
            IHttpContextService httpContextService,
            IHotelMovieTraceNoTemplateManager hotelMovieTraceNoTemplateManager,
            IHotelMovieTraceNoTemplateWrapperFacade hotelMovieTraceNoTemplateWrapperFacade,
            ITVHotelConfigManager tvHotelConfigManager,
            IConstantSystemConfigManager constantSystemConfigManager)
            : base(logManager, httpContextService)
        {
            this.movieFacade = movieFacade;
            this.appManager = appManager;
            this.hotelManager = hotelManager;
            this.appLibraryManager = appLibraryManager;
            this.tagManager = tagManager;
            this.hotelMovieTraceNoTemplateManager = hotelMovieTraceNoTemplateManager;
            this.hotelMovieTraceNoTemplateWrapperFacade = hotelMovieTraceNoTemplateWrapperFacade;
            this.tvHotelConfigManager = tvHotelConfigManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
        }
        // GET: VOD
        public ActionResult Index(MovieForLocalizeCriteria movieCriteria, string MovieName)
        {
            movieCriteria.NeedPaging = true;
            var partialViewResult = this.List(movieCriteria) as PartialViewResult;
            if (partialViewResult != null)
                this.ViewBag.List = partialViewResult.Model;
            List<SelectListItem> listClass = new List<SelectListItem>();
            listClass.Add(new SelectListItem() { Text = "选择分类", Value = "" });
            tagManager.GetALLTagWithLocalizeResource().OrderBy(c => c.Id).ToList().ForEach(m =>
            {
                if (m.LocalizeResources.Count() > 0 && m.Id != 2)
                    listClass.AddRange(m.LocalizeResources.Where(l => l.Lang.ToLower().Equals("zh-cn")).Select(s => new SelectListItem() { Text = s.Content, Value = m.Id.ToString(), Selected = (m.Id.ToString() == movieCriteria.TagId) }));
            });
            List<SelectListItem> listState = new List<SelectListItem>();
            listState.Add(new SelectListItem() { Text = "选择分发状态", Value = "-1", Selected = (-1 == movieCriteria.HotelCount) });
            listState.Add(new SelectListItem() { Text = "已分发", Value = "1", Selected = (1 == movieCriteria.HotelCount) });
            listState.Add(new SelectListItem() { Text = "未分发", Value = "0", Selected = (0 == movieCriteria.HotelCount) });

            ViewBag.classList = listClass;
            ViewBag.listState = listState;
            ViewBag.movieCriteria = movieCriteria;
            ViewBag.MovieName = MovieName ?? "";
            return View();
        }
        [AjaxOnly]
        public ActionResult List(MovieForLocalizeCriteria movieCriteria)
        {
            var list = new PagedViewList<MovieForLocalize>();
            if (movieCriteria.SortFiled.Equals("Id"))
            {
                movieCriteria.SortFiled = "LastUpdateTime";
                movieCriteria.OrderAsc = false;
            }
            movieCriteria.NeedPaging = true;
            list.PageIndex = movieCriteria.Page;
            list.PageSize = movieCriteria.PageSize;
            list.Source = movieFacade.SearchMovieForLocalizesByController(movieCriteria);
            list.TotalCount = movieCriteria.TotalCount;

            return this.PartialView("List", list);
        }
        #region 电影：add,delete
        [HttpGet]
        public ActionResult Add()
        {
            GetTags();
            return View();
        }

        private void GetTags()
        {
            var tagList = tagManager.GetALLTagWithLocalizeResource().OrderBy(c => c.Id).ToList();
            var checkList = new List<SelectListItem>();
            tagList.ForEach(m =>
            {
                if (m.Id != 2)
                    m.LocalizeResources.Where(n => n.Lang.ToLower() == "zh-cn").ToList().ForEach(n =>
                    {
                        checkList.Add(new SelectListItem() { Text = n.Content, Value = m.Id.ToString() });
                    }
                    );
            });
            ViewBag.checkList = checkList;
        }
        [AjaxOnly]
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(MovieForLocalize model, string HotelIds, int distribution)
        {
            string errorMsg = "";
            string tipMsg = "";
            model.Names = CovertToObj(model.Name);
            model.Name = model.Names.First().Id;
            model.Languages = CovertToObj(model.Language);
            model.Language = model.Languages.First().Id;
            model.Directors = CovertToObj(model.Director);
            model.Director = model.Directors.First().Id;
            model.MovieReviews = CovertToObj(model.MovieReview);
            model.MovieReview = model.MovieReviews.First().Id;
            model.Districts = CovertToObj(model.District);
            model.District = model.Districts.First().Id;
            model.Starreds = CovertToObj(model.Starred);
            model.Starred = model.Starreds.First().Id;
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;
            model.LastUpdateUser = CurrentUser.ChineseName;
            model.DistributeAll = (distribution == 2);

            if (model.IsTop == true && !CheckIsReachMax(model))
            {
                tipMsg = "电影添加成功,置顶失败,原因:超过系统最大电影置顶数! ";
                model.IsTop = false;
            }
            model.CurrencyType = "RMB";

            List<string> hotelIds = null;
            DistributeType distributeType = DistributeType.All;
            if (distribution == 1)
            {
                if (string.IsNullOrWhiteSpace(HotelIds) ||
                (hotelIds = HotelIds.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList())
                    .Count == 0)
                    errorMsg = "请选择要分发的酒店";
                distributeType = DistributeType.Part;
            }
            if (string.IsNullOrWhiteSpace(errorMsg))
            {
                movieFacade.AddMovie(model);
                try
                {
                    if (distribution > 0)
                    {
                        hotelMovieTraceNoTemplateWrapperFacade.Distribute(distributeType, model,
                                      hotelManager.GetAllCoreSysHotels(), CurrentUser.ChineseName, hotelIds);
                        if (model.IsTop == true)
                        {
                            SetHotelMoviesTop(model);
                        }
                    }
                }
                catch (Exception err)
                {
                    if (!string.IsNullOrEmpty(tipMsg))
                    {
                        tipMsg += " 分发失败!";
                    }
                    else
                        tipMsg += " 电影添加成功, 分发失败!";
                }
            }

            GetTags();
            string retMsg = "{{\"Success\":\"{0}\",\"Msg\":\"{1}\"}}";
            return Json(string.Format(retMsg, !string.IsNullOrEmpty(errorMsg) ? "true" : "false", !string.IsNullOrEmpty(errorMsg) ? "操作成功!" : (tipMsg == "" ? "操作成功!" : tipMsg)));

        }
        private List<LocalizeResource> CovertToObj(string json)
        {
            if (json == "")
            {
                return null;
            }
            var id = Guid.NewGuid().ToString("N");
            var list = JsonConvert.DeserializeObject<List<LocalizeResource>>(json);
            list.ForEach(o => { o.Id = id; });
            return list;
        }


        [HttpPost]
        [AjaxOnly]
        public JsonResult DeleteMovie(MovieForLocalize movie)
        {
            return base.ExecutionMethod(() =>
            {
                //movieFacade.DeleteByController(movie);
                return Json("Success");
            }, false);
        }
        #endregion

        #region 电影：update
        public ActionResult Edit(string id)
        {
            GetTags();
            var model = movieFacade.FindByKey(id);
            return View(model);
        }
        [AjaxOnly]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(MovieForLocalize model)
        {
            string errorMsg = "";
            var oldModel = movieFacade.FindByKey(model.Id);

            model.Names = CovertToObj(model.Name);
            model.Name = model.Names.First().Id;
            model.Languages = CovertToObj(model.Language);
            model.Language = model.Languages.First().Id;
            model.Directors = CovertToObj(model.Director);
            model.Director = model.Directors.First().Id;
            model.MovieReviews = CovertToObj(model.MovieReview);
            model.MovieReview = model.MovieReviews.First().Id;
            model.Districts = CovertToObj(model.District);
            model.District = model.Districts.First().Id;
            model.Starreds = CovertToObj(model.Starred);
            model.Starred = model.Starreds.First().Id;
            model.LastUpdateTime = DateTime.Now;
            model.LastUpdateUser = CurrentUser.ChineseName;
            model.CreateTime = oldModel.CreateTime;

            bool isSetFlag = false;
            if (model.IsTop != oldModel.IsTop)
            {
                isSetFlag = true;
                if (model.IsTop == true && !CheckIsReachMax(model))
                {
                    errorMsg = "修改成功,置顶失败,原因:超过系统最大电影置顶数";
                    isSetFlag = false;
                    model.IsTop = false;
                }
            }

            model.CurrencyType = "RMB";
            movieFacade.UpdateWithLocalize(model);
            if (oldModel.DefaultAmount != model.DefaultAmount)
            {
                hotelMovieTraceNoTemplateWrapperFacade.Update(h => h.MovieId == model.Id, h => new HotelMovieTraceNoTemplate
                {
                    Price = model.DefaultAmount,
                    LastUpdateUser = CurrentUser.ChineseName
                });
            }

            if (isSetFlag)
                SetHotelMoviesTop(model);

            GetTags();
            return Content(string.IsNullOrEmpty(errorMsg) ? "操作成功!" : errorMsg);
        }
        #endregion

        #region 电影 置顶

        [HttpPost]
        public JsonResult SetTop(MovieForLocalize movie)
        {
            if (movie.IsTop == true)
            {
                if (!CheckIsReachMax(movie))
                {
                    return Json("操作失败,超过系统最大电影置顶数");
                }
            }
            var model = movieFacade.FindByKey(movie.Id);
            model.IsTop = movie.IsTop;
            model.LastUpdateTime = DateTime.Now;
            return base.ExecutionMethod(() =>
            {
                SetHotelMoviesTop(model);
                movieFacade.Update(model);
                return Json("Success");
            }, false);
        }

        private bool CheckIsReachMax(MovieForLocalize movie)
        {
            var tops = movieFacade.Search(new MovieForLocalizeCriteria { IsTop = true }).Count();
            var hotelPayment = JsonConvert.DeserializeObject<GlobalPayment>(constantSystemConfigManager.HotelPayment);
            int maxTops = hotelPayment.TopMoviesCount;
            return tops < maxTops ? true : false;
        }
        private string SetHotelMoviesTop(MovieForLocalize movie)
        {
            hotelMovieTraceNoTemplateWrapperFacade.Update(h => h.MovieId == movie.Id, h => new HotelMovieTraceNoTemplate
            {
                IsTop = movie.IsTop,
                LastUpdateUser = CurrentUser.ChineseName
            });
            return "";
        }
        #endregion

        #region 电影分发

        [HttpGet]
        public ActionResult Distribute(string movieId, int pageIndex)
        {
            var hotelMovies = hotelMovieTraceNoTemplateManager.SearchHotelMovieTraceNoTemplates(new HotelMovieTraceNoTemplateCriteria { MovieId = movieId })                
                .Where(m => !m.IsDelete).ToList();
            var distribute = new DistributeModel()
            {
                HotelIds = string.Join("|", hotelMovies.Select(m => m.HotelId).ToList())
            };

            var hotels = hotelManager.GetAllCoreSysHotels().Where(m => distribute.HotelIds.Contains(m.Id)).ToList();

            distribute.HotelListJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(hotels.Select(m => new { m.Id, Name = m.HotelName }).ToList());

            distribute.HotelNames = string.Join("、", hotels.Select(m => m.HotelName).ToList());
            distribute.PageIndex = pageIndex;

            return View(distribute);
        }

        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> Distribute(DistributeModel distributeModel)
        {
            var chineseName = CurrentUser.ChineseName;
            var distributeResult = await Task.Run(() =>
            {
                var errorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(distributeModel.MovieId))
                    errorMessage = "电影ID不能为空！";

                var movie = movieFacade.FindByKey(distributeModel.MovieId);

                if (movie == null)
                    errorMessage = "未找到电影相关信息！";

                List<string> hotelIds = null;
                if (distributeModel.DistributeType == DistributeType.Part)
                {
                    if (string.IsNullOrWhiteSpace(distributeModel.HotelIds) ||
                (hotelIds = distributeModel.HotelIds.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList())
                    .Count == 0)
                        errorMessage = "请选择要分发的酒店！";
                }

                if (string.IsNullOrWhiteSpace(errorMessage))
                {
                    hotelMovieTraceNoTemplateWrapperFacade.Distribute(distributeModel.DistributeType, movie,
                        hotelManager.GetAllCoreSysHotels(), chineseName, hotelIds);
                }

                return (string.IsNullOrWhiteSpace(errorMessage) ? "Success" : errorMessage);
            });
            return Json(distributeResult);
        }


        #endregion
    }
}