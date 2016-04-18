namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel;
    using EntityFramework.Extensions;

    public class MovieTemplateRepertory : BaseRepertory<MovieTemplate, string>, IMovieTemplateRepertory
    {
        public override List<MovieTemplate> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as MovieCriteria;

            var query = Query(criteria);

            return query.ToPageList(criteria);
        }

        public bool Any(MovieCriteria movieCriteria)
        {
            return Query(movieCriteria).Any();
        }

        private IQueryable<MovieTemplate> Query(MovieCriteria criteria)
        {
            var query = base.Entities.Include("MovieTemplateRelations.Movie").AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));

            if (!string.IsNullOrEmpty(criteria.TemplateDescription))
                query = query.Where(q => q.Description.Contains(criteria.TemplateDescription));

            if (!string.IsNullOrEmpty(criteria.TemplateTags))
                query = query.Where(q => q.Tags.Contains(criteria.TemplateTags));

            if (!string.IsNullOrEmpty(criteria.TemplateTitle))
                query = query.Where(q => q.Title.Contains(criteria.TemplateTitle));

            if (!string.IsNullOrEmpty(criteria.MovieTemplateId))
                query = query.Where(q => q.Id.Equals(criteria.MovieTemplateId));

            if (!string.IsNullOrEmpty(criteria.MovieId))
                query = query.Where(q => q.MovieTemplateRelations.Any(a => a.MovieId.Contains(criteria.MovieId))); 

            return query;
        }
        public new List<MovieTemplate> GetAll()
        {
            return Entities.Include("MovieTemplateRelations.Movie").ToList();
        }
    }
}
