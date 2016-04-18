namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IHotelTVChannelManager : IBaseManager<HotelTVChannel, HotelTVChannelCriteria>
    {
        [Cache]
        List<HotelTVChannel> SearchHotelTVChannels(RequestHeader header);
        
        List<HotelTVChannel> SearchHotelTVChannels(HotelTVChannelCriteria hotelTVChannelCriteria);

        void AddHotelTVChannel(HotelTVChannel hotelTVChannel);

        void AddHotelTVChannel(string hotelId, string lastUpdateUser);
        void UpdateHotelTVChannel(HotelTVChannel hotelTVChannel);

        void DeleteHotelTVChannel(HotelTVChannel hotelTVChannel);

        List<string> SearchOnlyHotelId(BaseSearchCriteria searchCriteria);
    }
}
