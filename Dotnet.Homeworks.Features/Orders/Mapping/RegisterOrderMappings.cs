using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Mapster;

namespace Dotnet.Homeworks.Features.Orders.Mapping;

public class RegisterOrderMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, GetOrderDto>()
            .RequireDestinationMemberSource(true);

        config.NewConfig<IEnumerable<Order>, GetOrdersDto>()
            .Map(to => to.Orders, from => from)
            .RequireDestinationMemberSource(true);
    }
}