using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrders;

public record GetOrdersQuery() : IQuery<GetOrdersDto>, IHasAuthorizationCheck;