using System.Linq.Expressions;

namespace Promomash.EntityFramework.Common;

public static class ExpressionMappingExtensions<TSource, TDest> where TDest : class
{
    public static IQueryable<TDest> MapIncludeProperties(IQueryable<TDest> query,
        Expression<Func<TSource, object>>? includePropertiesExpression)
    {
        if (includePropertiesExpression.Body is NewExpression body)
        {
            return body.Arguments.OfType<MemberExpression>().Select(memberExpression => memberExpression.Member.Name)
                .Aggregate(query, (current, navigationPropertyName) => current.Include(navigationPropertyName));
        }

        return query;
    }
}