using System.Linq.Expressions;

namespace YeahTVApiLibrary.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;

    /// <summary>
    /// 
    /// </summary>
    public class HotelMovieTraceNoTemplateManager :BaseManager<HotelMovieTraceNoTemplate,HotelMovieTraceNoTemplateCriteria>, IHotelMovieTraceNoTemplateManager
    {
        private IHotelMovieTraceNoTemplateRepertory hotelMovieTraceNoTemplateRepertory;
        private IRedisCacheService redisCacheService;

        public HotelMovieTraceNoTemplateManager(IHotelMovieTraceNoTemplateRepertory hotelMovieTraceNoTemplateRepertory,
            IRedisCacheService redisCacheService):
            base(hotelMovieTraceNoTemplateRepertory)
        {
            this.hotelMovieTraceNoTemplateRepertory = hotelMovieTraceNoTemplateRepertory;
            this.redisCacheService = redisCacheService;
        }        
        public List<HotelMovieTraceNoTemplate> SearchHotelMovieTraceNoTemplates(HotelMovieTraceNoTemplateCriteria criteria)
        {
            return base.ModelRepertory.Search(criteria);
        } 
        public void AddHotelMovieTraceNoTemplates(List<HotelMovieTraceNoTemplate> hotelMovieTraceNoTemplates)
        {
            base.ModelRepertory.Insert(hotelMovieTraceNoTemplates);             
        }

        public void AddHotelMovieTraceNoTemplate(HotelMovieTraceNoTemplate hotelMovieTraceNoTemplates)
        {
            base.ModelRepertory.Insert(hotelMovieTraceNoTemplates);           
        }

        public override void Update(HotelMovieTraceNoTemplate movie)
        {
            base.ModelRepertory.Update(movie);             
        }
        public void BatchChangeIsDelete(List<HotelMovieTraceNoTemplate> readyChanges, bool isDelete)
        {
            var keys = readyChanges.Select(m => m.HotelId + m.MovieId).ToList();
            hotelMovieTraceNoTemplateRepertory.BatchChangeIsDelete(h => keys.Contains(h.HotelId + h.MovieId), isDelete); 
        }
       
        public override void Delete(HotelMovieTraceNoTemplate entity)
        {
            var movieTraceNoTemplate = SearchHotelMovieTraceNoTemplates(new HotelMovieTraceNoTemplateCriteria { MovieId = entity.Id, HotelId = entity.HotelId })
                .FirstOrDefault();
            if (movieTraceNoTemplate != null)
            {
                movieTraceNoTemplate.IsDelete = true;
                base.ModelRepertory.Update(movieTraceNoTemplate);               
            }
        }
        public void DeleteByMovieId(string movieId)
        {
            var movieTraceNoTemplates = SearchHotelMovieTraceNoTemplates(new HotelMovieTraceNoTemplateCriteria { MovieId = movieId});
            BatchChangeIsDelete(movieTraceNoTemplates, true);
        }

        [UnitOfWork]
        public List<HotelMovieTraceNoTemplate> AddRangTransaction(List<HotelMovieTraceNoTemplate> hotelMovieTraceNoTemplates)
        {
            base.ModelRepertory.Insert(hotelMovieTraceNoTemplates);
            return hotelMovieTraceNoTemplates;
        }

        [UnitOfWork]
        public T RunTransaction<T>(Func<T> fun)
        {
            return fun();
        }

        public int Update(Expression<Func<HotelMovieTraceNoTemplate, bool>> Predicate, Expression<Func<HotelMovieTraceNoTemplate, HotelMovieTraceNoTemplate>> Updater)
        {
            return base.ModelRepertory.Update(Predicate, Updater);
        }

        public List<HotelMovieTraceNoTemplate> SearchWithLocalize(BaseSearchCriteria searchCriteria)
        {
            return hotelMovieTraceNoTemplateRepertory.SearchWithLocalize(searchCriteria);
        }
    }
}
