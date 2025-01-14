﻿using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;

namespace Dotnet.Homeworks.Features.Behaviors;

public class PermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IAdminRequest
{
    private readonly IPermissionCheck<IAdminRequest> _permissionCheck;

    public PermissionBehavior(IPermissionCheck<IAdminRequest> permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var permissionResult = await _permissionCheck.CheckPermissionAsync(request);

        if (permissionResult.IsFailure)
        {
            return permissionResult.Error! as dynamic;
        }
        
        return await next();
    }
}