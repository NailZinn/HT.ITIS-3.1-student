using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

public record GetAllUsersQuery : IQuery<GetAllUsersDto>, IAdminRequest;