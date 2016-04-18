using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YeahTVApi.Controllers
{
    public class CacheController : BaseController
    {
        private ICacheManager cacheManager;
        private ILogManager logManager;

        public CacheController(ICacheManager cacheManager, ILogManager logManager)
        {
            this.cacheManager = cacheManager;
            this.logManager = logManager;
        }
        
        public JsonResult SetCache()
        {
            try
            {
                cacheManager.SetWeather();
                cacheManager.SetAppsList();

                logManager.SaveInfo("设置缓存成功", "设置缓存成功", AppType.TV);
                return Json("设置缓存成功");
            }
            catch(Exception ex)
            {
                logManager.SaveError("设置缓存失败", ex, AppType.TV);
                return Json("设置缓存失败");
            }
        }
    }
}
