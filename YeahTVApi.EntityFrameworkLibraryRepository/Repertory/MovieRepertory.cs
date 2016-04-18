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

    public class MovieRepertory : BaseRepertory<Movie, string>, IMovieRepertory
    {
        public override List<Movie> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as MovieCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));

            if (!string.IsNullOrEmpty(criteria.MovieReview))
                query = query.Where(q => q.MovieReview.Contains(criteria.MovieReview));

            if (!string.IsNullOrEmpty(criteria.MovieReviewEn))
                query = query.Where(q => q.MovieReviewEn.Contains(criteria.MovieReviewEn));

            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(q => q.Name.Contains(criteria.Name));

            if (!string.IsNullOrEmpty(criteria.NameEn))
                query = query.Where(q => q.NameEn.Contains(criteria.NameEn));

            return query.ToPageList(criteria);
        }
    }
}
