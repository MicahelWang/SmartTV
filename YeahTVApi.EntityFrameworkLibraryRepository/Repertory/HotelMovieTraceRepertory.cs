namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models.ViewModels;
    using YeahTVApi.DomainModel.Models.DataModel;
    using System;

    public class HotelMovieTraceRepertory : BaseRepertory<HotelMovieTrace, string>, IHotelMovieTraceRepertory
    {
        public List<HotelMovieTrace> GetAllWithInclude()
        {
            return Entities.Include("Movie.MovieTemplateRelations").Include("MovieTemplate").ToList();
        }

        public override List<HotelMovieTrace> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HotelMovieTraceCriteria;

            var query = base.Entities
                .Include("Movie.MovieTemplateRelations")
                .Include("MovieTemplate")
                .AsQueryable();

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));

            if (!string.IsNullOrEmpty(criteria.MovieId))
                query = query.Where(q => q.MovieId.Equals(criteria.MovieId)
                    && q.Movie.MovieTemplateRelations.Any(r => r.MovieId.Equals(q.MovieId)));

            if (criteria.MaxViewCount.HasValue)
                query = query.Where(q => q.ViewCount.Value <= criteria.MaxViewCount.Value);

            if (criteria.MinViewCount.HasValue)
                query = query.Where(q => q.ViewCount.Value <= criteria.MinViewCount.Value);

            if (criteria.IsDownload.HasValue)
                query = query.Where(q => q.IsDownload.Equals(criteria.IsDownload.Value));

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));

            if (!string.IsNullOrEmpty(criteria.MoiveTemplateId))
                query = query.Where(q => q.MoiveTemplateId.Equals(criteria.MoiveTemplateId));

            if (criteria.SortFiled.Equals("Id"))
                criteria.SortFiled = "LastViewTime";

            return query.ToPageList(searchCriteria);
        }

        public List<HotelMovieTraceViewModel> SearchForHotelTemplate(HotelMovieTraceCriteria searchCriteria)
        {
            var criteria = searchCriteria as HotelMovieTraceCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));

            if (!string.IsNullOrEmpty(criteria.MovieId))
                query = query.Where(q => q.MovieId.Equals(criteria.MovieId)
                    && q.Movie.MovieTemplateRelations.Any(r => r.MovieId.Equals(q.MovieId)));

            if (criteria.MaxViewCount.HasValue)
                query = query.Where(q => q.ViewCount.Value <= criteria.MaxViewCount.Value);

            if (criteria.MinViewCount.HasValue)
                query = query.Where(q => q.ViewCount.Value <= criteria.MinViewCount.Value);

            if (criteria.IsDownload.HasValue)
                query = query.Where(q => q.IsDownload.Equals(criteria.IsDownload.Value));

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));


            criteria.SortFiled = "HotelId";

            var result = (from hotelMovieTrace in query
                          join hotelTeamplate in base.Context.Set<MovieTemplate>()
                          on hotelMovieTrace.MoiveTemplateId equals hotelTeamplate.Id
                          where hotelTeamplate.Title.Equals(string.IsNullOrEmpty(criteria.MoiveTemplateName) ? hotelTeamplate.Title : criteria.MoiveTemplateName)
                          orderby hotelMovieTrace.Order descending
                          select new HotelMovieTraceViewModel
                         {
                             HotelId = hotelMovieTrace.HotelId,
                             MoiveTemplateDescription = hotelTeamplate.Description,
                             MoiveTemplateId = hotelMovieTrace.MoiveTemplateId,
                             MoiveTemplateName = hotelTeamplate.Title,
                             MoiveTemplateTages = hotelTeamplate.Tags

                         }).Distinct().ToPageList(searchCriteria);

            return result;
        }
    }
}
