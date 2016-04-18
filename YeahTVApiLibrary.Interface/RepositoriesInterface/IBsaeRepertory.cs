namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.SearchCriteria;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
using System.Linq.Expressions;

    public interface IBsaeRepertory<TEntity> where TEntity : class, new()
    {
        void Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void Update(Expression<Func<TEntity, bool>> filterExpression, TEntity entity);
        int Update(Expression<Func<TEntity, bool>> Predicate, Expression<Func<TEntity, TEntity>> Updater);
        void Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);

        void Delete(Expression<Func<TEntity, bool>> filterExpression);

        TEntity FindByKey(object key);

        List<TEntity> GetAll();

        DbSet<TEntity> GetFakeAll();

        List<TEntity> Search(BaseSearchCriteria searchCriteria);
       
    }
}
