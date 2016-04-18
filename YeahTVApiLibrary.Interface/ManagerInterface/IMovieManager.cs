namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IMovieManager
    {
        List<Movie> SearchMovies(MovieCriteria movieCriteria);
        Movie FindByKey(string movieId);

        void AddMovies(List<Movie> movies);

        void AddMovies(Movie movie);

        void Update(Movie movie);

        bool Delete(Movie movie);
        List<Movie> GetAllFromCache();
    }
}
