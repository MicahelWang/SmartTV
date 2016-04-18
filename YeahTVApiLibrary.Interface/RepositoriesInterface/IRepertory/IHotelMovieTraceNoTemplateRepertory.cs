using System;
using System.Linq.Expressions;

namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;

    public interface IHotelMovieTraceNoTemplateRepertory : IBsaeRepertory<HotelMovieTraceNoTemplate>
    {
        void BatchChangeIsDelete(Expression<Func<HotelMovieTraceNoTemplate, bool>> filterExpression, bool isDelete);
        List<HotelMovieTraceNoTemplate> SearchWithLocalize(BaseSearchCriteria searchCriteria);
    }
}
