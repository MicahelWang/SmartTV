using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahCatchHotel.Util
{
    public  class FactoryClass
    {
        public static FeachHotelBase GetInstence(string type)
        {
            FeachHotelBase baseHotel=null;
            switch (type)
            {
                case "七天":
                    baseHotel = new FeachSevenDay(type);
                    break;
                case "速八":
                    baseHotel = new FeachSuba(type);
                    break;
                default:
                    break;
            }
            return baseHotel;
        }
    }
}