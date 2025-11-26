using System;
using System.Reflection.Metadata;
using RSVP.Domain.Enums;
using System.Security.Cryptography;

namespace RSVP.Domain.Entities;

public class Events
{
    public int Id { get; private set; }
    public string Name { get; private set; }=null!;
    public string Description { get; private set; }=null!;
    public int ImageId { get; private set; }
    public int CreatedBy { get; private set; }
    public DateTime Date { get; private set; }
    public string Venue { get; private set; }=null!;

    public TimeOnly Time { get; private set; }

    public string? InviteCode { get; private set; }
    public Boolean IsPublic { get; private set; }

    public EventStatus Status { get; private set; } 

    public Events(string name, string description, int imageId, int createdBy, DateTime date, string venue, TimeOnly time,  Boolean isPublic)
    {
        Name = name;
        Description = description;
        ImageId = imageId;
        CreatedBy = createdBy;
        Date = date;
        Venue = venue;
        Time = time;
        InviteCode = GenerateRandomCode(8);
        IsPublic = isPublic;
        Status = EventStatus.Active;
    }

    public void UpdateEvent(string? name, string? description, int? imageId, DateTime? date, string? venue, TimeOnly? time, Boolean? isPublic, EventStatus? status)
    {
        if(name is not null)
            Name = name;
        if(description is not null)
            Description = description;
        if(imageId.HasValue)
            ImageId = imageId.Value;
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
