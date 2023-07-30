using Microsoft.EntityFrameworkCore;
using Calendar_Api.Models;

namespace Calendar_Api.Services;

public class CalendarContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Event> Events { get; set; } = null!;

    public CalendarContext(DbContextOptions<CalendarContext> options)
        : base(options)
    {
    }
}