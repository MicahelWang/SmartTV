using System.Data;
namespace YeahTVApiLibrary.Infrastructure
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// start a transaction against the underlying connection
        /// </summary>
        void BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// write out all changes to data source
        /// </summary>
        void Commit();

        /// <summary>
        /// rollback all changes to data source
        /// </summary>
        void Rollback();
    }
}
