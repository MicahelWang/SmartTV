using System;

namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IMovieTemplateRelationManager
    {
        List<MovieTemplateRelation> SearchMovieTemplateRelations(MovieTemplateRelationCriteria movieTemplateRelationCriteria);

        void UpdateMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation);

        void DeleteMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation, Action<MovieTemplateRelation> transactionMethod);

        /// <summary>
        /// 解决缓存和事务冲突，涉及缓存请使用DeleteMovieTemplateRelation
        /// </summary>
        /// <param name="movieTemplateRelation"></param>
        [UnitOfWork]
        void DeleteMovieTemplateRelationWithTransaction(MovieTemplateRelation movieTemplateRelation);

        void AddMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation, Func<MovieTemplateRelation, MovieTemplateRelation> transactionMethod);

        /// <summary>
        /// 解决缓存和事务冲突，涉及缓存请使用AddMovieTemplateRelation
        /// </summary>
        /// <param name="movieTemplateRelation"></param>
        [UnitOfWork]
        MovieTemplateRelation AddMovieTemplateRelationWithTransaction(MovieTemplateRelation movieTemplateRelation);

        List<MovieTemplateRelation> GetAllFromCache();
    }
}
