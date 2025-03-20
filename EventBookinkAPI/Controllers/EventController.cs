using System.Text.Json;
using EventBookinkAPI.Data;
using EventBookinkAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventBookinkAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly PeopleSyncDbContext context;

    public EventController(PeopleSyncDbContext context)
    {
        this.context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await context.Events.ToListAsync();
        return Ok(events);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateEvent(Event ev)
    {
        var appMetadataClaim = User.Claims.FirstOrDefault(c => c.Type == "app_metadata");
        // var userRole = appMetadataClaim?.Value;
        // Console.WriteLine($"User Role: {userRole}");
        Console.WriteLine($"AppMetadata: {appMetadataClaim?.Value}");

        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
        }

        if (appMetadataClaim == null)
        {
            return Forbid();
        }

        var appMetadata = JsonSerializer.Deserialize<Dictionary<string, object>>(appMetadataClaim.Value);
        var userRole = appMetadata.TryGetValue("role", out var roleValue) ? roleValue?.ToString() : null;

        if (userRole != "organizer")
        {
            return Forbid();
        }

        context.Events.Add(ev);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvents), new { id = ev.id }, ev);
    }
}