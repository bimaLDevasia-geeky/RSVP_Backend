using MediatR;

namespace RSVP.Application.Features.Event.Commands.CreateEvent;

public record class CreateEventCommand:IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;

    public DateTime Date { get; init; }
    public string Venue { get; init; } = null!;

    public TimeOnly Time { get; init; }
}
