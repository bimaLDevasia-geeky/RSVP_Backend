using System;
using MediatR;
using RSVP.Application.Dtos;

namespace RSVP.Application.Features.User.Query;

public class GetUsersQuery:IRequest<List<UserDto>>
{

}
