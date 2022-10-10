using Microsoft.EntityFrameworkCore;

namespace Assessment.Models
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {

        }

        public DbSet<PlatformDto> Platform { get; set; } = null!;

        public DbSet<WellDto> Well { get; set; } = null!;

        public DbSet<PlatformDummyDto> PlatformDummy { get; set; } = null!;

        public DbSet<WellDummyDto> WellDummy { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlatformDto>()
                .HasMany(e => e.Well).WithOne(e => e.Platform);

            modelBuilder.Entity<PlatformDummyDto>()
                .HasMany(e => e.Wells).WithOne(e => e.Platform);
        }
    }
}
