using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;
namespace Model
{
    public class ReviewRepository
    {
        private ServiceContext context;
        public ReviewRepository(ServiceContext context)
        {
            this.context = context;
        }
        public int Insert(Review review)
        {
            context.reviews.Add(review);
            context.SaveChanges();
            return review.id;
        }
        public void Update(int id, Review review)
        {
            var local = context.reviews.Find(id);
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(review).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void DeleteById(int id)
        {
            context.reviews.Remove(context.reviews.Find(id));
            context.SaveChanges();
        }
        public void DeleteByClientId(int clientId)
        {
            context.reviews.RemoveRange(context.reviews.Where(x => x.client_id == clientId));
            context.SaveChanges();
        }
        public void DeleteByGarmentId(int garmentId)
        {
            context.reviews.RemoveRange(context.reviews.Where(x => x.garment_id == garmentId));
            context.SaveChanges();
        }
        public Review GetById(int id)
        {
            Review result = context.reviews.Find(id);
            return result;
        }
        public List<Review> GetByClientId(int clientId)
        {
            List<Review> reviews = context.reviews.Where(x => x.client_id == clientId).ToList();
            return reviews;
        }
        public List<Review> GetByGarmentId(int garmentId)
        {
            List<Review> reviews = context.reviews.Where(x => x.garment_id == garmentId).ToList();
            return reviews;
        }
        public List<int> GetGarmentRatings(int garmentId)
        {
            List<int> result = context.reviews.Where(x => x.garment_id == garmentId).Select(o => o.rating).ToList();
            return result;
        }
        public Review GetHighestRatingReview(int garmentId)
        {
            try
            {
                return context.reviews.Where(x => x.garment_id == garmentId).OrderByDescending(o => o.rating).First();
            }
            catch
            {
                return null;
            }
        }
        public Review GetLowestRatingReview(int garmentId)
        {
            try
            {
                return context.reviews.Where(x => x.garment_id == garmentId).OrderBy(o => o.rating).First();
            }
            catch
            {
                return null;
            }
        }
        public double GetGarmentRating(int garmentId)
        {
            return context.reviews.Where(x => x.garment_id == garmentId).Average(o => o.rating);
        }
        public int GetCount()
        {
            int result = context.reviews.Count<Review>();
            return result;
        }
        public void CreateBtreeIndex()
        {
            string sql = "CREATE INDEX IF NOT EXISTS rw_index ON reviews USING btree(rating, posted_at)";
            context.Database.ExecuteSqlRaw(sql);
        }
        public void DropBtreeIndex()
        {
            string sql = "DROP INDEX IF EXISTS rw_index";
            context.Database.ExecuteSqlRaw(sql);
        }
        public List<Review> GetByRatingAndPosted(int rating, DateTime posted_at)
        {
            List<Review> result = context.reviews.Where(x => x.rating >= rating && x.posted_at > posted_at).ToList();
            return result;
        }
        public void Generate(int amount)
        {
            string query = @$"CREATE EXTENSION IF NOT EXISTS tsm_system_rows;
                            CREATE OR REPLACE FUNCTION random_string(int) RETURNS TEXT as $$
                            SELECT string_agg(substring('abcdfghjkmnpqrstvwxyz', round(random()*30)::integer, 1), '') FROM generate_series(1, $1);
                            $$ language sql;
                            CREATE OR REPLACE FUNCTION random_client_id() RETURNS INT as $$
                            SELECT id FROM clients TABLESAMPLE system_rows(1);
                            $$ language sql;
                            CREATE OR REPLACE FUNCTION random_garment_id() RETURNS INT as $$
                            SELECT id FROM garments TABLESAMPLE system_rows(1);
                            $$ language sql;
                            INSERT INTO reviews (opinion, rating, posted_at, client_id, garment_id)
                            SELECT
                            random_string(20),
                            trunc(random()*10+1)::int,
                            timestamp '2018-01-01' + random() * (timestamp '2021-12-12' - timestamp '2018-01-01'),
                            random_client_id(),
                            random_garment_id()
                            FROM generate_series(1, {amount})";
            context.Database.ExecuteSqlRaw(query);
        }
    }
}