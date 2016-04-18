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
    public class BrandController : ApiController
    {
        private readonly IBrandManager _brandManager;

        public BrandController(IBrandManager brandManager)
        {
            _brandManager = brandManager;
        }

        public List<CoreSysBrand> Get()
        {
            return _brandManager.GetAll();
        }

        public CoreSysBrand Get(string id)
        {
            return _brandManager.GetBrand(id);
        }

        [AcceptVerbs("ByGroup")]
        public List<CoreSysBrand> GetByGroup(string id)
        {
            return _brandManager.GetBrandsByGroup(id);
        }
    }
}
