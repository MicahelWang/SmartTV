namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.ViewModels;
    using YeahTVApi.DomainModel.SearchCriteria;

    public interface IHotelMovieTraceRepertory : IBsaeRepertory<HotelMovieTrace>
    {
        List<HotelMovieTrace> GetAllWithInclude();
        List<HotelMovieTraceViewModel> SearchForHotelTemplate(HotelMovieTraceCriteria searchCriteria);
    }
}
