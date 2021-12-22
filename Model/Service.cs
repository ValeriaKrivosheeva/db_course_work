using Microsoft.EntityFrameworkCore;

namespace Model
{
    public class Service
    {
        public GarmentRepository garmentRepository;
        public OrderRepository orderRepository;
        public ReviewRepository reviewRepository;
        public ClientRepository clientRepository;
        public OrderItemRepository orderItemRepository;
        private ServiceContext context;
        private ServiceContext replica;
        public Service()
        {
            CreateContext();
            this.garmentRepository = new GarmentRepository(context);
            this.orderRepository = new OrderRepository(context);
            this.reviewRepository = new ReviewRepository(context);
            this.clientRepository = new ClientRepository(context);
            this.orderItemRepository = new OrderItemRepository(context);
        }
        public void CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServiceContext>();
            context = new ServiceContext("Host=localhost;Database=online_shop;Username=postgres;Password=LeraLera");
            var result = -1;
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM pg_publication";
                context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                    if (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                context.Database.CloseConnection();
            }

            if (result == 0)
            {
                context.Database.ExecuteSqlRaw("CREATE PUBLICATION logical_pub FOR ALL TABLES;");
            }

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM pg_replication_slots";
                context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                    if (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                context.Database.CloseConnection();
            }

            if (result == 0)
            {
                context.Database.ExecuteSqlRaw("SELECT * FROM pg_create_logical_replication_slot('logical_slot', 'pgoutput');");
            }
            replica = new ServiceContext("Host=localhost;Database=new_online_shop;Username=postgres;Password=LeraLera");
            replica.CreateSubscription();
        }
    }
}
