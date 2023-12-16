using Dotnet.Homeworks.Features.Products.Commands.DeleteProduct;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.Homeworks.MainProject.Controllers;

[ApiController]
public class ProductManagementController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
    {
        var query = new GetProductsQuery();
        var productsResult = await _mediator.Send(query, cancellationToken);

        return productsResult.IsSuccess
            ? Ok(productsResult.Value)
            : BadRequest(productsResult.Error);
    }

    [HttpPost("product")]
    public async Task<IActionResult> InsertProduct(string name, CancellationToken cancellationToken)
    {
        var command = new InsertProductCommand(name);
        var insertedProductResult = await _mediator.Send(command, cancellationToken);

        return insertedProductResult.IsSuccess
            ? Ok(insertedProductResult.Value)
            : BadRequest(insertedProductResult.Error);
    }

    [HttpDelete("product")]
    public async Task<IActionResult> DeleteProduct(Guid guid, CancellationToken cancellationToken)
    {
        var command = new DeleteProductByGuidCommand(guid);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPut("product")]
    public async Task<IActionResult> UpdateProduct(Guid guid, string name, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(guid, name);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}