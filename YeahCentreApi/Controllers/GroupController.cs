using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YeahTVApi.DomainModel.Models;
using YeahCenter.Infrastructure;

namespace YeahCentreApi.Controllers
{
    public class GroupController : ApiController
    {
        private readonly IGroupManager _groupManager;

        public GroupController(IGroupManager groupManager)
        {
            _groupManager = groupManager;
        }

        public IEnumerable<CoreSysGroup> Get()
        {
            return _groupManager.GetAll();
        }

        public CoreSysGroup Get(string id)
        {
            return _groupManager.GetGroup(id);
        }
    }
}
