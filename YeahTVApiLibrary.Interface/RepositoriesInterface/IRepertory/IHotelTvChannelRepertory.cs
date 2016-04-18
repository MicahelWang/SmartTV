namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;

    public interface IHotelTVChannelRepertory : IBsaeRepertory<HotelTVChannel>
    {
        List<string> SearchOnlyHotelId(BaseSearchCriteria searchCriteria);
    }
}
