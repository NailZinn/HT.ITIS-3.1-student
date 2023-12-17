using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductMapper _productMapper;

    public UpdateProductCommandHandler(IProductRepository productRepository, IProductMapper productMapper)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productToUpdate = _productMapper.Map(request);
        
        await _productRepository.UpdateProductAsync(productToUpdate, cancellationToken);

        return new Result(true);
    }
}