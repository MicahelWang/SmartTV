using System.Web;
using System.Web.Mvc;
using YeahAppCentre.Models;
using YeahAppCentre.Web.Utility;

namespace YeahAppCentre.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        
        public ActionResult Index(int? httpCode)
        {
            Response.TrySkipIisCustomErrors = true;
            if (Request.IsAjaxRequest())
            {
                Response.StatusCode = 500;
                var errorInfo = new ErrorInfo() { ErrorId = RouteData.Values["errorId"].ToString() };
                return this.JsonNet(errorInfo, JsonRequestBehavior.AllowGet);
            }

            var httpException = RouteData.Values["httpException"] as HttpException;
            httpCode = httpCode ?? ((httpException == null) ? 500 : httpException.GetHttpCode());

            switch (httpCode)
            {
                case 401:
                    Response.StatusCode = httpCode.Value;
                    return View("~/Views/Error/401.cshtml");
                case 403:
                    Response.StatusCode = httpCode.Value;
                    return View("~/Views/Error/403.cshtml");
                case 404:
                    Response.StatusCode = 404;
                    return View("~/Views/Error/404.cshtml");
                case 500:
                default:
                    ViewBag.ErrorId = RouteData.Values["errorId"];
                    Response.StatusCode = 500;
                    return View("~/Views/Error/500.cshtml");
            }
        }
       
    }
}