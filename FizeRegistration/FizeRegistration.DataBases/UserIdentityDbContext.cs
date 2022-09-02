namespace FizeRegistration.DataBases;

using FizeRegistration.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

public class UserIdentityDbContext : DbContext
{
    public UserIdentityDbContext()
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
    }

    public DbSet<UserIdentity> UserIdentities { get; set; }

    // public HarvestedDbContext(
    //     DbContextOptions<HarvestedDbContext> options) : base(options) {
    //      }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=tcp:fizeserver.database.windows.net,1433;Initial Catalog=fizedata;Persist Security Info=False;User ID=balis77;Password=Nspre169169!;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserIdentity>().Property(p => p.Created).HasDefaultValueSql("getutcdate()");
        modelBuilder.Entity<UserIdentity>().HasIndex(p => p.Email).IsUnique();
    }
}