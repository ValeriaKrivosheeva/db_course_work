using Microsoft.EntityFrameworkCore;
namespace Model
{
    public class OrderItemRepository
    {
        private ServiceContext context;
        public OrderItemRepository(ServiceContext context)
        {
            this.context = context;
        }
    }
}