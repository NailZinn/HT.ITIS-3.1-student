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

namespace Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : CqrsDecorator<CreateOrderCommand, Result<CreateOrderDto>>, ICommandHandler<CreateOrderCommand, CreateOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly HttpContext _httpContext;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository, 
        IHttpContextAccessor httpContextAccessor,
        IEnumerable<IValidator<IHasProductIdsValidation>> validators,
        IPermissionCheck<IClientRequest> permissionCheck, 
        IUserRepository userRepository)
        : base(validators, permissionCheck)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public override async Task<Result<CreateOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var decoratorsResult = await base.Handle(request, cancellationToken);

        if (decoratorsResult.IsFailure) return decoratorsResult;
        
        var userId = Guid.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userRepository.GetUserByGuidAsync(userId, cancellationToken);

        if (user is null)
        {
            return "Could not create order for non-existing user";
        }
        
        var orderToInsert = new Order
        {
            ProductsIds = request.ProductIds,
            OrdererId = userId
        };

        var orderId = await _orderRepository.InsertOrderAsync(orderToInsert, cancellationToken);

        return new CreateOrderDto(orderId);
    }
}