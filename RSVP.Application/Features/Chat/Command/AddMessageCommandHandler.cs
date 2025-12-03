using System;
using MediatR;
using RSVP.Application.Dtos;
using RSVP.Application.Interfaces;
using appDomain=RSVP.Domain.Entities;

namespace RSVP.Application.Features.Chat.Command;

public class AddMessageCommandHandler:IRequestHandler<AddMessageCommand,ChatMessageDTO>
{
    private readonly IRepository<appDomain.ChatMessage> _chatMessageRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<appDomain.User> _userRepository;
    public AddMessageCommandHandler(IRepository<appDomain.ChatMessage> chatMessageRepository, ICurrentUser currentUser, IRepository<appDomain.User> userRepository)
    {
        _chatMessageRepository = chatMessageRepository;
        _currentUser = currentUser;
        _userRepository = userRepository;
    }
    public async Task<ChatMessageDTO> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        int userId = _currentUser.UserId;
        

        appDomain.User? user =  await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }
        appDomain.ChatMessage newMessage = new(
            eventId: request.EventId,
            userId: userId,
            userName: user.Name,
            message: request.Message
        );
        await _chatMessageRepository.AddAsync(newMessage, cancellationToken);
        await _chatMessageRepository.SaveChangesAsync(cancellationToken);

        ChatMessageDTO chatMessageDTO = new ChatMessageDTO
        {
            Id = newMessage.Id,
            EventId = newMessage.EventId,
            UserId = newMessage.UserId,
            UserName = user.Name,
            Message = newMessage.Message
        };

        return chatMessageDTO;
    }
}
