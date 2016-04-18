using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahCenter.Infrastructure;

namespace YeahCentreApi.Controllers
{
    public class TemplateController : ApiController
    {
        private readonly ITvTemplateManager _templateManager;

        public TemplateController(ITvTemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        //HotelId
        public Object Get(string id, string templateRootName)
        {
            return _templateManager.GetById(id,templateRootName);
        }
    }
}
