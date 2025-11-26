using System;
using RSVP.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace RSVP.Application.Service;

public class CurrentUser:ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor; 
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            throw new Exception("User ID not found in the current context.");
        }
    }

    public string Role
    {
        get
        {
            var role = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            if (role != null)
            {
                return role;
            }
            throw new Exception("User Role not found in the current context.");
        }
    }
}
