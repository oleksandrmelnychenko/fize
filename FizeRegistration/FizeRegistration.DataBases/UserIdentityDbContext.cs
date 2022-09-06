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
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Fize;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserIdentity>().Property(p => p.Created).HasDefaultValueSql("getutcdate()");
        modelBuilder.Entity<UserIdentity>().HasIndex(p => p.Email).IsUnique();
    }
}