using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Queries.GetOrder;

public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, GetOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly HttpContext _httpContext;

    public GetOrderQueryHandler(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
    {
        _orderRepository = orderRepository;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<Result<GetOrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByGuidAsync(request.Id, cancellationToken);
        var claim = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = claim is not null ? Guid.Parse(claim) : Guid.Empty;

        if (order is null)
        {
            return $"Order with id {request.Id} was not found";
        }

        if (userId != order.OrdererId)
        {
            return "You have no access";
        }

        return new GetOrderDto(order.Id, order.ProductsIds);
    }
}