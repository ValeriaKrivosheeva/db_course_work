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
        public string connectionString { get; set; }
        public ServiceContext(string connectionString)
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
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
        public void CreateSubscription()
        {
            var result = -1;
            using (var command = this.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM pg_subscription";
                this.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                    if (reader.Read())
                        result = reader.GetInt32(0);
                this.Database.CloseConnection();
            }
            if (result == 0)
                this.Database.ExecuteSqlRaw("CREATE SUBSCRIPTION logical_sub\n" +
                                                "CONNECTION 'host=localhost port=5432 user=postgres password=LeraLera dbname=online_shop'\n" +
                                                "PUBLICATION logical_pub\n" +
                                                "WITH(create_slot = false, slot_name = 'logical_slot');");
        }
    }
}