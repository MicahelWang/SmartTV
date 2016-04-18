using YeahTVApi.DomainModel;

namespace YeahTVApi.Manager
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel.Mapping;
    using System;
    using YeahTVApi.DomainModel.Models.DataModel;

    public class MovieTemplateRelationManager : IMovieTemplateRelationManager
    {
        private IMovieTemplateRelationRepertory movieTemplateRelationRepertory;
        private IMovieTemplateManager movieMovieTemplateManager;
        private IRedisCacheService redisCacheService;

        public MovieTemplateRelationManager(
            IMovieTemplateRelationRepertory movieTemplateRelationRepertory,
            IMovieTemplateManager movieMovieTemplateManager,
            IRedisCacheService redisCacheService)
        {
            this.movieTemplateRelationRepertory = movieTemplateRelationRepertory;
            this.movieMovieTemplateManager = movieMovieTemplateManager;
            this.redisCacheService = redisCacheService;
        }

        #region Redis

        public List<MovieTemplateRelation> GetAllFromCache()
        {
            return redisCacheService.GetAllFromCache(RedisKey.MovieTemplateRelationsKey, movieTemplateRelationRepertory.GetAll);
        }

        public void RemoveItemByExpressionFromCache(Func<MovieTemplateRelation, bool> fun)
        {
            GetAllFromCache().Where(fun).ToList().ForEach(m => redisCacheService.RemoveItemFromSet(RedisKey.MovieTemplateRelationsKey, m));

            movieMovieTemplateManager.ReLoadMovieTemplateCache();
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }

        public void AddItemToCache(MovieTemplateRelation entity)
        {
            var movieTemplateRelationNew = movieTemplateRelationRepertory.Search(new MovieTemplateRelationCriteria() { MovieId = entity.MovieId, MovieTemplateId = entity.MovieTemplateId }).FirstOrDefault();
            redisCacheService.AddItemToSet(RedisKey.MovieTemplateRelationsKey, movieTemplateRelationNew, movieTemplateRelationRepertory.GetAll);

            movieMovieTemplateManager.ReLoadMovieTemplateCache();
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }
        public void UpdateCache(MovieTemplateRelation entity)
        {
            var movieTemplateRelationNew = movieTemplateRelationRepertory.Search(new MovieTemplateRelationCriteria() { MovieId = entity.MovieId, MovieTemplateId = entity.MovieTemplateId }).FirstOrDefault();
            redisCacheService.UpdateItemFromSet(RedisKey.MovieTemplateRelationsKey,
                GetAllFromCache().Single(m => m.MovieId.Equals(entity.MovieId) && m.MovieTemplateId.Equals(entity.MovieTemplateId)), movieTemplateRelationNew);

            movieMovieTemplateManager.ReLoadMovieTemplateCache();
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }

        #endregion

        public List<MovieTemplateRelation> SearchMovieTemplateRelations(MovieTemplateRelationCriteria criteria)
        {
            try
            {
                return movieTemplateRelationRepertory.Search(criteria);
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("SearchMovieTemplateRelations Error!", ex);
            }
        }

        public void AddMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation, Func<MovieTemplateRelation, MovieTemplateRelation> transactionMethod)
        {
            AddItemToCache(transactionMethod(movieTemplateRelation));
        }

        [UnitOfWork]
        public MovieTemplateRelation AddMovieTemplateRelationWithTransaction(MovieTemplateRelation movieTemplateRelation)
        {
            try
            {
                var exitTemplate = movieMovieTemplateManager.GetAllFromCache().FirstOrDefault(m => m.Id == movieTemplateRelation.MovieTemplateId);

                if (exitTemplate != null)
                {
                    exitTemplate.MovieCount++;
                    movieMovieTemplateManager.Update(exitTemplate);

                    movieTemplateRelationRepertory.Insert(movieTemplateRelation);
                    return movieTemplateRelation;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("AddMovieTemplateRelation Error!", ex);
            }
        }

        public void UpdateMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation)
        {
            try
            {
                var movieTemplateRelationDb = GetAllFromCache().FirstOrDefault(m => m.MovieTemplateId.Equals(movieTemplateRelation.MovieTemplateId) && m.MovieId.Equals(movieTemplateRelation.MovieId));

                movieTemplateRelation.CopyTo(movieTemplateRelationDb, new string[] { "Id" });

                movieTemplateRelationRepertory.Update(movieTemplateRelationDb);
                UpdateCache(movieTemplateRelationDb);
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("UpdateMovieTemplateRelation Error!", ex);
            }
        }

        public void DeleteMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation, Action<MovieTemplateRelation> transactionMethod)
        {
            transactionMethod(movieTemplateRelation);
            var expression = new Func<MovieTemplateRelation, bool>(m => m.MovieId.Equals(movieTemplateRelation.MovieId)
                && m.MovieTemplateId.Equals(movieTemplateRelation.MovieTemplateId));
            RemoveItemByExpressionFromCache(expression);
        }

        [UnitOfWork]
        public void DeleteMovieTemplateRelationWithTransaction(MovieTemplateRelation movieTemplateRelation)
        {
            try
            {
                var exitTemplate = movieMovieTemplateManager.GetAllFromCache().FirstOrDefault(m => m.Id == movieTemplateRelation.MovieTemplateId);

                if (exitTemplate != null)
                {
                    exitTemplate.MovieCount--;
                    movieMovieTemplateManager.Update(exitTemplate);
                    movieTemplateRelationRepertory.Delete(m => m.MovieId.Equals(movieTemplateRelation.MovieId)
                        && m.MovieTemplateId.Equals(movieTemplateRelation.MovieTemplateId));
                }
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("DeleteMovieTemplateRelation Error!", ex);
            }
        }
    }
}
