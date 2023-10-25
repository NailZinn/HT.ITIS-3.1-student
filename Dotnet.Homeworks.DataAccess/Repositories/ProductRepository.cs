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
        return _dbContext.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        return _dbContext.Products
            .Where(p => p.Id == product.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Name, product.Name), cancellationToken);
    }

    public Task<Guid> InsertProductAsync(Product product, CancellationToken cancellationToken)
    {
        _dbContext.Products.Add(product);

        return Task.FromResult(product.Id);
    }
}