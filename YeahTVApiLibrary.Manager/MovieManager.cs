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
    public class MovieManager : IMovieManager
    {
        private IMovieRepertory movieRepertory;
        private IMovieTemplateManager movieMovieTemplateManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private ISysAttachmentManager sysAttachmentManager;
        private IRedisCacheService redisCacheService;

        public MovieManager(IMovieRepertory movieRepertory,
            IMovieTemplateManager movieMovieTemplateManager,
            IConstantSystemConfigManager constantSystemConfigManager,
            ISysAttachmentManager sysAttachmentManager,
            IRedisCacheService redisCacheService)
        {
            this.movieRepertory = movieRepertory;
            this.movieMovieTemplateManager = movieMovieTemplateManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.sysAttachmentManager = sysAttachmentManager;
            this.redisCacheService = redisCacheService;
        }

        #region Redis

        public List<Movie> GetAllFromCache()
        {
            return redisCacheService.GetAllFromCache(RedisKey.MovieSetsKey, movieRepertory.GetAll);
        }

        public void RemoveItemFromCache(string id)
        {
            redisCacheService.RemoveItemFromSet(RedisKey.MovieSetsKey, GetAllFromCache().FirstOrDefault(m => m.Id == id));
            movieMovieTemplateManager.ReLoadMovieTemplateCache();
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }

        public void AddItemToCache(Movie entity)
        {
            var movieNew = movieRepertory.Search(new MovieCriteria() { Id = entity.Id }).FirstOrDefault();
            redisCacheService.AddItemToSet(RedisKey.MovieSetsKey, movieNew, movieRepertory.GetAll);
            movieMovieTemplateManager.ReLoadMovieTemplateCache();
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }
        public void AddRangeToCache(List<Movie> entitys)
        {
            entitys.AsParallel().ForAll(AddItemToCache);
            movieMovieTemplateManager.ReLoadMovieTemplateCache();
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }
        public void UpdateCache(Movie entity)
        {
            var movieNew = movieRepertory.Search(new MovieCriteria() { Id = entity.Id }).FirstOrDefault();
            redisCacheService.UpdateItemFromSet(RedisKey.MovieSetsKey,
                GetAllFromCache().Single(m => m.Id.Equals(entity.Id)), movieNew);
            movieMovieTemplateManager.ReLoadMovieTemplateCache();
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }

        #endregion

        public List<Movie> SearchMovies(MovieCriteria movieCriteria)
        {
            var movies = movieRepertory.Search(movieCriteria);

            movies.ForEach(m =>
            {
                var file = sysAttachmentManager.GetById(int.Parse(m.CoverAddress));
                if (file != null)
                {
                    m.CoverPath = string.IsNullOrEmpty(m.CoverAddress) ?
                    "" : constantSystemConfigManager.ResourceSiteAddress + file.FilePath;
                }

                m.PosterAddress.Split(',').ToList().ForEach(p =>
                {
                    m.PosterPaths = new List<string>();
                    var postFile = sysAttachmentManager.GetById(int.Parse(p));

                    if (postFile != null)
                        m.PosterPaths.Add(constantSystemConfigManager.ResourceSiteAddress + postFile.FilePath);
                });
            });

            return movies;
        }

        public Movie FindByKey(string movieId)
        {
            return GetAllFromCache().FirstOrDefault(m => m.Id.Equals(movieId));
        }

        public void AddMovies(List<Movie> movies)
        {
            movieRepertory.Insert(movies);
            AddRangeToCache(movies);
        }

        public void AddMovies(Movie movie)
        {
            movieRepertory.Insert(movie);
            AddItemToCache(movie);
        }

        public void Update(Movie movie)
        {
            movieRepertory.Update(movie);
            UpdateCache(movie);
        }

        public bool Delete(Movie movie)
        {
            if (movieMovieTemplateManager.GetAllFromCache().Any(q => q.MovieTemplateRelations.Any(a => a.MovieId.Contains(movie.Id))))
            {
                return false;
            }
            else
            {
                movieRepertory.Delete(t => t.Id.Equals(movie.Id));
                RemoveItemFromCache(movie.Id);
                return true;
            }

        }
    }
}
