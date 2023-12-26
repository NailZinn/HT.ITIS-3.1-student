using Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;
using Dotnet.Homeworks.Features.Orders.Commands.DeleteOrder;
using Dotnet.Homeworks.Features.Orders.Commands.UpdateOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrder;
using Dotnet.Homeworks.Features.Orders.Queries.GetOrders;
using Dotnet.Homeworks.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class OrderManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetUserOrdersAsync(CancellationToken cancellationToken)
    {
        var query = new GetOrdersQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error is null ? Unauthorized() : BadRequest(result.Error);
    }

    [HttpGet("order/{id:guid}")]
    public async Task<IActionResult> GetUserOrderAsync(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrderQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }

    [HttpPost("order")]
    public async Task<IActionResult> CreateOrderAsync([FromBody] IEnumerable<Guid> productsIds, CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(productsIds);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.Error is null ? Unauthorized() : BadRequest(result.Error);
    }

    [HttpPut("order/{id:guid}")]
    public async Task<IActionResult> UpdateOrderAsync(Guid id, [FromBody] IEnumerable<Guid> productsIds, CancellationToken cancellationToken)
    {
        var command = new UpdateOrderCommand(id, productsIds);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok()
            : result.Error is null ? Unauthorized() : BadRequest(result.Error);
    }

    [HttpDelete("order/{id:guid}")]
    public async Task<IActionResult> DeleteOrderAsync(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteOrderByGuidCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok()
            : result.Error is null ? Unauthorized() : BadRequest(result.Error);
    }
}