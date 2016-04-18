using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentreApi.Controllers
{
    public class AppController : ApiController
    {
        private IAppLibraryManager _appLibrary;

        public AppController(IAppLibraryManager appLibrary)
        {
            
            _appLibrary = appLibrary;
        }

        public Dictionary<String, Apps> Get()
        {
            var apps = _appLibrary.GetAppVersionList();
            return apps;
        }
    }
}
