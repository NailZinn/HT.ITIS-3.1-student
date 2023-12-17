using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Mapster;

namespace Dotnet.Homeworks.Features.UserManagement.Mapping;

public class RegisterUserManagementMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, GetUserDto>()
            .Map(to => to.Guid, from => from.Id)
            .RequireDestinationMemberSource(true);
        
        config.NewConfig<IQueryable<User>, GetAllUsersDto>()
            .Map(to => to.Users, from => from)
            .RequireDestinationMemberSource(true);
    }
}