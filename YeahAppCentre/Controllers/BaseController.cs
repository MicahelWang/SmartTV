using System;
using System.Linq;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILogManager _logmanager;
        private readonly IHttpContextService httpContextService;
        private CurrentUser _currentUser;
        
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        protected CurrentUser CurrentUser
        {
            //get { return _currentUser ?? (_currentUser = DependencyResolver.Current.GetService<CurrentUser>()); }
            get { return _currentUser ??(_currentUser = WebHelper.GetCurrentUser(httpContextService.Current)); }
        }

        public BaseController(ILogManager logmanager, IHttpContextService httpContextService)
        {
            this._logmanager = logmanager;
            this.httpContextService = httpContextService;
        }

        public BaseController()
        {
            this._logmanager = DependencyResolver.Current.GetService<ILogManager>();
            this.httpContextService = DependencyResolver.Current.GetService<IHttpContextService>();
        }

        // GET: Base
        public JsonResult ExecutionMethod(Func<JsonResult> fun, bool ShouldValidModel = true)
        {
            try
            {
                if (ShouldValidModel && !ModelState.IsValid)
                {
                    return Json("请填入必填值!", JsonRequestBehavior.AllowGet);
                }

                return fun();
            }
            catch(Exception ex)
            {
                _logmanager.SaveError(ex, "ExecutionMethodError", AppType.AppCenter);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExecutionMethod(Func<ActionResult> fun, bool ShouldValidModel = true)
        {
            try
            {
                if (ShouldValidModel && !ModelState.IsValid)
                {
                    var errMsg = ModelState.Values.SelectMany(modelState => modelState.Errors).Aggregate(string.Empty, (current, modelError) => current + (modelError.ErrorMessage));
                    return  this.Content(string.IsNullOrWhiteSpace(errMsg)?"请填入必填值!":errMsg);
                }

                return fun();
            }
            catch (Exception ex)
            {
                _logmanager.SaveError(ex, "ExecutionMethodError", AppType.AppCenter);
                return this.Content("保存失败!");
            }
        }
    }
}