using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Decorators;

public class PermissionDecorator<TRequest, TResponse> : ValidationDecorator<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IPermissionCheck<IClientRequest> _permissionCheck;
    
    protected PermissionDecorator(IEnumerable<IValidator<TRequest>> validators, IPermissionCheck<IClientRequest> permissionCheck) 
        : base(validators)
    {
        _permissionCheck = permissionCheck;
    }

    public override async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        if (request is not IClientRequest clientRequest)
        {
            return await base.Handle(request, cancellationToken);
        }
        
        var permissionResult = await _permissionCheck.CheckPermissionAsync(clientRequest);

        if (permissionResult.IsFailure)
        {
            return permissionResult.Error! as dynamic;
        }
        
        return await base.Handle(request, cancellationToken);
    }
}