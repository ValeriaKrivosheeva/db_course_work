using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Collections.Generic;
using System.Linq;
namespace Model
{
    public class GarmentRepository
    {
        private ServiceContext context;
        public GarmentRepository(ServiceContext context)
        {
            this.context = context;
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
    }
}