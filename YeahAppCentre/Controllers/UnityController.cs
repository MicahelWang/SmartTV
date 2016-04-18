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
    using YeahTVApi.DomainModel;
    using YeahCenter.Infrastructure;

    public class UnityController : BaseController
    {
        private IDeviceTraceLibraryManager traceManager;
        private IAppLibraryManager appManager;
        private IHotelManager hotelManager;
        private IMovieManager movieManager;
        private ITVChannelManager tVChannelManager;
        private IRedisCacheManager redisCacheManager;
        private ISysAttachmentManager sysAttachmentManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IUserManager userManager;
        private ICacheManager cacheManager;
        private readonly IHotelPermitionManager hotelPermitionManager;
        private IMovieForLocalizeWrapperFacade movieForLocalizeFacade;

        public UnityController(IDeviceTraceLibraryManager traceManager,
            IAppLibraryManager appManager,
            ILogManager logManager,
            IHotelManager hotelManager,
            IMovieManager movieManager,
            ITVChannelManager tVChannelManager,
            IRedisCacheManager redisCacheManager,
            IHttpContextService httpContextService,
            ISysAttachmentManager sysAttachmentManager,
            IConstantSystemConfigManager constantSystemConfigManager,
            IUserManager userManager,
            ICacheManager cacheManager,
            IHotelPermitionManager hotelPermitionManager,
            IMovieForLocalizeManager movieForLocalizeManager,
            ILocalizeResourceManager localizeRsourceManager,
            IMovieForLocalizeWrapperFacade movieForLocalizeFacade)
            : base(logManager, httpContextService)
        {
            this.traceManager = traceManager;
            this.appManager = appManager;
            this.hotelManager = hotelManager;
            this.movieManager = movieManager;
            this.tVChannelManager = tVChannelManager;
            this.redisCacheManager = redisCacheManager;
            this.sysAttachmentManager = sysAttachmentManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.userManager = userManager;
            this.cacheManager = cacheManager;
            this.hotelPermitionManager = hotelPermitionManager;
            this.movieForLocalizeFacade = movieForLocalizeFacade;
        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetHotelNames(string query)
        {
            return base.ExecutionMethod(() =>
            {
                var hotelNames = hotelManager.GetAllHotels()
                  .Where(j => j.HotelName.Contains(query.Trim()) || j.HotelCode.Contains(query.Trim()))
                  .Select(h => new { HotelId = h.HotelId, HotelName = h.HotelName + "(" + h.HotelCode + ")", }).Distinct();

                return Json(hotelNames, JsonRequestBehavior.AllowGet);


            }, false);

        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetHotelNamesByPermition(string query)
        {
            return base.ExecutionMethod(() =>
            {
                var listTotal = hotelPermitionManager.GetHotelListByPermition(CurrentUser.UID);
                var hotelNames = listTotal.Where(j => j.HotelName.Contains(query.Trim()) || j.HotelCode.Contains(query.Trim()))
                  .Select(h => new { HotelId = h.Id, HotelName = h.HotelName + "(" + h.HotelCode + ")", }).Distinct();

                return Json(hotelNames, JsonRequestBehavior.AllowGet);


            }, false);

        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult GetUseNames(string query)
        {
            return base.ExecutionMethod(() =>
            {
                var usernames = userManager.SearchErpSysUser(new ErpSysUserCriteria())
                .Where(u => u.UserName.Contains(query.Trim()))
                .Select(h => new { UserId = h.Id, UserName = h.UserName }).Distinct();
                return Json(usernames, JsonRequestBehavior.AllowGet);
            }, false);
        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetMovieNames(string query)
        {
            return base.ExecutionMethod(() =>
            {
                var movieNames = movieManager.GetAllFromCache()
                    .Where(m => m.Name.Contains(query.Trim()) || m.NameEn.Contains(query.Trim()))
                    .Select(m => new { MovieId = m.Id, MovieName = m.Name + "(" + m.NameEn + ")" })
                    .Distinct();

                return Json(movieNames, JsonRequestBehavior.AllowGet);
            }, false);

        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult GetMovieNamesByLocalize(string query)
        {
            return base.ExecutionMethod(() =>
            {
                var sourceNames = movieForLocalizeFacade.SearchMovieForLocalizes(new MovieForLocalizeCriteria { }).SelectMany(m => m.Names).ToList();
                var moviesEn = sourceNames.Where(m => !string.IsNullOrEmpty(m.Content) && m.Content.Contains(query.Trim()) && m.Lang.ToLower() == "en-us").ToList();
                var en = new LocalizeResource();
                var movies = sourceNames.Where(m => !string.IsNullOrEmpty(m.Content) && m.Content.Contains(query.Trim()) && m.Lang.ToLower() == "zh-cn")
                    .Select(m => new { MovieId = m.Id, MovieName = m.Content + ((en = moviesEn.FirstOrDefault(o => o.Id == m.Id)) == null ? "" : "(" + en.Content + ")")})
                    .ToList();
                return Json(movies, JsonRequestBehavior.AllowGet);
            }, false);
        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult GetTVNames(string query)
        {
            return base.ExecutionMethod(() =>
            {
                var tvNames = tVChannelManager.SearchTVChannels(new TVChannelCriteria { })
                  .Where(t => t.Name.Contains(query.Trim()) || t.NameEn.Contains(query.Trim()))
                  .Select(t => new { ChannelId = t.Id, ChannelName = t.Name + "(" + t.NameEn + ")" }).Distinct();

                return Json(tvNames, JsonRequestBehavior.AllowGet);
            }, false);

        }

        public string GetAppName(string appId)
        {
            if (string.IsNullOrEmpty(appId))
            {
                return appId;
            }

            if (!redisCacheManager.IsSet(Constant.AppsListKey))
            {
                cacheManager.SetAppsList();
            }

            var appName = redisCacheManager.Get<Dictionary<String, Apps>>(Constant.AppsListKey)
                 .Where(r => r.Key.Equals(appId))
                 .FirstOrDefault().Value.Name;

            return string.IsNullOrEmpty(appName) ? appId : appName;

        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetAppNames(string query)
        {
            if (!redisCacheManager.IsSet(Constant.AppsListKey))
            {
                cacheManager.SetAppsList();
            }
            return base.ExecutionMethod(() =>
            {
                var appNames = redisCacheManager.Get<Dictionary<String, Apps>>(Constant.AppsListKey).Select(r => r.Value)
                  .Where(j => j.Name.Contains(query.Trim()))
                  .Select(h => new { AppId = h.Id, AppName = h.Name }).Distinct();
                return Json(appNames, JsonRequestBehavior.AllowGet);
            }, false);

        }

        [HttpGet]
        [AjaxOnly]
        public string GetAttachmentPath(int id)
        {
            var model = sysAttachmentManager.GetById(id);
            return model == null ? "" : constantSystemConfigManager.ResourceSiteAddress + model.FilePath;
        }
    }
}