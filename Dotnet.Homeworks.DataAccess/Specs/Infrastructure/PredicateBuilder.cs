using System.Linq.Expressions;

namespace Dotnet.Homeworks.DataAccess.Specs.Infrastructure;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return Compose(first, second, Expression.AndAlso);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return Compose(first, second, Expression.OrElse);
    }

    private static Expression<Func<T, bool>> Compose<T>(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second, Func<Expression, Expression, Expression> merge)
    {
        var map = first.Parameters
            .Zip(second.Parameters, (p1, p2) => new { p1, p2 })
            .ToDictionary(x => x.p2, y => y.p1);
        
        var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
        
        return Expression.Lambda<Func<T, bool>>(merge(first.Body, secondBody), first.Parameters);
    }
}