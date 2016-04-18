using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YeahCatchHotel.Models;

namespace YeahCatchHotel.Util
{
    public class FeachSevenDay:FeachHotelBase
    {
        public FeachSevenDay()
        {

        }

        public FeachSevenDay(string type):base(type)
        {
           
        }
        public override QitianDataModel HowGetDataSours()
        {
            QitianDataModel  o= JsonConvert.DeserializeObject<QitianDataModel>(base.JsonStr);
            return o;
        }

        public override void InitialUrl()
        {
            for (int i = 1; i <= 604; i++)
            {
                string url = "http://www.plateno.com/hotel/q/list?cityId=" + i.ToString() + "&fromDate=" + date + "&days=1&sortFlag=0&fetchRt=true&intlSearch=false&fromType=0&pageNumber=1&pageSize=300&checkCount=false&_=1433502762073";
                CheckUrl(url);
            }
        }
    }
}