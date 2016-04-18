using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Entity;
using YeahTVApi.Infrastructure;
using YeahTVApi.ServiceProvider.PMS;
using YeahTVApi.Common;
using HZ.Web.Authorization;

namespace YeahTVApi.ServiceProvider
{
    public class HotelCommonService : CentralGetwayServiceBase, IHotelCommonService
    {
        public HotelCommonService()
            : base(null)
        {
        }
         
        public string GetWeather(string cityName)
        {
            string result = "";
            try
            {
                String action = APICallFactory.CallAction(APICallFactory.APICallType.GetWeather);
                List<KeyValuePair<String, String>> pams = new List<KeyValuePair<String, String>>();
                pams.Add(new KeyValuePair<string, string>("cityName", cityName));
                var response = CentralApi.GetResponse(action, pams, null, null);
                result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
    }
}
