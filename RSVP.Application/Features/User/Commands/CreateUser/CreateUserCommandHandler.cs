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
        
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new Domain.Entities.User(
            name: request.Name.Trim(),
            email: request.Email.Trim().ToLower(),
            hashedPassword: hashedPassword
        );

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

}
