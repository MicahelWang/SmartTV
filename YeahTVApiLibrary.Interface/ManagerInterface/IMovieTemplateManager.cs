namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IMovieTemplateManager
    {
        List<MovieTemplate> SearchMovieTemplates(MovieCriteria movieCriteria);

        void AddMovieTemplates(List<MovieTemplate> movieTemplates);

        void AddMovieTemplates(MovieTemplate movieTemplate);

        void Update(MovieTemplate movieTemplate);

        bool Delete(MovieTemplate movieTemplate);
        List<MovieTemplate> GetAllFromCache();
        void UpdateCache(MovieTemplate entity);
        void ReLoadMovieTemplateCache();
    }
}
