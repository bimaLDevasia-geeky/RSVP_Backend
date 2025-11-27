// using System;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using RSVP.Application.Dtos;
// using RSVP.Application.Interfaces;

// namespace RSVP.Application.Features.Request.Queries.GetRequestsByUser;

// public class GetRequestsByUserQueryHandler:IRequestHandler<GetRequestsByUserQuery, List<RequestDto>>
// {

//     private readonly IRsvpDbContext _context;
//     private readonly ICurrentUser _currentUser;
//     private readonly IRepository<Domain.Entities.User> _userRepository;
//     private readonly IRepository<Domain.Entities.Event> _eventRepository;

//     public GetRequestsByUserQueryHandler(IRsvpDbContext context, ICurrentUser currentUser, IRepository<Domain.Entities.User> userRepository, IRepository<Domain.Entities.Event> eventRepository)
//     {
//         _context = context;
//         _currentUser = currentUser;
//         _userRepository = userRepository;
//         _eventRepository = eventRepository;
//     }

//     public async Task<List<RequestDto>> Handle(GetRequestsByUserQuery request, CancellationToken cancellationToken)
//     {
//         string userName = (await _userRepository.GetByIdAsync(_currentUser.UserId, cancellationToken))?.Name ?? "Unknown User";
//         string eventName = (await _eventRepository.GetByIdAsync(request.Id, cancellationToken))?.Name ?? "Unknown Event";
//         var requests =  _context.Requests
//             .Where(r => r.UserId == _currentUser.UserId)
//             .Select(r => new RequestDto(
//                 r.Id,
//                 r.EventId,
//                 r.UserId,
//                 r.RequestedAt,
//                 r.Status,
//                 userName,
//                 eventName
//             ))
//             .ToListAsync(cancellationToken);

//         return requests;
        
        
//     }
// }
