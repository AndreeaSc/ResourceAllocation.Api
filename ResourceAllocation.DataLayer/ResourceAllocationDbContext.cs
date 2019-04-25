using Microsoft.EntityFrameworkCore;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer
{
    public class ResourceAllocationDbContext : DbContext
    {
        public ResourceAllocationDbContext(DbContextOptions<ResourceAllocationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DesignerArtists>().HasKey(x => new { x.ArtistId, x.DesignerId });

            modelBuilder.Entity<DesignerArtists>()
                .HasOne(bc => bc.Artist)
                .WithMany(b => b.FavoriteForDesigners)
                .HasForeignKey(bc => bc.ArtistId);  
            modelBuilder.Entity<DesignerArtists>()
                .HasOne(bc => bc.Designer)
                .WithMany(c => c.FavoriteArtists)
                .HasForeignKey(bc => bc.DesignerId);
        }   

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Designer> Designers { get; set; }
    }
}