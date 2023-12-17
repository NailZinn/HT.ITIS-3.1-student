using System.Linq.Expressions;

namespace Dotnet.Homeworks.DataAccess.Specs.Infrastructure;

public class Specification<T> : IQueryableFilter<T>
    where T : class
{
    private readonly Expression<Func<T, bool>> _expression;

    public Specification(Expression<Func<T, bool>> expression)
    {
        _expression = expression;
    }

    public static implicit operator Expression<Func<T, bool>>(Specification<T> spec)
    {
        return spec._expression;
    }
    
    public static Specification<T> operator |(Specification<T> spec1, Specification<T> spec2)
    {
        return new Specification<T>(spec1._expression.Or(spec2._expression));
    }
    
    public static Specification<T> operator &(Specification<T> spec1, Specification<T> spec2)
    {
        return new Specification<T>(spec1._expression.And(spec2._expression));
    }

    public static bool operator false(Specification<T> _) => false;

    public static bool operator true(Specification<T> _) => false;

    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.Where(_expression);
    }
}