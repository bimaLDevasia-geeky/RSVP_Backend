using System;
using MediatR;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.User.Query.GetUserById;

public class GetUserByIdQueryHandler:IRequestHandler<GetUserByIdQuery,UserDto>
{

    private readonly IRepository<Domain.Entities.User> _userRepository;

    public GetUserByIdQueryHandler(IRepository<Domain.Entities.User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found");
        }

        UserDto userDto = new UserDto(
            user.Id,
            user.Name,
            user.Email
        );

        return userDto;
    }
}
