using Microsoft.EntityFrameworkCore;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.FashionModels
{
    public class ResourceAllocationDbContext : DbContext
    {
        public ResourceAllocationDbContext(DbContextOptions<ResourceAllocationDbContext> options) : base(options)
        {

        }

        public DbSet<FashionModelEntity> FashionModels { get; set; }
        public DbSet<DesignerEntity> Designers { get; set; }
        public DbSet<ShowEntity> Shows { get; set; }
    }
}