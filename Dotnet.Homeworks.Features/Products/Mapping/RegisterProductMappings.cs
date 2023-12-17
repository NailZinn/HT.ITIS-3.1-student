using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;
using Mapster;

namespace Dotnet.Homeworks.Features.Products.Mapping;

public class RegisterProductMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, GetProductDto>()
            .Map(to => to.Guid, from => from.Id)
            .RequireDestinationMemberSource(true);

        config.NewConfig<IEnumerable<Product>, GetProductsDto>()
            .Map(to => to.Products, from => from)
            .RequireDestinationMemberSource(true);

        config.NewConfig<UpdateProductCommand, Product>()
            .Map(to => to.Id, from => from.Guid)
            .RequireDestinationMemberSource(true);

        config.NewConfig<InsertProductCommand, Product>()
            .Ignore(to => to.Id)
            .RequireDestinationMemberSource(true);
    }
}