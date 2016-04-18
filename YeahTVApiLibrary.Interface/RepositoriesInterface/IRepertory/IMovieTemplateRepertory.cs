namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;

    public interface IMovieTemplateRepertory : IBsaeRepertory<MovieTemplate>
    {
        bool Any(MovieCriteria movieCriteria);
    }
}
