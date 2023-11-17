using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Features.UserManagement.Commands.DeleteUserByAdmin;

public record DeleteUserByAdminCommand(Guid Guid) : ICommand, IAdminRequest;