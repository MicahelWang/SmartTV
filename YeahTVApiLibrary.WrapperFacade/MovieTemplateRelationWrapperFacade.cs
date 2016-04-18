using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Manager;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.WrapperFacade
{
    public class MovieTemplateRelationWrapperFacade : IMovieTemplateRelationWrapperFacade
    {
        private IMovieTemplateRelationManager movieTemplateRelationManager;
        private IHotelMovieTraceManager hotelMovieTraceManager;

        public MovieTemplateRelationWrapperFacade(IMovieTemplateRelationManager movieTemplateRelationManager, IHotelMovieTraceManager hotelMovieTraceManager)
        {
            this.movieTemplateRelationManager = movieTemplateRelationManager;
            this.hotelMovieTraceManager = hotelMovieTraceManager;
        }

        public void DeleteMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation)
        {
            movieTemplateRelationManager.DeleteMovieTemplateRelation(movieTemplateRelation, movieTemplateRelationManager.DeleteMovieTemplateRelationWithTransaction);
            UpdateHotelMovieTrace(movieTemplateRelation);
        }

        public void AddMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation)
        {
            movieTemplateRelationManager.AddMovieTemplateRelation(movieTemplateRelation, movieTemplateRelationManager.AddMovieTemplateRelationWithTransaction);
            UpdateHotelMovieTrace(movieTemplateRelation);
        }

        public List<MovieTemplateRelation> SearchMovieTemplateRelations(MovieTemplateRelationCriteria movieTemplateRelationCriteria)
        {
            return movieTemplateRelationManager.SearchMovieTemplateRelations(movieTemplateRelationCriteria);
        }

        public void UpdateMovieTemplateRelation(MovieTemplateRelation movieTemplateRelation)
        {
            movieTemplateRelationManager.UpdateMovieTemplateRelation(movieTemplateRelation);
        }

        private void UpdateHotelMovieTrace(MovieTemplateRelation movieTemplateRelation)
        {
            var hotelModels = hotelMovieTraceManager.Search(new HotelMovieTraceCriteria() { MoiveTemplateId = movieTemplateRelation.MovieTemplateId }).Select(m => m.HotelId).Distinct();
            string tempid = movieTemplateRelation.MovieTemplateId;
            foreach (string item in hotelModels)
            {
                var model = new HotelMovieTrace();
                model.Id = item;
                model.Active = true;
                model.HotelId = item;
                model.MoiveTemplateId = tempid;
                hotelMovieTraceManager.UpdateMovieTraceManager(model);
            }
        }
    }
}
