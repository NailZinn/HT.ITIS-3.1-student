using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders.Shared;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Behaviors;

public class OrderValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IHasOrderValidation
{
    private readonly IOrderRepository _orderRepository;
    private readonly HttpContext _httpContext;

    public OrderValidationBehavior(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
    {
        _orderRepository = orderRepository;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByGuidAsync(request.OrderId, cancellationToken);
        var claim = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = claim is not null ? Guid.Parse(claim) : Guid.Empty;
        
        if (order is null)
        {
            return $"Order with id {request.OrderId} was not found" as dynamic;
        }
        
        if (userId != order.OrdererId)
        {
            return "You have no access" as dynamic;
        }

        return await next();
    }
}