using System;
using MediatR;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.User.Query;

public class GetUsersQueryHandler:IRequestHandler<GetUsersQuery,List<UserDto>>
{

    private readonly IRepository<Domain.Entities.User> _userRepository; 

    public GetUsersQueryHandler(IRepository<Domain.Entities.User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

       List<UserDto> userDtos = users.Select(user => new UserDto(
            user.Id,
            user.Name,
            user.Email
        )).ToList();

         return userDtos;
    }
}
