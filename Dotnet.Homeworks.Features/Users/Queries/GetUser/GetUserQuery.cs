using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public record GetUserQuery(Guid Guid) : IQuery<GetUserDto>, IClientRequest;