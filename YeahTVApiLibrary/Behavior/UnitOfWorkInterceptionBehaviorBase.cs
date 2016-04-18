namespace YeahTVApiLibrary.Behavior
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.EntityFrameworkRepository;
    using YeahTVApiLibrary.EntityFrameworkRepository.Models;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Data;
    using YeahTVApiLibrary.Service.Cache;
    using YeahTVApiLibrary.Infrastructure;
    using System.Reflection;
    using YeahTVApi.Common;

    using Newtonsoft.Json;

    public class UnitOfWorkInterceptionBehaviorBase : IInterceptionBehavior
    {
        public IMethodReturn Invoke(
            IMethodInvocation input,
            GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn retvalue;

            if (EFUnitOfWork.Current != null || !UnitOfWorkHelper.IsDBConnectionRequired(input.MethodBase))
            {
                retvalue = CacheBehavior.ReturnCacheValue(input, getNext);
                retvalue = retvalue == null ? getNext()(input, getNext) : retvalue;

                return retvalue;
            }

            try
            {
                EFUnitOfWork.Current = CreateUnitOfWork();
                var isolationLevel = IsolationLevel.ReadUncommitted;

                if (UnitOfWorkHelper.NeedTransaction(input.MethodBase))
                {
                    isolationLevel = IsolationLevel.ReadCommitted;
                }

                EFUnitOfWork.Current.BeginTransaction(isolationLevel);

                try
                {
                    retvalue = getNext()(input, getNext);
                    EFUnitOfWork.Current.Commit();
                }
                catch (Exception ex)
                {
                    if (ex is DbEntityValidationException)
                        ex = ex as DbEntityValidationException;

                    try
                    {
                        EFUnitOfWork.Current.Rollback();
                    }
                    catch
                    {
                    }

                    throw new ApiException("方法" + input.MethodBase.Name + "执行记录： " + ex.ToString());
                }
            }
            catch (Exception ex)
            {

                throw new ApiException("方法" + input.MethodBase.Name + "执行记录： " + ex.ToString());
            }
            finally
            {
                EFUnitOfWork.Current.Context.Dispose();
                EFUnitOfWork.Current = null;
            }

            return retvalue;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }

        protected virtual EFUnitOfWork CreateUnitOfWork()
        {
            return new EFUnitOfWork(new YeahTVLibraryContext(Constant.NameOrConnectionString));
        }
    }
}