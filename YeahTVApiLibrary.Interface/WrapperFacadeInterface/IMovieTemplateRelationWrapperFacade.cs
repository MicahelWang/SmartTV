using System.Collections.Generic;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IMovieTemplateRelationWrapperFacade 
    {

        List<MovieTemplateRelation> SearchMovieTemplateRelations(MovieTemplateRelationCriteria movieTemplateRelationCriteria);

        void UpdateMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation);

        void DeleteMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation);

        void AddMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation);
    }
}
