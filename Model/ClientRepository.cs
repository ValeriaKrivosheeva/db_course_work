using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
    }
}