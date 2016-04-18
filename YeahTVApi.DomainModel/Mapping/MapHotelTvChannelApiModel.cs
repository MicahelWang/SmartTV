namespace YeahTVApi.DomainModel.Mapping
{
    using YeahTVApi.DomainModel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MapHotelTVChannelApiModel
    {
        public static List<HotelTVChannelApiModel> ToHotelTvChannelApiModel(this List<HotelTVChannel> hotelTVChannels, string adImgUrl)
        {
            var hotelTVChannelApiModels = new List<HotelTVChannelApiModel>();
            
            hotelTVChannels.ForEach(h=>
            {
                hotelTVChannelApiModels.Add(new HotelTVChannelApiModel 
                {
                    AdImgUrl = adImgUrl,
                    Category = h.Category,
                    CategoryEn = h.CategoryEn,
                    ChannelCode = h.ChannelCode,
                    ChannelId = h.ChannelId,
                    ChannelOrder = h.ChannelOrder,
                    HostAddress = h.HostAddress,
                    HotelId = h.HotelId,
                    Icon = h.Icon,
                    Name = h.Name,
                    NameEn = h.NameEn,
                });
            });
           
            return hotelTVChannelApiModels;
        }
    }
}
