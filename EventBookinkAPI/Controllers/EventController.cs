using EventBookinkAPI.Data;
using EventBookinkAPI.Models;
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

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        var events = await context.Events.ToListAsync();
        return Ok(events);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent(Event ev)
    {
        context.Events.Add(ev);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvents), new { id = ev.id}, ev);
    }
}