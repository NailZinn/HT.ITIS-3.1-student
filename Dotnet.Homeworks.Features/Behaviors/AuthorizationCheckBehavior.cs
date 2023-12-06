using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Behaviors;

public class AuthorizationCheckBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IHasAuthorizationCheck
{
    private readonly HttpContext _httpContext;

    public AuthorizationCheckBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_httpContext.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
        {
            return false as dynamic;
        }
        
        return await next();
    }
}