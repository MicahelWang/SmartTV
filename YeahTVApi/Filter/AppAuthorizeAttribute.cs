namespace HZTVApi.Filter
{
    using HZTVApi.Infrastructure;
    using HZTVApi.Entity;
    using Microsoft.Practices.Unity;
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class AppAuthorizeAttribute : FilterAttribute
    {
        [Dependency]
        public ITVTraceManager traceMagager { get; set; }

        public AppAuthorizeAttribute()
        {
        }

        public String Authorized(RequestHeader data, HttpContextBase context)
        {
            return traceMagager.GetHotelRoomKey(data);
        }
    }
}