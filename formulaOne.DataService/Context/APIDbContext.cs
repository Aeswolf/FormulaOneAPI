using formulaOne.Entities.DbSets;
using Microsoft.EntityFrameworkCore;

namespace formulaOne.DataService.Context;

public class APIDbContext : DbContext
{
    public APIDbContext(DbContextOptions<APIDbContext> contextOptions) : base(contextOptions) { }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Achievement> Achievements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
            entity.HasOne(d => d.Driver)
                    .WithMany(a => a.Achievements)
                    .HasForeignKey(a => a.DriverId)
                    .OnDelete(DeleteBehavior.NoAction)
        );
    }
}