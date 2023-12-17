using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Mapster;

namespace Dotnet.Homeworks.Features.UserManagement.Mapping;

[Mapper]
public interface IUserManagementMapper
{
    GetAllUsersDto Map(IQueryable<User> users);
}