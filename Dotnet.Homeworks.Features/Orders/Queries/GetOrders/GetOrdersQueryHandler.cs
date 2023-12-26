using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders.Mapping;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, GetOrdersDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderMapper _orderMapper;
    private readonly HttpContext _httpContext;

    public GetOrdersQueryHandler(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor, IOrderMapper orderMapper)
    {
        _orderRepository = orderRepository;
        _orderMapper = orderMapper;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<Result<GetOrdersDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var orders = await _orderRepository.GetAllOrdersFromUserAsync(userId, cancellationToken);

        return _orderMapper.Map(orders);
    }
}