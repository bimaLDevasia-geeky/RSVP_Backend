using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;

namespace RSVP.Application.Features.Request.Commands.CreateRequest;

public class CreateRequestCommandHandler:IRequestHandler<CreateRequestCommand,int>
{

        private readonly IRepository<Domain.Entities.Request> _requestRepository;
        private readonly ICurrentUser _currentUser;

        private readonly IRsvpDbContext _context;

        public CreateRequestCommandHandler(IRepository<Domain.Entities.Request> requestRepository, ICurrentUser currentUser, IRsvpDbContext context)
        {
            _requestRepository = requestRepository;
            _currentUser = currentUser;
            _context = context;
        }

    public async Task<int> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
    {

        var requestExists = await _context.Requests
            .AnyAsync(r => r.EventId == request.EventId && r.UserId == _currentUser.UserId, cancellationToken);

        if (requestExists)
        {
            throw new InvalidOperationException("Request for this event by the current user already exists.");
        }

       var Request = new Domain.Entities.Request(
            eventId: request.EventId,
            userId: _currentUser.UserId
        );



        await _requestRepository.AddAsync(Request, cancellationToken);
        await _requestRepository.SaveChangesAsync(cancellationToken);

        return Request.Id;
       
    }
}
