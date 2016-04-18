namespace HZTVApi.EntityFrameworkRepository
{
    using HZTVApi.DomainModel.SearchCriteria;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IBsaeRepertory<TEntity> where TEntity : class, new()
    {
        void Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        TEntity FindByKey(object key);

        List<TEntity> GetAll();

        List<TEntity> Search(BaseSearchCriteria searchCriteria);
    }
}
