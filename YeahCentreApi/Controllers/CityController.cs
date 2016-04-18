using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Infrastructure;
using YeahCenter.Infrastructure;

namespace YeahCentreApi.Controllers
{
    public class CityController : ApiController
    {
        private readonly IHotelManager _hotelManager;
        private readonly ICityManager _cityManager;

        public CityController(IHotelManager hotelManager, ICityManager cityManager)
        {
            _hotelManager = hotelManager;
            _cityManager = cityManager;
        }

        // GET: api/City
        public IEnumerable<CoreSysCity> Get()
        {
            var reuslt = _cityManager.GetAll();
            return reuslt;
        }
        // GetUseCity: api/City
        [AcceptVerbs("GetUseCity")]
        public IEnumerable<CoreSysCity> GetUseCity()
        {
            var useCity = _hotelManager.GetAllUseCity();
            var reuslt = _cityManager.GetByIds(useCity);
            return reuslt;
        }

        // GET: api/City/5
        public CoreSysCity Get(int id)
        {
            return _cityManager.GetById(id);
        }

        
    }
}
