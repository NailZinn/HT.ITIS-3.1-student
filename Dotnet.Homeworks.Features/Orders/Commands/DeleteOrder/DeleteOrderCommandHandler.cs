using System.Security.Claims;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderByGuidCommand>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> Handle(DeleteOrderByGuidCommand request, CancellationToken cancellationToken)
    {
        await _orderRepository.DeleteOrderByGuidAsync(request.OrderId, cancellationToken);

        return true;
    }
}