using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Mapster;

namespace Dotnet.Homeworks.Features.Products.Mapping;

[Mapper]
public interface IProductMapper
{
    GetProductsDto Map(IEnumerable<Product> product);

    Product Map(UpdateProductCommand command);

    Product Map(InsertProductCommand command);
}