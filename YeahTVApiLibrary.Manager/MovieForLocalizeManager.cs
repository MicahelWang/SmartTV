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
    public class MovieForLocalizeManager :BaseManager<MovieForLocalize,MovieForLocalizeCriteria>, IMovieForLocalizeManager
    {
        private IMovieForLocalizeRepertory movieForLocalizeRepertory;
        private IRedisCacheService redisCacheService;

        public MovieForLocalizeManager(IMovieForLocalizeRepertory _movieForLocalizeRepertory,
            IRedisCacheService redisCacheService):
            base(_movieForLocalizeRepertory)
        {
            this.movieForLocalizeRepertory = _movieForLocalizeRepertory;
            this.redisCacheService = redisCacheService;
        }

        #region Redis

        public List<MovieForLocalize> GetAllFromCache()
        {
            return redisCacheService.GetAllFromCache(RedisKey.MovieForLocalizeKey, movieForLocalizeRepertory.GetAll);
        }           
         
        #endregion
        public List<MovieForLocalize> SearchMovieForLocalizes(MovieForLocalizeCriteria criteria)
        {
            return movieForLocalizeRepertory.SearchMoiveWithLocalize(criteria);
        }
        public MovieForLocalize FindByKey(string movieId)
        {
            return movieForLocalizeRepertory.SearchMoiveWithLocalize(new MovieForLocalizeCriteria { Id = movieId }).FirstOrDefault();            
        }
          
        public new void Delete(MovieForLocalize movieForLocalize)
        { 
            base.ModelRepertory.Delete(t => t.Id.Equals(movieForLocalize.Id));
        }

        public List<MovieForLocalize> ChangeHotelCount(List<MovieForLocalize> movies, Func<int?, int> changeFun)
        {
            var ids = movies.Select(m => m.Id).ToList();
            movieForLocalizeRepertory.ChangeHotelCount(m => ids.Contains(m.Id), changeFun); 
            return movies;
        }
    }
}
