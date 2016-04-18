using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class CacheController : BaseController
    {
        private readonly IRedisCacheService _cacheService;

        public CacheController(IRedisCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        // GET: Cache
        public ActionResult Index()
        {
            var dataSet = _cacheService.GetAllCache();
            return View(dataSet);
        }

        public ActionResult Edit(string key)
        {
            var value = new object();
            if (_cacheService.IsSet(key))
            {
                try
                {
                    value = _cacheService.Get(key);
                }
                catch
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    var setsValue = _cacheService.GetAllItemsFromSet<string>(key);
                    setsValue.ForEach(m => sb.Append(m));
                    value = sb.ToString();
                }
            }
            var result = new KeyValue {Key = key, Value = value};
            return PartialView(result);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Delete(string key)
        {
            var errorMsg = string.Empty;
            if (_cacheService.IsSet(key))
                _cacheService.Remove(key);
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult Clear()
        {
            var errorMsg = string.Empty;
            var keys=_cacheService.GetAllKeys();
            keys.Where(m => !m.ToLower().StartsWith("dashboard")).ToList().ForEach(m => _cacheService.Remove(m));
            //_cacheService.Clear();
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }


        [HttpPost]
        [AjaxOnly]
        public ActionResult BatchDelete(string id)
        {
            var errorMsg = string.Empty;
            var userIds = id.Split('+');
            foreach (var key in userIds)
            {
                bool existkey = _cacheService.IsSet(key);
                if (existkey == true)
                {
                    _cacheService.Remove(key);
                }
            }
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }
    }
}