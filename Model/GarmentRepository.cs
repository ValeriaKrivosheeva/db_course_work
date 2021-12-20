using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Collections.Generic;
using System.Linq;
using System;
namespace Model
{
    public class GarmentRepository
    {
        private ServiceContext context;
        public GarmentRepository(ServiceContext context)
        {
            this.context = context;
        }
        public int Insert(Garment garment)
        {
            context.garments.Add(garment);
            context.SaveChanges();
            return garment.id;
        }
        
        public void Update(int id, Garment garment)
        {
            var local = context.garments.Find(id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(garment).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void DeleteById(int id)
        {
            context.garments.Remove(context.garments.Find(id));
            context.SaveChanges();
        }
        public Garment GetById(int id)
        {
            Garment result = context.garments.Find(id);
            return result;
        }
        public void InsertRange(List<Garment> garments)
        {
            context.AddRange(garments);
            context.SaveChanges();
        }
        public int GetCount()
        {
            int result = context.garments.Count<Garment>();
            return result;
        }
        public List<Garment> GetByBrand(string brand)
        {
            List<Garment> garments = context.garments.Where(x => x.brand == brand).ToList();
            return garments;
        }
        public List<Garment> GetByCostRangeAndBrand(string brand, int minCost, int maxCost)
        {
            List<Garment> garments = context.garments.Where(x => x.cost >= minCost && x.cost <= maxCost && x.brand == brand).ToList();
            return garments;
        }
        public List<Garment> GetByCostRange(int minCost, int maxCost)
        {
            List<Garment> garments = context.garments.Where(x => x.cost >= minCost && x.cost <= maxCost).ToList();
            return garments;
        }
        public void CreateBrandIndex()
        {
            string sql = "CREATE INDEX IF NOT EXISTS gm_brand ON garments USING hash(brand)";
            context.Database.ExecuteSqlRaw(sql);
        }
        public void DropBrandIndex()
        {
            string sql = "DROP INDEX IF EXISTS gm_brand";
            context.Database.ExecuteSqlRaw(sql);
        }
        public void CreateBtreeIndex()
        {
            string sql = "CREATE INDEX IF NOT EXISTS gm_cost ON garments USING btree (cost)";
            context.Database.ExecuteSqlRaw(sql);
        }
        public void DropBtreeIndex()
        {
            string sql = "DROP INDEX IF EXISTS gm_brand";
            context.Database.ExecuteSqlRaw(sql);
        }
        public string GetRandomBrandForChart()
        {
            Random rand = new Random();
            int toSkip = rand.Next(1, context.garments.Count());
            return context.garments.Skip(toSkip).Take(1).First().brand;
        }
        public List<double> GetClientsAge(int garmentId)
        {
            List<double> result = new List<double>();
            var connection = context.Database.GetDbConnection();
            
                try
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.garment_id = {garmentId} 
                        AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date >= timestamp '{DateTime.Now.Year - 18}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                        result.Add(Convert.ToDouble(command.ExecuteScalar()));

                        command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.garment_id = {garmentId} 
                        AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date < timestamp '{DateTime.Now.Year - 18}-{DateTime.Now.Month}-{DateTime.Now.Day}'
                        AND clients.birthday_date >= timestamp '{DateTime.Now.Year - 25}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                        result.Add(Convert.ToDouble(command.ExecuteScalar()));

                        command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.garment_id = {garmentId} 
                        AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date < timestamp '{DateTime.Now.Year - 25}-{DateTime.Now.Month}-{DateTime.Now.Day}'
                        AND clients.birthday_date >= timestamp '{DateTime.Now.Year - 35}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                        result.Add(Convert.ToDouble(command.ExecuteScalar()));

                        command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.garment_id = {garmentId} 
                        AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date < timestamp '{DateTime.Now.Year - 35}-{DateTime.Now.Month}-{DateTime.Now.Day}'
                        AND clients.birthday_date >= timestamp '{DateTime.Now.Year - 45}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                        result.Add(Convert.ToDouble(command.ExecuteScalar()));

                        command.CommandText = @$"SELECT Count(*) FROM clients, orders, order_items WHERE order_items.garment_id = {garmentId} 
                        AND orders.id = order_items.order_id AND clients.id = orders.client_id AND clients.birthday_date < timestamp '{DateTime.Now.Year - 45}-{DateTime.Now.Month}-{DateTime.Now.Day}'";
                        result.Add(Convert.ToDouble(command.ExecuteScalar()));
                    }
                }
                catch (System.Exception e) { Console.WriteLine(e.Message);}
                finally { connection.Close(); }
                return result;
            
        }
    }
}