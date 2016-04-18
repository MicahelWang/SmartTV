using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IBaseManager<TModel, TCriteria>
        where TCriteria : BaseSearchCriteria, new()
        where TModel : class,new()
    {
        [Cache]
        List<TModel> SearchFromCache(TCriteria criteria);

        List<TModel> Search(TCriteria criteria);

        void Add(TModel model);

        void Add(List<TModel> models);

        void Update(TModel model);

        void Delete(TModel model);

        void Delete(List<TModel> models);
        void Delete(Expression<Func<TModel, bool>> filterExpression);
    }
}
