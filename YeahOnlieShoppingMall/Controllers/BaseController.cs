using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Web;
using System.Web.Mvc;

using YeahTVApi.Common;
using YeahTVApi.Entity;
using System.Globalization;
using YeahTVApiLibrary.Infrastructure;

namespace YeahOnlieShoppingMall.Controllers
{
    /// <summary>
    /// 基本控制器
    /// </summary>
    [ValidateInput(false)]
    public class BaseController : Controller
    {
         

        protected RequestHeader Header
        {
            get
            {
                RequestHeader header = new RequestHeader();
                try
                {
                    String UserAgent = HttpContext.Request.UserAgent.ToUpper();
                    String[] agent = UserAgent.Split(';');
                    //todo....add log
                    for (int i = 0; i < agent.Length; i++)
                    {
                        int index = agent[i].IndexOf(':');
                        string itemName = agent[i].Substring(0, index);
                        string itemValue = agent[i].Substring(index + 1);
                        if ("APP_ID".Equals(itemName))
                        {
                            header.APP_ID = itemValue;
                        }
                        else if ("DEV_NO".Equals(itemName))
                        {
                            header.DEVNO = itemValue;
                        }
                        else if ("APP_VERSION".Equals(itemName))
                        {
                            header.Ver = itemValue;
                        }
                        else if ("PLATFORM".Equals(itemName))
                        {
                            header.Platform = itemValue;
                        }
                        else if ("LANGUAGE".Equals(itemName))
                        {
                            header.Language = itemValue;
                        }

                    }
                }
                catch (Exception)
                {

                    // Response.Redirect("~/html/error.html",true);
                    //Response.End();

                }

                return header;
            }
        }


        public BaseRequestData RequestData
        {
            get
            {
                RequestHeader header = this.Header;
                BaseRequestData data = new BaseRequestData();
                data.devNo = header.DEVNO;
                data.APP_ID = header.APP_ID;

                return data;
            }
        }

        protected Guest MemberInfo
        {
            get
            {
                return Session[RequestParameter.Guest] as Guest;
            }

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CultureInfo culture = null;

            try
            {
                culture = new CultureInfo(Header.Language);
            }
            catch
            {
                culture = new CultureInfo("zh-CN");
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

            base.OnActionExecuting(filterContext);
        }
    }
}