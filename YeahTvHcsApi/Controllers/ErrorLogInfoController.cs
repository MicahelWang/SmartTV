using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;

using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Enum;
using YeahTVApiLibrary.Infrastructure;
using YeahTvHcsApi.ViewModels;
using YeahTVApiLibrary.Filter;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTvHcsApi.Controllers
{
    public class ErrorLogInfoController : ApiController
    {
        private ILogManager logManager;
        private IDeviceTraceLibraryManager deviceTraceLibraryManager;

        public ErrorLogInfoController(ILogManager logManager, IDeviceTraceLibraryManager  deviceTraceLibraryManager)
        {
            this.deviceTraceLibraryManager = deviceTraceLibraryManager;
            this.logManager = logManager;
        }

        [HttpPost]
        [ActionName("ErrorNotify")]
        [HCSCheckSignFilter(NeedCheckSign = false)]
        public void POST(PostParameters<PostErrorNotifyData> request)
        {
            var notifyData = request.Data;

            var hotelId = deviceTraceLibraryManager.Search(new DeviceTraceCriteria { DeviceSeries = request.Server_Id, DeviceType = DeviceType.HCSServer}).FirstOrDefault().HotelId;

            var behaviorLog = new BehaviorLog()
            {
                DeviceSerise = request.Server_Id,
                Id = Guid.NewGuid().ToString("N"),
                CreateTime = DateTime.Parse(notifyData.Time),
                BehaviorType = notifyData.Level.ParseAsEnum<BehaviorType>().ToString(),
                BehaviorInfo = notifyData.Exception,
                HotelId = hotelId
            };

            logManager.SaveBehavior(behaviorLog);
        }
    }
}