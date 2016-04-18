namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using EntityFramework.Extensions;
    using YeahTVApi.Common;
    using System.Linq.Expressions;
    using System.Reflection;
    using System;
    using YeahTVApiLibrary.EntityFrameworkRepository.Helper;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using YeahTVApiLibrary.EntityFrameworkRepository.Mapping;

    public abstract class BaseRepertory<TEntity, Key> : IBsaeRepertory<TEntity> where TEntity : BaseEntity<Key>, new()
    {
        public DbContext Context
        {
            get
            {
                return EFUnitOfWork.Current.Context;
            }
        }

        public DbSet<TEntity> Entities
        {
            get { return Context.Set<TEntity>(); }
        }

        public void Create(TEntity entity)
        {
            Entities.Add(entity);
        }

        public List<TEntity> GetAll()
        {
            return Entities.ToList();
        }

        public DbSet<TEntity> GetFakeAll()
        {
            return Entities;
        }

        public void Insert(TEntity entity)
        {
            Entities.Add(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(Expression<Func<TEntity, bool>> filterExpression,TEntity entity)
        {
            Entities.Update(filterExpression, t => entity);
        }

        /// <summary>
        /// 更新满足条件的实体，返回更新实体的条数
        /// </summary>
        /// <typeparam name="T">更新的类型</typeparam>
        /// <param name="Entity">实体类</param>
        /// <param name="Predicate">更新的条件</param>
        /// <param name="Updater">更新的值</param>
        /// <returns>int</returns>
        public int Update(Expression<Func<TEntity, bool>> Predicate, Expression<Func<TEntity, TEntity>> Updater)
        {
            ConditionBuilder Builder = new ConditionBuilder();

            Builder.Build(Predicate.Body);
            string sqlCondition = Builder.Condition;
            //获取Update的赋值语句
            var updateMemberExpr = (MemberInitExpression)Updater.Body;
            var updateMemberCollection = updateMemberExpr.Bindings.Cast<MemberAssignment>().Select(c => new
            {
                Name = c.Member.Name,
                Value = Expression.Lambda(c.Expression, null).Compile().DynamicInvoke()
            });
            
            int i = Builder.Arguments.Length;
            string sqlUpdateBlock = string.Join(", ", updateMemberCollection.Select(c => string.Format("`{0}`={1}", Operate.GetEntityDBName<TEntity>(c.Name), "{" + (i++) + "}")).ToArray());
            string commandText = string.Format("Update {0} Set {1} Where {2}", Operate.GetTableName<TEntity>(), sqlUpdateBlock, Operate.GetEntityDBName<TEntity>(sqlCondition));
            //获取SQL参数数组 (包括查询参数和赋值参数)
            var args = Builder.Arguments.Union(updateMemberCollection.Select(c => c.Value)).ToArray();
            var result = Entities.GetContext().ExecuteStoreCommand(commandText, args);


            return result;
        }

        public virtual void Delete(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            var entities = Entities.Where(filterExpression);
            Entities.RemoveRange(entities);
        }

        public void DeleteByKey(object Id)
        {
            Entities.Where(e=>e.Id.Equals(Id)).Delete();
        }

        public TEntity FindByKey(object key)
        {
            return Entities.Find(key);
        }

        /// <summary>
        /// should implementation of sub class
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public abstract List<TEntity> Search(BaseSearchCriteria searchCriteria);
    }

    public static class RepertoryUtils
    {
        public static List<TEntity> ToPageList<TEntity>(this IQueryable<TEntity> query, BaseSearchCriteria searchCriteria) where TEntity : class
        {
            return query.ToPageQueryable(searchCriteria).ToList();
        }
        public static IQueryable<TEntity> ToPageQueryable<TEntity>(this IQueryable<TEntity> query, BaseSearchCriteria searchCriteria) where TEntity : class
        {
            query = query.OrderBy(searchCriteria.SortFiled.Split(','), searchCriteria.OrderAsc);

            if (searchCriteria.NeedPaging)
            {
                try
                {
                    searchCriteria.TotalCount = query.FutureCount();
                }
                catch
                {
                    searchCriteria.TotalCount = query.Count();
                }

                query = query.Page(searchCriteria.PageSize, searchCriteria.Page);
            }

            return query;
        }
        /// <summary>
        /// 针对roomNo排序暂用方法，需优化
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> ToPageListQueryable<TEntity>(this IQueryable<TEntity> query, BaseSearchCriteria searchCriteria) where TEntity : class
        {
            query = query.OrderBy(searchCriteria.SortFiled.Split(','), searchCriteria.OrderAsc);

            if (searchCriteria.NeedPaging)
            {
                searchCriteria.TotalCount = query.FutureCount();
                query = query.Page(searchCriteria.PageSize, searchCriteria.Page);
            }

            return query;
        }
    }


}
