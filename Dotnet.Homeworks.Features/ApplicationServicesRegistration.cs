using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Behaviors;
using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Features;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediator(AssemblyReference.Assembly);
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(PermissionBehavior<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationCheckBehavior<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(OrderValidationBehavior<,>));
        
        serviceCollection.AddPermissionChecks(AssemblyReference.Assembly);

        serviceCollection.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        
        serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
        serviceCollection.AddTransient<IProductRepository, ProductRepository>();
        serviceCollection.AddTransient<IUserRepository, UserRepository>();

        return serviceCollection;
    }
}