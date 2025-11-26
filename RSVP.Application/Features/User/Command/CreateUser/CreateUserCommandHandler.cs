using System;
using MediatR;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.User.Command.CreateUser;

public class CreateUserCommandHandler:IRequestHandler<CreateUserCommand,int>
{

    private readonly IRepository<Domain.Entities.User> _userRepository;

    public CreateUserCommandHandler(IRepository<Domain.Entities.User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        var user = new Domain.Entities.User(
            name: request.Name.Trim(),
            email: request.Email.Trim().ToLowerInvariant(),
            hashedPassword: request.Password
        );

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user.Id;
    }

}
