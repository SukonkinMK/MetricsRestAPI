using MetricsAgent.Models;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace MetricsAgent.Services
{
    public class MetricsContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<CpuMetric> CpuMetrics { get; set; }
        public DbSet<DotNetMetric> DotNetMetrics { get; set; }
        public DbSet<HddMetric> HddMetrics { get; set; }
        public DbSet<NetworkMetric> NetworkMetrics { get; set; }
        public DbSet<RamMetric> RamMetrics { get; set; }

        public MetricsContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("metricsdb");
        }

        //public MetricsContext(DbContextOptions<MetricsContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CpuMetric>(e =>
            {
                e.ToTable("cpumetrics");

                e.HasKey(x  => x.Id).HasName("cpumetricId");

                e.Property(x => x.Value)
                .HasColumnName("value");

                e.Property(x=> x.Time)
                .HasColumnName("time");
            });

            modelBuilder.Entity<DotNetMetric>(e =>
            {
                e.ToTable("dotnetmetrics");

                e.HasKey(x => x.Id).HasName("dotnetmetricId");

                e.Property(x => x.Value)
                .HasColumnName("value");

                e.Property(x => x.Time)
                .HasColumnName("time");
            });

            modelBuilder.Entity<HddMetric>(e =>
            {
                e.ToTable("hddmetrics");

                e.HasKey(x => x.Id).HasName("hddmetricId");

                e.Property(x => x.Value)
                .HasColumnName("value");

                e.Property(x => x.Time)
                .HasColumnName("time");
            });

            modelBuilder.Entity<NetworkMetric>(e =>
            {
                e.ToTable("networkmetrics");

                e.HasKey(x => x.Id).HasName("networkmetricId");

                e.Property(x => x.Value)
                .HasColumnName("value");

                e.Property(x => x.Time)
                .HasColumnName("time");
            });

            modelBuilder.Entity<RamMetric>(e =>
            {
                e.ToTable("rammetrics");

                e.HasKey(x => x.Id).HasName("rammetricId");

                e.Property(x => x.Value)
                .HasColumnName("value");

                e.Property(x => x.Time)
                .HasColumnName("time");
            });
        }
    }
    
}
