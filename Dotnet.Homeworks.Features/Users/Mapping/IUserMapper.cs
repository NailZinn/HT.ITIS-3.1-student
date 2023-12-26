using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Mapster;

namespace Dotnet.Homeworks.Features.Users.Mapping;

[Mapper]
public interface IUserMapper
{
    GetUserDto Map(User user);

    User Map(CreateUserCommand command);
}