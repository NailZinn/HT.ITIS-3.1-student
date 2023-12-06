using Dotnet.Homeworks.Features.Orders.Shared;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(Guid OrderId, IEnumerable<Guid> ProductIds) : 
    ICommand, 
    IHasAuthorizationCheck,
    IHasProductIdsValidation,
    IHasOrderValidation;