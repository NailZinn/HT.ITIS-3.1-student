using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser;
using Dotnet.Homeworks.Features.Users.Mapping;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;

namespace Dotnet.Homeworks.Features.Users.Mapping
{
    public partial class UserMapper : IUserMapper
    {
        public GetUserDto Map(User p1)
        {
            return p1 == null ? null : new GetUserDto(p1.Id, p1.Name, p1.Email);
        }
        public User Map(CreateUserCommand p2)
        {
            return p2 == null ? null : new User()
            {
                Email = p2.Email,
                Name = p2.Name
            };
        }
    }
}