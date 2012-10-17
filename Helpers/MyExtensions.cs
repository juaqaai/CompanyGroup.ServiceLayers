using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Helpers
{
    public static class MyExtensions
    {
        //public static IEnumerable<TEntity> DynamicOrderBy<TEntity>(this IEnumerable<TEntity> source, string orderByProperty, bool desc) where TEntity : class
        //{
        //    string command = desc ? "OrderByDescending" : "OrderBy";

        //    var type = typeof(TEntity);

        //    var property = type.GetProperty(orderByProperty);

        //    var parameter = System.Linq.Expressions.Expression.Parameter(type, "p");

        //    var propertyAccess = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, property);

        //    var orderByExpression = System.Linq.Expressions.Expression.Lambda(propertyAccess, parameter);

        //    var resultExpression = System.Linq.Expressions.Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },

        //                           source.Expression, System.Linq.Expressions.Expression.Quote(orderByExpression));

        //    return source.Provider.CreateQuery<TEntity>(resultExpression);

        //}

        /// <summary>
        /// extension method 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }
        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            System.Linq.Expressions.ParameterExpression arg = System.Linq.Expressions.Expression.Parameter(type, "x");
            System.Linq.Expressions.Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                System.Reflection.PropertyInfo pi = type.GetProperty(prop);
                expr = System.Linq.Expressions.Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            System.Linq.Expressions.LambdaExpression lambda = System.Linq.Expressions.Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

    }
}
