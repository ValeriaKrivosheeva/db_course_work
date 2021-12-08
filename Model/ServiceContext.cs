using Microsoft.EntityFrameworkCore;
namespace Model
{
    public class ServiceContext : DbContext
    {
        public DbSet<Garment> garments { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> order_items { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=online-shop;Username=postgres;Password=LeraLera");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .HasOne(p => p.client)
                .WithMany(t => t.reviews)
                .HasForeignKey(p => p.client_id)
                .HasPrincipalKey(t=>t.id);

            modelBuilder.Entity<Review>()
                .HasOne(p => p.garment)
                .WithMany(t => t.reviews)
                .HasForeignKey(p => p.garment_id)
                .HasPrincipalKey(t=>t.id);

            modelBuilder.Entity<Order>()
                .HasOne(p => p.client)
                .WithMany(t => t.orders)
                .HasForeignKey(p => p.client_id)
                .HasPrincipalKey(t=>t.id);

            modelBuilder.Entity<Garment>()
                .HasMany(c => c.orders)
                .WithMany(s => s.garments)
                .UsingEntity<OrderItem>(
                    j => j
                    .HasOne(pt => pt.order)
                    .WithMany(t => t.order_items)
                    .HasForeignKey(pt => pt.order_id),
                    j => j
                    .HasOne(pt => pt.garment)
                    .WithMany(p => p.order_items)
                    .HasForeignKey(pt => pt.garment_id));
        }

    }
}