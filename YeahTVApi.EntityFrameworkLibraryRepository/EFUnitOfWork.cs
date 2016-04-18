namespace YeahTVApiLibrary.EntityFrameworkRepository
{
    using YeahTVApi.DomainModel;
    using YeahTVApiLibrary.EntityFrameworkRepository.Models;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
using System.Data;

    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        [ThreadStatic]
        private static EFUnitOfWork s_current;
        private DbContextTransaction transaction;
        private YeahTVLibraryContext c;

        public DbContext Context { get; set; }
        
        public static EFUnitOfWork Current
        {
            get
            {
                return s_current;
            }
            set
            {
                s_current = value;
            }
        }

        public EFUnitOfWork()
        {
            Context = new YeahTVLibraryContext(Constant.NameOrConnectionString);
        }

        public EFUnitOfWork(DbContext context)
        {
            Context = context;
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            transaction = Context.Database.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            try
            {
                Context.SaveChanges();
                transaction.Commit();
            }
            catch (DbEntityValidationException dbEx)
            {
                var s = dbEx.EntityValidationErrors;
                throw dbEx;
            }
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            Context.Dispose();
            Context = null;
        }
    }
}
