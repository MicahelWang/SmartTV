namespace YeahTVApiLibrary.Manager
{
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
    public class MovieTemplateManager : IMovieTemplateManager
    {
        private IMovieTemplateRepertory movieTemplateRepertory;
        private IHotelMovieTraceRepertory hotelMovieTraceRepertory;
        private IRedisCacheService redisCacheService;

        public MovieTemplateManager(IMovieTemplateRepertory movieTemplateRepertory, IHotelMovieTraceRepertory hotelMovieTraceRepertory,
            IRedisCacheService redisCacheService)
        {
            this.movieTemplateRepertory = movieTemplateRepertory;
            this.hotelMovieTraceRepertory = hotelMovieTraceRepertory;
            this.redisCacheService = redisCacheService;
        }

        #region Redis

        public List<MovieTemplate> GetAllFromCache()
        {
            return redisCacheService.GetAllFromCache(RedisKey.MovieTemplatesKey, movieTemplateRepertory.GetAll);
        }

        public void RemoveItemFromCache(string id)
        {
            redisCacheService.RemoveItemFromSet(RedisKey.MovieTemplatesKey, GetAllFromCache().FirstOrDefault(m => m.Id == id));
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }

        public void AddItemToCache(MovieTemplate entity)
        {
            var movieTemplateNew = SearchMovieTemplates(new MovieCriteria() { Id = entity.Id }).FirstOrDefault();
            redisCacheService.AddItemToSet(RedisKey.MovieTemplatesKey, movieTemplateNew, movieTemplateRepertory.GetAll);
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }
        public void AddRangeToCache(List<MovieTemplate> entitys)
        {
            entitys.AsParallel().ForAll(AddItemToCache);
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }
        public void UpdateCache(MovieTemplate entity)
        {
            var movieTemplateNew = SearchMovieTemplates(new MovieCriteria() { Id = entity.Id }).FirstOrDefault();
            redisCacheService.UpdateItemFromSet(RedisKey.MovieTemplatesKey,
                GetAllFromCache().Single(m => m.Id.Equals(entity.Id)), movieTemplateNew);
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }
        public void ReLoadMovieTemplateCache()
        {
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
            redisCacheService.Remove(RedisKey.MovieTemplatesKey);
            GetAllFromCache();
        }

        #endregion

        public List<MovieTemplate> SearchMovieTemplates(MovieCriteria movieCriteria)
        {
            return movieTemplateRepertory.Search(movieCriteria);
        }

        public void AddMovieTemplates(List<MovieTemplate> movieTemplates)
        {
            movieTemplateRepertory.Insert(movieTemplates);
            AddRangeToCache(movieTemplates);
        }

        public void AddMovieTemplates(MovieTemplate movieTemplate)
        {
            movieTemplateRepertory.Insert(movieTemplate);
            AddItemToCache(movieTemplate);
        }

        public void Update(MovieTemplate movieTemplate)
        {
            var movieTemplateDb = movieTemplateRepertory.FindByKey(movieTemplate.Id);

            movieTemplate.CopyTo(movieTemplateDb, new string[] { "Id","MovieTemplateRelations" });

            movieTemplateRepertory.Update(movieTemplateDb);
            UpdateCache(movieTemplateDb);
        }

        public bool Delete(MovieTemplate movieTemplate)
        {
            var movieTemplateDb = GetAllFromCache().Where(m => m.Id == movieTemplate.Id);
            //TODO:依赖循环引用，暂不用缓存
            var hotelMovieTrace = hotelMovieTraceRepertory.Search(new HotelMovieTraceCriteria { MoiveTemplateId = movieTemplate.Id });

            if ((!movieTemplateDb.Any() && movieTemplateDb.Any(m => m.MovieTemplateRelations.Any())) || hotelMovieTrace.Any())
            {
                return false;
            }
            else
            {
                movieTemplateRepertory.Delete(t => t.Id.Equals(movieTemplate.Id));
                RemoveItemFromCache(movieTemplate.Id);
                return true;
            }
        }
    }
}
