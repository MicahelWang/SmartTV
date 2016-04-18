namespace HZTVApi.Manager
{
    using HZTVApi.Infrastructure;
    using HZTVApi.Entity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using HZTVApi.Entity.CentralMapping;

    public static class HotelHelp
    {
        public static String GetHotelStyleName(this HotelStyle style)
        {
            if (style == HotelStyle.HANTING)
                return "汉庭";
            else if (style == HotelStyle.KEZHAN)
                return "海友";
            else if (style == HotelStyle.MANXIN)
                return "漫心";
            else if (style == HotelStyle.QUANJI)
                return "全季";
            else if (style == HotelStyle.XINGCHENG)
                return "星程";
            else if (style == HotelStyle.XIYUE)
                return "禧玥";
            return null;
        }

        public static BaseRequestData ToBaseRequestData(this RequestHeader header)
        {
            BaseRequestData data = new BaseRequestData();
            data.APP_ID = header.APP_ID;
            data.Brand = header.Brand;
            data.devNo = header.DEVNO;
            data.language = header.Language;
            data.Manufacturer = header.Manufacturer;
            data.Model = header.Model;
            data.OSVersion = header.OSVersion;
            data.Platform = header.Platform;
            data.ver = header.Ver;
            return data;
        }

    }
}
