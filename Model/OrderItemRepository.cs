using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Model
{
    public class OrderItemRepository
    {
        private ServiceContext context;
        public OrderItemRepository(ServiceContext context)
        {
            this.context = context;
        }
        public void Insert(OrderItem orderItem)
        {
            context.order_items.Add(orderItem);
            context.SaveChanges();
        }
        public void DeleteByOrderId(int orderId)
        {
            context.order_items.RemoveRange(context.order_items.Where(x => x.order_id == orderId));
            context.SaveChanges();
        }
        public void DeleteByGarmentId(int garmentId)
        {
            context.order_items.RemoveRange(context.order_items.Where(x => x.garment_id == garmentId));
            context.SaveChanges();
        }
    }
}