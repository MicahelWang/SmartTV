namespace YeahTVApiLibrary.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IMovieForLocalizeManager : IBaseManager<MovieForLocalize, MovieForLocalizeCriteria>
    {
        List<MovieForLocalize> GetAllFromCache();
        List<MovieForLocalize> SearchMovieForLocalizes(MovieForLocalizeCriteria criteria);
        MovieForLocalize FindByKey(string movieLocalizeId);
        [UnitOfWork]
        List<MovieForLocalize> ChangeHotelCount(List<MovieForLocalize> movies, Func<int?, int> changeFun);
    }
}
