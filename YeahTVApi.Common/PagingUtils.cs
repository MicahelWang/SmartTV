using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Common
{
    public static class PagingUtils
    {
        public static IEnumerable<T> Page<T>(this IEnumerable<T> en, int pageSize, int page)
        {
            page = page == 0 ? 0 : page;
            return en.Skip(page * pageSize).Take(pageSize);
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> en, int pageSize, int page)
        {
            page = page == 0 ? 0 : page;
            return en.Skip(page * pageSize).Take(pageSize);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string[] propertyNames, bool ascending) where T : class
        {
            for (int i = 0; i < propertyNames.Count(); i++)
            {
                if (i > 0)
                {
                    var orderQuery = source as IOrderedQueryable<T>;
                    source = orderQuery.ThenBy(propertyNames[i], ascending);
                }
                else
                {
                    source = source.OrderBy(propertyNames[i], ascending);
                }
            }
            return source;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string propertyName, bool ascending) where T : class
        {
            var queryableSource = source.AsQueryable();
            var type = typeof(T);

            var property = type.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException("propertyName", "Not Exist");

            var param = Expression.Parameter(type, "p");
            var propertyAccessExpression = Expression.MakeMemberAccess(param, property);
            var orderByExpression = Expression.Lambda(propertyAccessExpression, param);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            var resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, queryableSource.Expression, Expression.Quote(orderByExpression));

            return queryableSource.Provider.CreateQuery<T>(resultExp);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property, bool ascending)
        {
            string Name = ascending ? "OrderBy" : "OrderByDescending";
            return ApplyOrder<T>(source, property, Name);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property, bool ascending)
        {
            string Name = ascending ? "ThenBy" : "ThenByDescending";
            return ApplyOrder<T>(source, property, Name);
        }

        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "a");
            PropertyInfo pi = type.GetProperty(property);
            Expression expr = Expression.Property(arg, pi);
            type = pi.PropertyType;
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            object result = typeof(Queryable).GetMethods().Single(
            a => a.Name == methodName
            && a.IsGenericMethodDefinition
            && a.GetGenericArguments().Length == 2
            && a.GetParameters().Length == 2).MakeGenericMethod(typeof(T), type).Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }

}