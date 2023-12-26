using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrder;

public record GetOrderQuery(Guid Id) : IQuery<GetOrderDto>;