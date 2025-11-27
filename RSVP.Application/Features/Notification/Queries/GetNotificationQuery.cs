using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.Notification.Queries;

public class GetNotificationQuery:IRequest<List<NotificationDto>>
{

}
