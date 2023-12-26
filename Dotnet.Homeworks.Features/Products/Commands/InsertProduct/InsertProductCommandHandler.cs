using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;

internal sealed class InsertProductCommandHandler : ICommandHandler<InsertProductCommand, InsertProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductMapper _productMapper;
    
    public InsertProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IProductMapper productMapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _productMapper = productMapper;
    }

    public async Task<Result<InsertProductDto>> Handle(InsertProductCommand request, CancellationToken cancellationToken)
    {
        var productToInsert = _productMapper.Map(request);
        var guid = await _productRepository.InsertProductAsync(productToInsert, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new InsertProductDto(guid);
    }
}