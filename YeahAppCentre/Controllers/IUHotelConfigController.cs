using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class IUHotelConfigController : BaseController
    {
        private IHotelManager hotelManager;
        private ITVHotelConfigManager configManager;
        private ITvDocumentElementManager docElementManager;
        private ITvDocumentAttributeManager docAttributeManager;
        public IUHotelConfigController(IHotelManager hotelManager, ITVHotelConfigManager _configManager
            , ITvDocumentElementManager docElementManager
            , ITvDocumentAttributeManager docAttributeManager
        )
        {
            this.hotelManager = hotelManager;
            configManager = _configManager;
            this.docElementManager = docElementManager;
            this.docAttributeManager = docAttributeManager;
        }
        // GET: IUHotelConfig
        public ActionResult Index()
        {
            return View();
        }
        #region IUHotelConfig IU酒店 个性化配置
        #region 模板:首页背景图 首页背景音乐 图片轮播 开机视频
        [HttpGet]
        [AjaxOnly]
        public JsonResult IUIndexBgMuisc(string brandID)
        {
            var attr = GetDocAttribute(brandID, "HomeConfig");
            if (attr == null)
            {
                return Json(new HomeConfigModel() { home_background_musaudio = "" });
            }
            return Json(attr.Value);
        }
        string errorConfig = "对不起,没有找到该配置项!";
        [HttpPost]
        [AjaxOnly]
        public JsonResult EditIUIndexBgMuisc(string brandID, string ConfigValue)
        {
            if (string.IsNullOrEmpty(ConfigValue))
            {
                return Json("首页背景音乐不能为空!");
            }
            var attr = GetDocAttribute(brandID, "HomeConfig");
            if (attr == null)
            {
                return Json(errorConfig);
            }
            var js = new JavaScriptSerializer();
            var newConfig = js.Deserialize<HomeConfigModel>(attr.Value);
            if (newConfig.home_background_musaudio == null)
            {
                return Json(errorConfig);
            }
            newConfig.home_background_musaudio = ConfigValue;
            UpdateDocAttribute(brandID, "HomeConfig", js.Serialize(newConfig));
            return Json("操作成功！");
        }
        private TvDocumentAttribute GetDocAttribute(string brandID, string colunm)
        {
            var hotelModel = hotelManager.GetByBrand(brandID).FirstOrDefault();
            if (hotelModel == null)
            {
                return null;            
            }
            var documentElement = docElementManager.GetElementsByTemplateId(hotelModel.TemplateId)
                .Where(m => m.Name == "Document").FirstOrDefault();
            if (documentElement == null)
            {
                return null;
            }
            var docAttr = documentElement.Attributes.FirstOrDefault(m => m.Text == colunm);
            if (docAttr == null || string.IsNullOrEmpty(docAttr.Value))
            {
                return null;
            }
            return docAttr;
        }
        private TvDocumentAttribute UpdateDocAttribute(string brandID, string colunm, string value)
        {
            //colunm:Backgrand,HomeConfig,HomeVideoUrl,HomeConfig.home_background_musaudio
            var attr = GetDocAttribute(brandID, colunm);
            if (attr == null)
            {
                return null;
            }
            attr.Value = value;
            docAttributeManager.Update(attr);
            return attr;
        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult IUIndexBG(string brandID)
        {
            var attr = GetDocAttribute(brandID, "Backgrand");
            if (attr == null)
            {
                return Json("");
            }
            return Json(attr.Value);
        }
        [HttpPost]
        [AjaxOnly]
        public JsonResult IUIndexBG(string brandID, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Json("首页背景图片不能为空!");
            }
            TvDocumentAttribute attrbute = UpdateDocAttribute(brandID, "Backgrand", value);
            if (attrbute == null)
            {
                return Json(errorConfig);
            }
            return Json("操作成功！");
        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult IUPicCarousel(string brandID)
        {
            var attr = GetDocAttribute(brandID, "HomeConfig");
            if (attr == null)
            {
                return Json(new HomeConfigModel() { home_background_musaudio = "" });
            }
            return Json(attr.Value);
        }
        [HttpPost]
        [AjaxOnly]
        public JsonResult IUPicCarousel(string brandID, HomeConfigModel value)
        {
            if (value.home_pictures==null)
            {
                return Json("轮播图片不能为空!");
            }
            var attr = GetDocAttribute(brandID, "HomeConfig");
           
            if (attr == null)
            {
                return Json(errorConfig);
            }
            var js = new JavaScriptSerializer();
            var newConfig = js.Deserialize<HomeConfigModel>(attr.Value);
            if (newConfig.home_pictures == null)
            {
                return Json(errorConfig);
            }
            value.home_background_musaudio = newConfig.home_background_musaudio;
            UpdateDocAttribute(brandID, "HomeConfig", js.Serialize(value));
            return Json("操作成功！");
        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult IUStartVideo(string brandID)
        {
            var attr = GetDocAttribute(brandID, "HomeVideoUrl");
            if (attr == null)
            {
                return Json("");
            }
            return Json(attr.Value);
        }
        [HttpPost]
        [AjaxOnly]
        public JsonResult IUStartVideo(string brandID, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Json("开机视频不能为空!");
            }
            TvDocumentAttribute attrbute = UpdateDocAttribute(brandID, "HomeVideoUrl", value);
            if (attrbute == null)
            {
                return Json(errorConfig);
            }
            return Json("操作成功！");
        }
        #endregion

        #region 开机音乐  欢迎界面背景图  欢迎界面欢迎词 跳转首页
        [HttpGet]
        [AjaxOnly]
        public JsonResult IUStartMuisc(string BrandId)
        {
            TVHotelConfig tvHotelConfig = Initializtion(BrandId, "BackgroundMusicUrl");
            return Json(tvHotelConfig.ConfigValue);
        }
        [HttpPost]
        [AjaxOnly]
        public JsonResult EditIUStartMuisc(string BrandId, string ConfigValue)
        {
            if (string.IsNullOrEmpty(ConfigValue))
            {
                return Json("开机音乐不能为空!");
            }
            int count = UpdaeConfig(BrandId, ConfigValue, "BackgroundMusicUrl");
            if (count <= 0)
            {
                return Json(errorConfig);
            }
            return Json("操作成功！");
        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult IUWelcomeBg(string BrandId)
        {
            TVHotelConfig tvHotelConfig = Initializtion(BrandId, "BackgroundImageUrl");
            return Json(tvHotelConfig.ConfigValue);
        }
        [HttpPost]
        [AjaxOnly]
        public JsonResult EditIUWelcomeBg(string BrandId, string ConfigValue)
        {
            if (string.IsNullOrEmpty(ConfigValue))
            {
                return Json("欢迎背景图片不能为空!");
            }
            int count = UpdaeConfig(BrandId, ConfigValue, "BackgroundImageUrl");
            if (count <= 0)
            {
                return Json(errorConfig);
            }
            return Json("操作成功！");
        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult IUWelcomeWord(string BrandId)
        {
            HotelEntity hotelEntity = hotelManager.GetByBrand(BrandId).FirstOrDefault();
            if (hotelEntity == null)
            {
                return Json("");
            }
            return Json(hotelEntity.WelcomeWord);
        }
        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        public JsonResult EditIUWelcomeWord(string BrandId, string WelcomeWord)
        {
            List<string> hotelId = hotelManager.GetByBrand(BrandId).ToList().Select(m => m.HotelId).Distinct().ToList();
            int count = hotelManager.Update(h => hotelId.Contains(h.Id), h => new CoreSysHotelSencond { WelcomeWord = WelcomeWord });
            if (count <= 0)
            {
                return Json(errorConfig);
            }
            return Json("操作成功！");
        }
        [HttpGet]
        [AjaxOnly]
        public JsonResult IUAutoToHome(string BrandId)
        {
            HotelEntity hotelEntity = hotelManager.GetByBrand(BrandId).FirstOrDefault();
            if (hotelEntity == null)
            {
                return Json(false);
            }
            return Json(hotelEntity.AutoToHome);
        }
        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        public JsonResult EditIUAutoToHome(string BrandId, bool AutoToHome)
        {
            List<string> hotelId = hotelManager.GetByBrand(BrandId).ToList().Select(m => m.HotelId).Distinct().ToList();
            int count = hotelManager.Update(h => hotelId.Contains(h.Id), h => new CoreSysHotelSencond { AutoToHome = AutoToHome });
            if (count <= 0)
            {
                return Json(errorConfig);
            }
            return Json("操作成功！");
        }
        //页面初始化
        public TVHotelConfig Initializtion(string BrandId, string ConfigCode)
        {
            HotelEntity hotelEntity = hotelManager.GetByBrand(BrandId).FirstOrDefault();            
            TVHotelConfig tvHotelConfig = configManager.GetHotelConfig(new HotelConfigCriteria() { HotelId = hotelEntity.HotelId, ConfigCodes = ConfigCode });
            if (tvHotelConfig == null)
            {
                return new TVHotelConfig() { ConfigValue = "" };
            }
            return tvHotelConfig;
        }
        //修改配置()
        public int UpdaeConfig(string brandId, string configValue, string configCode)
        {
            List<string> hotelId = hotelManager.GetByBrand(brandId).ToList().Select(m => m.HotelId).Distinct().ToList();
            return configManager.Update(h => hotelId.Contains(h.HotelId) && h.ConfigCode == configCode, h => new TVHotelConfig { ConfigValue = configValue });
        }
        #endregion
        #endregion
    }

}