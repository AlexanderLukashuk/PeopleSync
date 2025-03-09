using EventBookinkAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventBookinkAPI.Data;

public class PeopleSyncDbContext : DbContext
{
    public PeopleSyncDbContext(DbContextOptions<PeopleSyncDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Registration> Registrations { get; set; }
}