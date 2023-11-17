using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IQueryable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = _dbContext.Users.AsQueryable();
        return Task.FromResult(users);
    }

    public Task<User?> GetUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == guid);
        return Task.FromResult(user);
    }

    public Task DeleteUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        _dbContext.Entry(new User { Id = guid }).State = EntityState.Deleted;

        return Task.CompletedTask;
    }

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Update(user);
        
        return Task.CompletedTask;
    }

    public Task<Guid> InsertUserAsync(User user, CancellationToken cancellationToken)
    {
        var emailAlreadyExists = _dbContext.Users.Any(x => x.Email == user.Email);

        if (emailAlreadyExists)
        {
            return Task.FromResult(Guid.Empty);
        }
        
        _dbContext.Users.Add(user);

        return Task.FromResult(user.Id);
    }
}