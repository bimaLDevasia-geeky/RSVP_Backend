using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using appDomain= RSVP.Domain.Entities;
using RSVP.Domain.Enums;

namespace RSVP.Application.Features.Request.Commands.UpdateRequest;

public class UpdateRequestCommandHandler:IRequestHandler<UpdateRequestCommand,bool>
{
    private readonly IRepository<Domain.Entities.Request> _requestRepository;

    private readonly IRepository<appDomain.Attendie> _attendieRepository;
    private readonly IRepository<Domain.Entities.User> _userRepository;
    private readonly IRsvpDbContext _context;


    public UpdateRequestCommandHandler(IRepository<Domain.Entities.Request> requestRepository, IRepository<appDomain.Attendie> attendieRepository,IRepository<Domain.Entities.User> userRepository, IRsvpDbContext context)
    {
        _requestRepository = requestRepository;
        _attendieRepository = attendieRepository;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<bool> Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
    {
        
        var requestToUpdate =  await _requestRepository.GetByIdAsync(request.Id, cancellationToken);


        if (requestToUpdate == null)
        {
            throw new KeyNotFoundException("Request not found.");
        }

        if(request.Status == RequestStatus.Accepted)
        {
            requestToUpdate.UpdateStatus(RequestStatus.Accepted);
            
            string userEmail = (await _userRepository.GetByIdAsync(requestToUpdate.UserId, cancellationToken))?.Email ?? throw new KeyNotFoundException("User not found.");
            
            var existingAttendie = await _context.Attendies
                .AnyAsync(a => a.EventId == requestToUpdate.EventId && a.UserId == requestToUpdate.UserId, cancellationToken);
            if (existingAttendie)
            {
                throw new InvalidOperationException("Attendie already exists for this event and user.");
            }
            var newAttendie = new appDomain.Attendie
            (
                requestToUpdate.EventId,
                requestToUpdate.UserId,
                userEmail
                
            );
            newAttendie.UpdateStatus(AttendiesStatus.Attending);
            
            await _attendieRepository.AddAsync(newAttendie, cancellationToken);
        }
        else if(request.Status == RequestStatus.Rejected)
        {
            requestToUpdate.UpdateStatus(RequestStatus.Rejected);
        }
        else
        {
            throw new InvalidOperationException("Invalid status update.");
        }

    
        await _requestRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
