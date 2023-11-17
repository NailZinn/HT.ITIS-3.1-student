using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public record CreateUserCommand(string Name, string Email) : ICommand<CreateUserDto>;