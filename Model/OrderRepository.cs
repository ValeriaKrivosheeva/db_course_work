using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
namespace Model
{
    public class OrderRepository
    {
        private ServiceContext context;
        public OrderRepository(ServiceContext context)
        {
            this.context = context;
        }
        public int Insert(Order order)
        {
            context.orders.Add(order);
            context.SaveChanges();
            return order.id;
        }
        
        public void Update(int id, Order order)
        {
            var local = context.orders.Find(id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(order).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void DeleteById(int id)
        {
            context.orders.Remove(context.orders.Find(id));
            context.SaveChanges();
        }
        public void DeleteByClientId(int clientId)
        {
            List<Order> orders = context.orders.Where(x => x.client_id == clientId).ToList();
            foreach (Order order in orders)
            {
                context.order_items.RemoveRange(context.order_items.Where(x => x.order_id == order.id));
            }
            context.orders.RemoveRange(orders);
            context.SaveChanges();
        }
        public Order GetById(int id)
        {
            Order result = context.orders.Find(id);
            return result;
        }
        public void Generate(int amount)
        {
            string query = @$"CREATE TRIGGER order_items
                            AFTER INSERT ON orders
                            FOR EACH ROW EXECUTE PROCEDURE trigger();

                            CREATE EXTENSION IF NOT EXISTS tsm_system_rows;
                            CREATE OR REPLACE FUNCTION random_client_id() RETURNS INT as $$
                            SELECT id FROM clients TABLESAMPLE system_rows(1);
                            $$ language sql;
                            CREATE OR REPLACE FUNCTION random_choice(choices text[])
                            RETURNS text AS $$
                            DECLARE
                                size_ int;
                            BEGIN
                                size_ = array_length(choices, 1);
                                RETURN (choices)[floor(random()*size_)+1];
                            END
                            $$ LANGUAGE plpgsql;
                            INSERT INTO orders (created_date, shipping_method, client_id)
                            SELECT
                            timestamp '2018-01-01' + random() * (timestamp '2021-12-12' - timestamp '2018-01-01'),
                            random_choice(array['by air', 'by land', 'by sea']) as random_char,
                            random_client_id()
                            FROM generate_series(1, {amount});
                            
                            DROP TRIGGER order_items ON orders";
            context.Database.ExecuteSqlRaw(query);
        }
    }
}