using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Filter
{
    public class ShopingMallFilterAttribute : ActionFilterAttribute
    {
        [Dependency]
        public IDeviceTraceLibraryManager DeviceTraceManager { get; set; }
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var context = filterContext.HttpContext;
        //    CheckBindDevice(filterContext.HttpContext, header);
        //}
        //public virtual void CheckBindDevice(HttpContextBase context, RequestHeader header)
        //{
        //    //判断该设备是否有绑定关系，如果没有绑定关系，则抛出该问题
        //    var trace = DeviceTraceManager.GetAppTrace(header);
        //    BackupDevice backupDevice = null;
        //    if (trace == null)
        //    {
        //        backupDevice = AppManager.GetAppBackupDevice(header);
        //    }
        //    if (trace == null && backupDevice == null)
        //    {
        //        throw new ApiException(ApiErrorType.Default, String.Format("该设备{0}尚未绑定，请绑定以后才可使用", header.DEVNO));
        //    }
        //    if (trace != null && !trace.Active)
        //    {
        //        throw new ApiException(ApiErrorType.Default, String.Format("该设备{0}已失效，无法继续使用", header.DEVNO));
        //    }
        //    if (trace != null)
        //    {
        //        header.HotelID = trace.HotelId;
        //        header.RoomNo = trace.RoomNo;
        //        context.Items[RequestParameter.TRACE] = trace;
        //    }
        //    else
        //    {
        //        header.HotelID = backupDevice.HotelId;
        //    }
        //}

    }
}
