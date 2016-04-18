using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.Manager
{
    public class BaseManager<TModel, TCriteria> : IBaseManager<TModel, TCriteria>
        where TCriteria : BaseSearchCriteria, new()
        where TModel : class,new()
    {
        public IBsaeRepertory<TModel> ModelRepertory;

        public BaseManager(IBsaeRepertory<TModel> modelRepertory)
        {
            this.ModelRepertory = modelRepertory;
        }

        public List<TModel> SearchFromCache(TCriteria criteria)
        {
            return ModelRepertory.Search(criteria);
        }

        public List<TModel> Search(TCriteria criteria)
        {
            return ModelRepertory.Search(criteria);
        }

        public virtual void Add(TModel model)
        {
            ModelRepertory.Insert(model);
        }

        public virtual void Add(List<TModel> models)
        {
            ModelRepertory.Insert(models);
        }

        public virtual void Update(TModel model)
        {
            ModelRepertory.Update(model);
        }

        public virtual void Delete(TModel model)
        {
            ModelRepertory.Delete(model);
        }

        public virtual void Delete(List<TModel> models)
        {
            ModelRepertory.Delete(models);
        }
        public virtual void Delete(Expression<Func<TModel, bool>> filterExpression)
        {
            ModelRepertory.Delete(filterExpression);
        }
    }
}
