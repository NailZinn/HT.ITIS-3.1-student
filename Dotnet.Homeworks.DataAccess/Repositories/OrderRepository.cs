using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using MongoDB.Driver;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orderCollection;

    public OrderRepository(IMongoCollection<Order> orderCollection)
    {
        _orderCollection = orderCollection;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersFromUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _orderCollection
            .Find(o => o.OrdererId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await _orderCollection
            .Find(o => o.Id == orderId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        await _orderCollection
            .DeleteOneAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _orderCollection
            .ReplaceOneAsync(o => o.Id == order.Id, order, cancellationToken: cancellationToken);
    }

    public async Task<Guid> InsertOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _orderCollection.InsertOneAsync(order, new InsertOneOptions(), cancellationToken);

        return order.Id;
    }
}