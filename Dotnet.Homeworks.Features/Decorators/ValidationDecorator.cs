using Dotnet.Homeworks.Mediator;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Decorators;

public class ValidationDecorator<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    protected ValidationDecorator(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return true as dynamic;
            
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var errors = validationResults
            .Where(x => !x.IsValid)
            .SelectMany(x => x.Errors)
            .Select(x => x.ToString())
            .ToList();
        
        if (errors.Count == 0) return true as dynamic;

        return errors.Aggregate((acc, next) => $"{acc} {next}")! as dynamic;
    }
}