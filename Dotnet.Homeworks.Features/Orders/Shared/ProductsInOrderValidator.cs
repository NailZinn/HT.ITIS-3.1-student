using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Orders.Commands.CreateOrder;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Orders.Shared;

public class ProductsInOrderValidator : AbstractValidator<IHasProductIdsValidation>
{
    private readonly IProductRepository _productRepository;
    
    public ProductsInOrderValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
        
        RuleFor(x => x.ProductIds)
            .NotEmpty().WithMessage("Products shouldn't be empty")
            .MustAsync(CheckProductsAsync).WithMessage("All products should exist");
    }

    private async Task<bool> CheckProductsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken)
    {
        var existingProductIds = (await _productRepository.GetAllProductsAsync(cancellationToken))
            .Select(p => p.Id)
            .ToHashSet();

        return productIds.All(id => existingProductIds.Contains(id));
    }
}