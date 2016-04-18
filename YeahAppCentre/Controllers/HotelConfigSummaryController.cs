using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahAppCentre.Controllers
{
    public class HotelConfigSummaryController : Controller
    {
        private IHotelMovieTraceManager hotelMovieTraceManager;
        private IHotelTVChannelManager hotelTVChannelManager;
        private ITvDocumentElementManager tvDocumentElementManager;
        private IHotelManager hotelManager;
        private IAppLibraryManager appLibraryManager;

        public HotelConfigSummaryController(
            IHotelMovieTraceManager hotelMovieTraceManager,
            IHotelTVChannelManager hotelTVChannelManager,
            ITvDocumentElementManager tvDocumentElementManager,
            IHotelManager hotelManager,
            IAppLibraryManager appLibraryManager)
        {
            this.hotelMovieTraceManager = hotelMovieTraceManager;
            this.hotelTVChannelManager = hotelTVChannelManager;
            this.tvDocumentElementManager = tvDocumentElementManager;
            this.hotelManager = hotelManager;
            this.appLibraryManager = appLibraryManager;
        }

        // GET: HotelConfigSummary
        public ActionResult Index(string hotelId = "")
        {
            ViewHotelConfig hotelConfig = new ViewHotelConfig();
            var configItemList = new List<ViewHotelConfigItem>();
            if (!string.IsNullOrWhiteSpace(hotelId))
            {
                var hotel = hotelManager.GetCoreSysHotelById(hotelId);
                if (hotel != null)
                {
                    ViewBag.HotelName = hotel.HotelName + "(" + hotel.HotelCode + ")";
                    hotelConfig.CoreSysHotel = hotel;
                    configItemList.Add(GetTemplateConfigInfo(hotel.TemplateId));
                    configItemList.Add(GetTvConfigInfo(hotelId));
                    configItemList.Add(GetVodConfigInfo(hotelId));
                    hotelConfig.ViewHotelApps = GetViewHotelApp(hotelId);
                }
            }
            hotelConfig.ViewHotelConfigItems = configItemList;
            return View(hotelConfig);
        }


        private ViewHotelConfigItem GetTvConfigInfo(string hotelId)
        {
            ViewHotelConfigItem config = new ViewHotelConfigItem();
            config.ConfigName = "TV数据";
            config.ExpectedItemsCount = 1;
            var hotelTvChannels = hotelTVChannelManager.SearchHotelTVChannels(new HotelTVChannelCriteria { HotelId = hotelId });
            config.ItemsCount = hotelTvChannels.Count;
            config.RightTips = hotelTvChannels.Select(m => m.Name).ToList();
            config.EditUrl = Url.Action("Index", "HotelTVChannel");

            return config;
        }
        private ViewHotelConfigItem GetVodConfigInfo(string hotelId)
        {
            ViewHotelConfigItem config = new ViewHotelConfigItem();
            config.ConfigName = "VOD数据";
            config.ExpectedItemsCount = 0;
            var hotelMovieList = hotelMovieTraceManager.Search(new HotelMovieTraceCriteria { HotelId = hotelId });
            config.ItemsCount = hotelMovieList.Count;
            config.RightTips = hotelMovieList.Select(m => m.Movie.Name).ToList();
            config.EditUrl = Url.Action("Index", "HotelMovieTrace");

            return config;
        }
        private ViewHotelConfigItem GetTemplateConfigInfo(string templateId)
        {
            ViewHotelConfigItem config = new ViewHotelConfigItem();
            config.ConfigName = "模板必填数据";
            var templateElements = tvDocumentElementManager.GetElementsByTemplateId(templateId);
            var requiredAttributes = templateElements.SelectMany(m => m.Attributes).Where(m => m.TemplateAttribute.Required);
            config.ExpectedItemsCount = templateElements.SelectMany(m => m.Attributes).Count(m => m.TemplateAttribute.Required);
            config.ItemsCount = requiredAttributes.Count();
            config.RightTips = requiredAttributes.Where(m => !string.IsNullOrWhiteSpace(m.Value)).Select(m => m.Text).ToList();
            config.WrongTips = requiredAttributes.Where(m => string.IsNullOrWhiteSpace(m.Value)).Select(m => m.Text).ToList();
            config.EditUrl = Url.Action("Elements", "Template", new { id = templateId });

            return config;
        }
        private ViewHotelApp GetViewHotelApp(string hotelId)
        {
            var apps = new ViewHotelApp();
            var launcherPackageName = "com.YeahInfo.Launcher";
            apps.Launcher = new List<AppPublish>();
            apps.ThirdPartyApps = new List<AppPublish>();

            var publishs = appLibraryManager.SearchAppPublishs(new AppPublishCriteria() { HotelId = hotelId, Active = true });

            var launcher = publishs.Where(m => m.AppVersion.App.PackageName.ToString().ToLower().Trim().Equals(launcherPackageName.ToLower())).OrderByDescending(
                        a => a.VersionCode).FirstOrDefault();

            if (launcher != null)
            {
                apps.Launcher.Add(launcher);
            }

            apps.ThirdPartyApps.AddRange(
                publishs.Where(
                    m => !m.AppVersion.App.PackageName.ToString().ToLower().Trim().Equals(launcherPackageName.ToLower())).GroupBy(m => m.Id).Select(m => m.OrderByDescending(
                        a => a.VersionCode).FirstOrDefault())
                    .ToList());

            return apps;
        }

    }
}