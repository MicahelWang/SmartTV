using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
using YeahTVApiLibrary.EntityFrameworkRepository;
using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.SearchCriteria;
using System.Linq;
using YeahTVApi.DomainModel;
using YeahTVApiLibrary.EntityFrameworkRepository.Models;
using System.Data;

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.EntityFrameworkRepositoryTest
{
    [TestClass]
    public class BaseRepertoryTest<TEntity, Key> where TEntity : BaseEntity<Key>, new()
    {
        public BaseRepertory<TEntity, Key> entityRepertory { get; set; }
        public List<TEntity> mockEntities { get; set; }

        public Func<BaseRepertory<TEntity, Key>> SetRepertory { get; set; }
        public Func<List<TEntity>> GetMockEntities { get; set; }

        [TestInitialize]
        public void Setup()
        {
            EFUnitOfWork.Current = new EFUnitOfWork();

            entityRepertory = SetRepertory.Invoke();

            EFUnitOfWork.Current.BeginTransaction(IsolationLevel.ReadUncommitted);

            mockEntities = GetMockEntities.Invoke();

            if (entityRepertory.Entities.Any())
            {
                var exitEntities = entityRepertory.GetAll();
                entityRepertory.Delete(exitEntities);
            }

            entityRepertory.Insert(mockEntities);

            EFUnitOfWork.Current.Commit();
           
        }

        [TestCleanup]
        public void Shutdown()
        {
            EFUnitOfWork.Current.BeginTransaction(IsolationLevel.ReadUncommitted);

            entityRepertory.Delete(mockEntities);

            EFUnitOfWork.Current.Commit();
            EFUnitOfWork.Current = null;
        }
    }
}
