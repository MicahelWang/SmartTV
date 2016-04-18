using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApi.DomainModel.Models;
using YeahTvHcsApi.ViewModels;
using YeahTVApiLibrary.Filter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YeahTvHcsApi.Controllers
{
    public class GlobalConfigController : ApiController
    {
        private readonly ILogManager _logManager;
        private readonly IHCSGlobalConfigManager _hcsGlobalConfigManager;

        public GlobalConfigController(ILogManager logManager, IHCSGlobalConfigManager hcsGlobalConfig)
        {
            _logManager = logManager;
            _hcsGlobalConfigManager = hcsGlobalConfig;
        }

        [HttpPost]
        [ActionName("GlobalConfig")]
        [HCSCheckSignFilter(NeedCheckSign = true, GetPrivateKey = false)]
        public ResponseData<HCSGlobalConfig> GlobalConfig(PostParameters<PostGlobalConfigData> request)
        {
            HCSGlobalConfig config = _hcsGlobalConfigManager.GetServerGlobalConfig(request.Server_Id, request.Sign, request.Data.OldConfigNo);

            return new ResponseData<HCSGlobalConfig> { Sign = "", Data = config };
        }
    }
}
