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
using YeahTVApiLibrary.Filter;
using YeahTvHcsApi.ViewModels;

using Newtonsoft.Json;

namespace YeahTvHcsApi.Controllers
{
    public class PerformanceDataNotifyController : ApiController
    {
        private ILogManager _logManager;

        public PerformanceDataNotifyController(ILogManager logManager)
        {
            _logManager = logManager;
        }

        [HttpPost]
        [ActionName("PerformanceDataNotify")]
        [HCSCheckSignFilter(NeedCheckSign = false)]
        public void PerformanceDataNotify(PostParameters<PostPerformanceDataNotifyData> request)
        {
            request.Data.ServerId = request.Server_Id;

            _logManager.SaveInfo("HCS Performance Data", JsonConvert.SerializeObject(request.Data), AppType.HCS, request.Server_Id);
        }
    }
}
