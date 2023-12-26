using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.UserManagement.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, GetAllUsersDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManagementMapper _userManagementMapper;

    public GetAllUsersQueryHandler(IUserRepository userRepository, IUserManagementMapper userManagementMapper)
    {
        _userRepository = userRepository;
        _userManagementMapper = userManagementMapper;
    }

    public async Task<Result<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetUsersAsync(cancellationToken);

        return _userManagementMapper.Map(users);
    }
}