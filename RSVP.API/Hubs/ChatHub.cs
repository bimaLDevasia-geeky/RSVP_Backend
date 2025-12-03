using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RSVP.Application.Dtos;
using RSVP.Application.Features.Chat.Command;
using RSVP.Application.Features.Chat.Queries;

namespace RSVP.API.Hubs;

[Authorize]
public class ChatHub:Hub
{
    private readonly IMediator _mediator;
    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }
     public async Task JoinEventChat(int eventId)
    {
        
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Event_{eventId}");
        
        // Get chat history using CQRS query
        GetMessageQuery query = new GetMessageQuery { EventId = eventId };
        List<ChatMessageDTO> messages = await _mediator.Send(query);



        await Clients.Caller.SendAsync("ReceiveChatHistory", messages);
    }
    public async Task LeaveEventChat(int eventId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Event_{eventId}");
    }
    public async Task SendMessage(int eventId, string message)
    {
        

        AddMessageCommand command = new AddMessageCommand
        {
            EventId = eventId,
            Message = message
        };
        ChatMessageDTO chatMessage = await _mediator.Send(command);
       
        await Clients.Group($"Event_{eventId}").SendAsync("ReceiveMessage", chatMessage);
    }
    // public override async Task OnConnectedAsync()
    // {
    //     await base.OnConnectedAsync();
    // }

    // public override async Task OnDisconnectedAsync(Exception exception)
    // {
    //     await base.OnDisconnectedAsync(exception);
    // }
}
