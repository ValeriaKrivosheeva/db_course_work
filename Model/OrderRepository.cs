using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;
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
        public List<Order> GetByClientId(int clientId)
        {
            List<Order> orders = context.orders.Where(x => x.client_id == clientId).ToList();
            return orders;
        }
        public List<double> GetIncomesByYear(int year)
        {
            List<double> result = new List<double>();
            var connection = context.Database.GetDbConnection();
                try
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        for(int i = 1; i<12; i++)
                        {
                            command.CommandText = @$"SELECT SUM(garments.cost) FROM garments,orders,order_items WHERE orders.created_date >= timestamp '{year}-{i}-1' AND orders.created_date < timestamp '{year}-{i+1}-1'
                            AND orders.id = order_items.order_id AND order_items.garment_id = garments.id";
                            result.Add(Convert.ToDouble(command.ExecuteScalar()));
                        }
                        command.CommandText = @$"SELECT SUM(garments.cost) FROM garments,orders,order_items WHERE orders.created_date >= timestamp '{year}-12-1' AND orders.created_date < timestamp '{year+1}-1-1'
                        AND orders.id = order_items.order_id AND order_items.garment_id = garments.id";
                        result.Add(Convert.ToDouble(command.ExecuteScalar()));
                    }
                }
                catch (System.Exception e) { }
                finally { connection.Close(); }
                return result;
        }
        public void Generate(int amount)
        {
            string query = @$"CREATE TRIGGER order_items
                            AFTER INSERT ON orders
                            FOR EACH ROW EXECUTE PROCEDURE trigger();";
            context.Database.ExecuteSqlRaw(query);

            int[] clientIds = context.clients.Select(o => o.id).ToArray();
            Random rand = new Random();
            for(int i = 0; i < amount; i++)
            {
                query = @$"INSERT INTO orders (created_date, shipping_method, client_id)
                            SELECT
                            timestamp '2018-01-01' + random() * (timestamp '2021-12-12' - timestamp '2018-01-01'),
                            random_choice(array['by air', 'by land', 'by sea']) as random_char,
                            {clientIds[rand.Next(0,clientIds.Length)]}";
                context.Database.ExecuteSqlRaw(query);
            }
            query = "DROP TRIGGER order_items ON orders";
            context.Database.ExecuteSqlRaw(query);
        }
    }
}