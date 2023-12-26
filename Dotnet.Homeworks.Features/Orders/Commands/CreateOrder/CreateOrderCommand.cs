using Dotnet.Homeworks.Features.Orders.Shared;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(IEnumerable<Guid> ProductIds) : 
    ICommand<CreateOrderDto>, 
    IHasAuthorizationCheck,
    IHasProductIdsValidation;