using System;
using MediatR;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.User.Command.UpdateUser;

public class UpdateUserCommandHandler:IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IRepository<Domain.Entities.User> _userRepository;

    private readonly ICurrentUser _currentUser;
    
    public UpdateUserCommandHandler(IRepository<Domain.Entities.User> userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);



        if (user is null)
        {
            throw new KeyNotFoundException("User not found");
        }

        user.UpdateUser(name: request.Name, email: request.Email);


        if (request.Status.HasValue )
        {
            if (_currentUser.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can change user status.");
            }

            user.ChangeStatus(request.Status.Value);
        }

        if (request.Role.HasValue)
        {
            if (_currentUser.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can change user roles.");
            }
            user.ChangeRole(request.Role.Value);
        }
        
       await _userRepository.SaveChangesAsync(cancellationToken);
       return true;
    }
}
