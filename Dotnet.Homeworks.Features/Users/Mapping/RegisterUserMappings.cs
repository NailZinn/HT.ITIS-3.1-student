using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Mapster;

namespace Dotnet.Homeworks.Features.Users.Mapping;

public class RegisterUserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, GetUserDto>()
            .Map(to => to.Guid, from => from.Id)
            .RequireDestinationMemberSource(true);

        config.NewConfig<CreateUserCommand, User>()
            .Ignore(to => to.Id)
            .RequireDestinationMemberSource(true);
    }
}