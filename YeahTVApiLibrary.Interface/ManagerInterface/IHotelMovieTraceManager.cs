namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.Models.ViewModels;
    using YeahTVApi.Common;

    public interface IHotelMovieTraceManager
    {
        List<HotelMovieTrace> Search(HotelMovieTraceCriteria criteria);

        [UnitOfWork]
        void AddMovieTraceManager(HotelMovieTrace trace);

        [UnitOfWork]
        void UpdateMovieTraceManager(HotelMovieTrace trace);
        bool UpdateMoviveTraceDownStatus(HotelMovieTrace trace);
        void UpdateMoviveTraceList(List<HotelMovieTrace> list,bool isDown);

        List<MovieApiModel> SearchMoviesForApi(RequestHeader header);
   
        List<HotelMovieTraceViewModel> SearchForHotelTemplate(HotelMovieTraceCriteria searchCriteria);
        List<HotelMovieTrace> GetAllFromCache();
    }
}
