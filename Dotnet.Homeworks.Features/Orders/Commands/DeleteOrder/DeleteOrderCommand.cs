using Dotnet.Homeworks.Features.Orders.Shared;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;

public record DeleteOrderByGuidCommand(Guid OrderId) : 
    ICommand, 
    IHasAuthorizationCheck,
    IHasOrderValidation;