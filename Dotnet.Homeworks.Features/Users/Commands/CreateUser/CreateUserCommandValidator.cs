using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;
    
    public CreateUserCommandValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.Email)
            .MustAsync(CheckExistingEmail)
            .NotEmpty().WithMessage("Email shouldn't be empty")
            .NotNull().WithMessage("Email shouldn't be null")
            .Matches(@"\w+@\w+\.\w+").WithMessage("Email doesn't match the pattern");
    }

    private async Task<bool> CheckExistingEmail(string email, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetUsersAsync(cancellationToken);
        return !users.Any(x => x.Email == email);
    }
}