using System.Linq.Expressions;

namespace Dotnet.Homeworks.DataAccess.Specs.Infrastructure;

public class ParameterRebinder : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

    private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
    {
        _map = map;
    }

    public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression expression)
    {
        return new ParameterRebinder(map).Visit(expression);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_map.TryGetValue(node, out var replacement))
        {
            node = replacement;
        }
        
        return base.VisitParameter(node);
    }
}