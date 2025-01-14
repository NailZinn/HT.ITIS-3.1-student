using System.Linq;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.UserManagement.Mapping;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

namespace Dotnet.Homeworks.Features.UserManagement.Mapping
{
    public partial class UserManagementMapper : IUserManagementMapper
    {
        public GetAllUsersDto Map(IQueryable<User> p1)
        {
            return p1 == null ? null : new GetAllUsersDto(p1 == null ? null : p1.Select<User, GetUserDto>(funcMain1));
        }
        
        private GetUserDto funcMain1(User p2)
        {
            return p2 == null ? null : new GetUserDto(p2.Id, p2.Name, p2.Email);
        }
    }
}