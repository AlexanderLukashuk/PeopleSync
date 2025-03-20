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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvents(int id)
    {
        var ev = await context.Events.FindAsync(id);
        if (ev == null)
        {
            return NotFound();
        }
        return Ok(ev);
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

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
    {
        var appMetadataClaim = User.Claims.FirstOrDefault(c => c.Type == "app_metadata");
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

        var ev = await context.Events.FindAsync(id);
        if (ev == null)
        {
            return NotFound();
        }

        // ADD FIELDS IN EVENTS AND CHANGE FIELDS IN THE SUPABASE
        ev.event_name = updatedEvent.event_name;
        ev.event_date = updatedEvent.event_date;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var appMetadataClaim = User.Claims.FirstOrDefault(c => c.Type == "app_metadata");
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

        var ev = await context.Events.FindAsync(id);
        if (ev == null)
        {
            return NotFound();
        }

        context.Events.Remove(ev);
        await context.SaveChangesAsync();
        return NoContent();
    }
}