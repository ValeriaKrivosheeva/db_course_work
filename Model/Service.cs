

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
        public Service()
        {
            context = new ServiceContext();
            this.garmentRepository = new GarmentRepository(context);
            this.orderRepository = new OrderRepository(context);
            this.reviewRepository = new ReviewRepository(context);
            this.clientRepository = new ClientRepository(context);
            this.orderItemRepository = new OrderItemRepository(context);
        }
    }
}
