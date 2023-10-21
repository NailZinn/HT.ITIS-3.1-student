using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Products.ToListAsync(cancellationToken);
    }

    public Task DeleteProductByGuidAsync(Guid id, CancellationToken cancellationToken)
    {
        var productToDelete = new Product
        {
            Id = id
        };
        
        _dbContext.Entry(productToDelete).State = EntityState.Deleted;

        return Task.CompletedTask;
    }

    public Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Entry(product).State = EntityState.Modified;
        
        return Task.CompletedTask;
    }

    public Task<Guid> InsertProductAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Products.Add(product);

        return Task.FromResult(product.Id);
    }
}