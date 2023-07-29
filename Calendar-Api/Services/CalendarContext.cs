using Microsoft.EntityFrameworkCore;
using Calendar_Api.Models;

namespace Calendar_Api.Services;

public class CalendarContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public CalendarContext(DbContextOptions<CalendarContext> options)
        : base(options)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseMySQL("server=localhost;database=myDatabase;user=raudel;password=2505");
    // }
}