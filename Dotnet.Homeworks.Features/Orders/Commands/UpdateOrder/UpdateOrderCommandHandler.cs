using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Decorators;
using Dotnet.Homeworks.Features.Orders.Shared;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : CqrsDecorator<UpdateOrderCommand, Result>, ICommandHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly HttpContext _httpContext;

    public UpdateOrderCommandHandler(
        IOrderRepository orderRepository, 
        IEnumerable<IValidator<IHasProductIdsValidation>> validators,
        IPermissionCheck<IClientRequest> permissionCheck, 
        IHttpContextAccessor httpContextAccessor)
        : base(validators, permissionCheck)
    {
        _orderRepository = orderRepository;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public override async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var decoratorsResult = await base.Handle(request, cancellationToken);

        if (decoratorsResult.IsFailure) return decoratorsResult;
        
        var userId = Guid.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var orderToUpdate = new Order
        {
            Id = request.OrderId,
            ProductsIds = request.ProductIds,
            OrdererId = userId
        };

        await _orderRepository.UpdateOrderAsync(orderToUpdate, cancellationToken);

        return true;
    }
}