using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Filter;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTvHcsApi.ViewModels;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YeahTvHcsApi.Controllers
{
    public class SystemHCSController : ApiController
    {
        private readonly IDeviceTraceLibraryManager _DeviceTraceManager;

        public SystemHCSController(IDeviceTraceLibraryManager deviceTraceLibraryManager)
        {
            _DeviceTraceManager = deviceTraceLibraryManager;
        }

        [HttpPost]
        [HCSCheckSignFilter(GetPrivateKey = true, NeedCheckSign = false)]
        public ResponseHCSKey GetPrivateKey(RequestHCSServerKey request)
        {
            RequestHeader header = new RequestHeader();
            header.DEVNO = request.server_id;

            var privateKey = LogDeviceTrace(header);
            var returnValue = new ResponseHCSKey { key = PostParameters<string>.EncryptContent(request.publicKey, privateKey) };

            return returnValue;
        }

        private string LogDeviceTrace(RequestHeader header)
        {
            var returnValue = _DeviceTraceManager.GetDevicePrivateKey(header);

            return returnValue;
        }
    }
}
