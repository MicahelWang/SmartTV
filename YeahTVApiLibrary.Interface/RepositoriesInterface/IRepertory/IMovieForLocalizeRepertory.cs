using System;
using System.Linq.Expressions;

namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;

    public interface IMovieForLocalizeRepertory : IBsaeRepertory<MovieForLocalize>
    {
        void ChangeHotelCount(Expression<Func<MovieForLocalize, bool>> filterExpression, Func<int?, int> changeFun);
        List<MovieForLocalize> SearchMoiveWithLocalize(BaseSearchCriteria searchCriteria);
    }
}
