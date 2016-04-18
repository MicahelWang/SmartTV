using YeahTVApi.Common;

namespace YeahTVApiLibrary.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IHotelMovieTraceNoTemplateManager
    { 
        List<HotelMovieTraceNoTemplate> SearchHotelMovieTraceNoTemplates(HotelMovieTraceNoTemplateCriteria criteria);
        void AddHotelMovieTraceNoTemplates(List<HotelMovieTraceNoTemplate> templates);
        void AddHotelMovieTraceNoTemplate(HotelMovieTraceNoTemplate template);
        void Update(HotelMovieTraceNoTemplate template);
        int Update(Expression<Func<HotelMovieTraceNoTemplate, bool>> Predicate, Expression<Func<HotelMovieTraceNoTemplate, HotelMovieTraceNoTemplate>> Updater);
        void Delete(HotelMovieTraceNoTemplate template);
        void DeleteByMovieId(string movieId);
        void BatchChangeIsDelete(List<HotelMovieTraceNoTemplate> readyChanges, bool isDelete);        
        [UnitOfWork]
        List<HotelMovieTraceNoTemplate> AddRangTransaction(List<HotelMovieTraceNoTemplate> hotelMovieTraceNoTemplates);

        [UnitOfWork]
        T RunTransaction<T>(Func<T> fun);

        List<HotelMovieTraceNoTemplate> SearchWithLocalize(BaseSearchCriteria searchCriteria);

    }
}
