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
    }
}