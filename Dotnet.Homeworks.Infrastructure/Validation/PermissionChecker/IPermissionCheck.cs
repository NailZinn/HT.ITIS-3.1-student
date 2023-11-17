using Dotnet.Homeworks.Infrastructure.Utils;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public interface IPermissionCheck<TRequest>
{ 
    Task<PermissionResult> CheckPermissionAsync(TRequest request);
}