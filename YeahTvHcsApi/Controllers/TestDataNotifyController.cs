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
    public class TestDataNotifyController : ApiController
    {
        private ILogManager _logManager;

        public TestDataNotifyController(ILogManager logManager)
        {
            _logManager = logManager;
        }

        [HttpPost]
        [ActionName("TestDataNotify")]
        [HCSCheckSignFilter(NeedCheckSign = false)]
        public void TestDataNotify(PostParameters<PostTestDataNotifyData> request)
        {
            request.Data.ServerId = request.Server_Id;

            _logManager.SaveInfo("HCS Test Data", JsonConvert.SerializeObject(request.Data), AppType.HCS, request.Server_Id);
        }
    }
}
