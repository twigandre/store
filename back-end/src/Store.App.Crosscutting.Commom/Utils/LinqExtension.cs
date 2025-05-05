using System.Linq.Expressions;

namespace Store.App.Crosscutting.Commom.Utils
{
    public static class LinqExtension
    {
        private static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "x");

            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda(body, param);
        }

        public static IQueryable<TSource> OrderBy<TSource>(IQueryable<TSource> query, string key, bool reverse = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return query;
            }

            var lambda = (dynamic)CreateExpression(typeof(TSource), key);

            return reverse
                ? Queryable.OrderByDescending(query, lambda)
                : Queryable.OrderBy(query, lambda);
        }

        public static IOrderedQueryable<TSource> ThenBy<TSource>(IQueryable<TSource> query, string key, bool reverse = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return (IOrderedQueryable<TSource>)query;
            }

            var lambda = (dynamic)CreateExpression(typeof(TSource), key);

            return reverse
                ? Queryable.ThenByDescending((IOrderedQueryable<TSource>)query, lambda)
                : Queryable.ThenBy((IOrderedQueryable<TSource>)query, lambda);
        }
    }
}
