using MetricsManager.Models;
using Microsoft.EntityFrameworkCore;

namespace MetricsManager.Services
{
    public class AgetsContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<AgentInfo> AgentInfos { get; set; }

        public AgetsContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("agentsdb");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AgentInfo>(e =>
            {
                e.ToTable("Agents");

                e.HasKey(x => x.AgentId).HasName("agentId");

                e.Property(x => x.AgentAddress)
                .HasColumnName("Address");

                e.Property(x => x.Enable)
                .HasColumnName("enabled");
            });
        }
    }
}
