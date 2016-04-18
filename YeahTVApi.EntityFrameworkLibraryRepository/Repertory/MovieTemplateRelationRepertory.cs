using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class MovieTemplateRelationRepertory : BaseRepertory<MovieTemplateRelation, string>, IMovieTemplateRelationRepertory
    {
        public override List<MovieTemplateRelation> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as MovieTemplateRelationCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.MovieId))
              query =  query.Where(q => q.MovieId.Equals(criteria.MovieId));

            if(!string.IsNullOrEmpty(criteria.MovieTemplateId))
                query = query.Where(q => q.MovieTemplateId.Equals(criteria.MovieTemplateId));

            return query.ToList();
        }

        public new List<MovieTemplateRelation> GetAll()
        {
            return base.Entities.ToList();
        }
    }
}
