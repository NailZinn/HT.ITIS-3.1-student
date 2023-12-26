using System.Collections.Generic;
using System.Linq;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
using Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;
using Dotnet.Homeworks.Features.Products.Mapping;
using Dotnet.Homeworks.Features.Products.Queries.GetProducts;

namespace Dotnet.Homeworks.Features.Products.Mapping
{
    public partial class ProductMapper : IProductMapper
    {
        public GetProductsDto Map(IEnumerable<Product> p1)
        {
            return p1 == null ? null : new GetProductsDto(p1 == null ? null : p1.Select<Product, GetProductDto>(funcMain1));
        }
        public Product Map(UpdateProductCommand p3)
        {
            return p3 == null ? null : new Product()
            {
                Name = p3.Name,
                Id = p3.Guid
            };
        }
        public Product Map(InsertProductCommand p4)
        {
            return p4 == null ? null : new Product() {Name = p4.Name};
        }
        
        private GetProductDto funcMain1(Product p2)
        {
            return p2 == null ? null : new GetProductDto(p2.Id, p2.Name);
        }
    }
}