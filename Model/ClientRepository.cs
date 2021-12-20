using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
namespace Model
{
    public class ClientRepository
    {
        private ServiceContext context;
        public ClientRepository(ServiceContext context)
        {
            this.context = context;
        }
        public int Insert(Client client)
        {
            context.clients.Add(client);
            context.SaveChanges();
            return client.id;
        }
        public void Update(int id, Client client)
        {
            var local = context.clients.Find(id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(client).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void DeleteById(int id)
        {
            context.clients.Remove(context.clients.Find(id));
            context.SaveChanges();
        }
        public Client GetById(int id)
        {
            Client result = context.clients.Find(id);
            return result;
        }
        public List<Client> GetByFullname(string fullname)
        {
            List<Client> clients = context.clients.Where(x => x.fullname == fullname).ToList();
            return clients;
        }
        public void InsertRange(List<Client> clients)
        {
            context.AddRange(clients);
            context.SaveChanges();
        }
        public int GetCount()
        {
            int result = context.clients.Count<Client>();
            return result;
        }
        public void CreateFullnameIndex()
        {
            string sql = "CREATE INDEX IF NOT EXISTS cl_fullname ON clients USING hash(fullname)";
            context.Database.ExecuteSqlRaw(sql);
        }
        public void DropFullnameIndex()
        {
            string sql = "DROP INDEX IF EXISTS cl_fullname";
            context.Database.ExecuteSqlRaw(sql);
        }
        public string GetRandomFullnameForChart()
        {
            Random rand = new Random();
            int toSkip = rand.Next(1, context.clients.Count());
            return context.clients.Skip(toSkip).Take(1).First().fullname;
        }
    }
}