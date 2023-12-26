using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Decorators;
using Dotnet.Homeworks.Features.Users.Commands.UpdateUser;
using Dotnet.Homeworks.Features.Users.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : CqrsDecorator<GetUserQuery, Result<GetUserDto>>, IQueryHandler<GetUserQuery, GetUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;

    public GetUserQueryHandler(
        IUserRepository userRepository,
        IEnumerable<IValidator<GetUserQuery>> validators, 
        IPermissionCheck<IClientRequest> permissionCheck, 
        IUserMapper userMapper)
        : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
    }

    public override async Task<Result<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var decoratorsResult = await base.Handle(request, cancellationToken);

        if (decoratorsResult.IsFailure) return decoratorsResult;
        
        var user = await _userRepository.GetUserByGuidAsync(request.Guid, cancellationToken)!;

        if (user is null)
        {
            return $"User with identifier {request.Guid} was not found";
        }

        return _userMapper.Map(user);
    }
}