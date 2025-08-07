using Microsoft.EntityFrameworkCore;
using OrderAndCargo.Domain.Entities;

namespace OrderAndCargo.Infrastructure.Data
{
    public class OrderAndCargoDbContext : DbContext
    {
        public OrderAndCargoDbContext(DbContextOptions<OrderAndCargoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }      
        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Order>().Property(o => o.CargoCompany).HasConversion<string>();

            b.Entity<Order>(cfg =>
            {
                cfg.HasKey(o => o.Id);
                cfg.Property(o => o.CargoCompany).IsRequired();

                cfg.HasMany(o => o.OrderItems)          
                   .WithOne(i => i.Order)
                   .HasForeignKey(i => i.OrderId)
                   .OnDelete(DeleteBehavior.Cascade); 
            });

            b.Entity<OrderItem>(cfg =>
            {
                cfg.HasKey(i => i.Id);
                cfg.Property(i => i.Quantity).IsRequired();
            });
        }
    }
}

