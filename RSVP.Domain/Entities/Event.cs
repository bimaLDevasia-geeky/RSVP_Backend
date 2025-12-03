using System;
using System.Reflection.Metadata;
using RSVP.Domain.Enums;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace RSVP.Domain.Entities;

public class Event
{
    public int Id { get; private set; }
    public string Name { get; private set; }=null!;
    public string Description { get; private set; }=null!;
    
    public int CreatedBy { get; private set; }
    public DateTime Date { get; private set; }
    public string Venue { get; private set; }=null!;

    public TimeOnly Time { get; private set; }

    public string? InviteCode { get; private set; }
    public Boolean IsPublic { get; private set; }

    public EventStatus Status { get; private set; } 

    public ICollection<Attendie> Attendies { get; private set; } = new List<Attendie>();
    public ICollection<Media> Media { get; private set; } = new List<Media>();
    public ICollection<Request> Requests { get; private set; } = new List<Request>();
    public ICollection<ChatMessage> Chats { get; set; } = new List<ChatMessage>();
    public User Creator { get; private set; }=null!;




    public Event(string name, string description, int createdBy, DateTime date, string venue, TimeOnly time,  Boolean isPublic)
    {
        Name = name;
        Description = description;
        CreatedBy = createdBy;
        Date = date;
        Venue = venue;
        Time = time;
        InviteCode = GenerateRandomCode(8);
        IsPublic = isPublic;
        Status = EventStatus.Active;
    }

    public void UpdateEvent(string? name, string? description, DateTime? date, string? venue, TimeOnly? time, Boolean? isPublic, EventStatus? status)
    {
        if(name is not null)
            Name = name;
        if(description is not null)
            Description = description;
        
        if(date.HasValue)
            Date = date.Value;
        if(venue is not null)
            Venue = venue;
        if(time.HasValue)
            Time = time.Value;
        if(isPublic.HasValue)
            IsPublic = isPublic.Value;
        if(status.HasValue)
            Status = status.Value;
    }

    public void GenerateNewInviteCode()
    {
        InviteCode = GenerateRandomCode(8); 
    }

    private static string GenerateRandomCode(int length)
    {
        const string chars = "23456789BCDFGHJKMNPQRTVWXYZ"; 
        var data = new byte[length];

        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }

        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            var rnd = data[i] % chars.Length;
            result[i] = chars[rnd];
        }

        return new string(result);
    }
    
}
