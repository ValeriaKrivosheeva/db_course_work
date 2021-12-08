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