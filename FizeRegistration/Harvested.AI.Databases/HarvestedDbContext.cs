using Harvested.AI.Databases.TableMaps;
using Harvested.AI.Databases.TableMaps.Identity;
using Microsoft.EntityFrameworkCore;

namespace Harvested.AI.Databases {
    public class HarvestedDbContext : DbContext {
        public HarvestedDbContext() { }

        public DbSet<UserIdentity> UserIdentities { get; set; }

        public HarvestedDbContext(
            DbContextOptions<HarvestedDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(ConfigurationManager.DatabaseConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.AddConfiguration(new UserIdentityMap());
        }
    }
}