using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Users;

public class ClientPermissionCheck : IPermissionCheck<IClientRequest>
{
    private readonly HttpContext _httpContext;

    public ClientPermissionCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public Task<PermissionResult> CheckPermissionAsync(IClientRequest request)
    {
        var isClient = _httpContext.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && c.Value == request.Guid.ToString());

        return Task.FromResult(new PermissionResult(isClient, "Access denied"));
    }
}