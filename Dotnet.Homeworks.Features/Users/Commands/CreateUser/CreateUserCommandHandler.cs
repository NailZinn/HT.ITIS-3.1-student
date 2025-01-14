﻿using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Decorators;
using Dotnet.Homeworks.Features.Users.Mapping;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : CqrsDecorator<CreateUserCommand, Result<CreateUserDto>>, ICommandHandler<CreateUserCommand, CreateUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserMapper _userMapper;
    
    public CreateUserCommandHandler(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork,
        IEnumerable<IValidator<CreateUserCommand>> validators, 
        IPermissionCheck<IClientRequest> permissionCheck, 
        IUserMapper userMapper)
        : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userMapper = userMapper;
    }

    public override async Task<Result<CreateUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {        
        var decoratorsResult = await base.Handle(request, cancellationToken);

        if (decoratorsResult.IsFailure) return decoratorsResult;

        var user = _userMapper.Map(request);
        var guid = await _userRepository.InsertUserAsync(user, cancellationToken);

        if (guid == Guid.Empty)
        {
            return false;
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateUserDto(guid);
    }
}