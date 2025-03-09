using EventBookinkAPI.Data;
using EventBookinkAPI.Models;
using EventBookinkAPI.Models.Dtos;
using EventBookinkAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventBookinkAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly PeopleSyncDbContext context;
    private readonly SupabaseAuthService authService;

    public AuthController(PeopleSyncDbContext context)
    {
        this.context = context;
        authService = new SupabaseAuthService();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto user)
    {
        // if (await context.Users.AnyAsync(u => u.email == user.email))
        // {
        //     return BadRequest(new { message = "Email already in use" });
        // }

        // // user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        // context.Users.Add(user);
        // await context.SaveChangesAsync();

        // return Ok(new { message = "User registered successfully" });

        var session = await authService.Register(user.email, user.password);
        return Ok(new { token = session.AccessToken });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDto user)
    {
        // var existingUser = await context.Users.FirstOrDefaultAsync(u => u.email == user.email);
        // if (existingUser == null)
        // {
        //     return Unauthorized(new { message = "Invalid email" });
        // }

        // if (user.password != existingUser.password)
        // {
        //     return Unauthorized(new { message = "Invalid password" });
        // }

        // return Ok(new { message = "Login successfully" });

        var session = await authService.Login(user.email, user.password);
        return Ok(new { tokem = session.AccessToken });
    }
}