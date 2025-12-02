using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Domain.Entities;
using RSVP.Domain.Enums;
using RSVP.Infrastructure.Persistence;

namespace RSVP.Infrastructure.Service;


public class SentNotification
{
    private readonly RsvpDbContext _context;
    public SentNotification(RsvpDbContext context)
    {
        _context = context;
    }

    public async Task SentNotificationToUser()
    {
        //I WANNA SENT NOTIFICATION TO ALL ATTENDIES WHO ARE ATTENDING THE EVENT IF THE EVENT IS TOMOOROW
        var tomorrowStart = DateTime.UtcNow.AddDays(1).Date;
        var tomorrowEnd = tomorrowStart.AddDays(1); 

        var targetAttendies = await _context.Events
                                            .AsNoTracking()
                                            .Where(e => e.Date >= tomorrowStart && e.Date < tomorrowEnd && e.Status == EventStatus.Active)
                                            .SelectMany(e => e.Attendies
                                                            .Where(a => a.Status == AttendiesStatus.Attending && a.Role != AttendiesRole.Organizer && a.UserId != null)
                                                            .Select(a => new 
                                                            { 
                                                                UserId = a.UserId!.Value, 
                                                                EventName = e.Name, 
                                                                EventDate = e.Date,
                                                                EventId = e.Id
                                                            }))
                                            .ToListAsync();

        List<Notification> notifications = new List<Notification>();
        foreach (var attendie in targetAttendies)
        {

                    Notification notification = new (
                    
                        userId: attendie.UserId,
                        description: $"Reminder: The event '{attendie.EventName}' is scheduled for tomorrow ({attendie.EventDate.ToShortDateString()}). Don't forget to attend!",
                        route:$"invitedevents/{attendie.EventId}"
                    );
                    notifications.Add(notification);
    
        }

        await _context.Notifications.AddRangeAsync(notifications);
        await _context.SaveChangesAsync();
    }
}
