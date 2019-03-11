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
            modelBuilder.Entity<DesignerFashionModelEntity>()
                .HasKey(sc => new {sc.DesignerEntityId, sc.FashionModelEntityId});
        }

        public DbSet<FashionModelEntity> FashionModels { get; set; }
        public DbSet<DesignerEntity> Designers { get; set; }
        public DbSet<ShowEntity> Shows { get; set; }

        public DbSet<DesignerFashionModelEntity> DesignerFavoriteFashionModels { get; set; }
        public DbSet<DesignerFashionModelEntity> DesignerAllocatedFashionModels { get; set; }
    }
}