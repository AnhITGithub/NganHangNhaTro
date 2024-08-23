using Microsoft.EntityFrameworkCore;
using BTL_WEB_NC.Models;

namespace BTL_WEB_NC.Data;

public class ApplicationContext : DbContext
{

    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<House> Houses { get; set; }
    public virtual DbSet<ImageCategory> ImageCategories { get; set; }
    public virtual DbSet<BookingCalender> BookingCalenders { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasData(
            new User { Id = Guid.NewGuid(), Name = "Admin", Email = "admin", Password = "admin", RoleId = "role1", PhoneNumber = "1111111111" }
        );
    }
}

