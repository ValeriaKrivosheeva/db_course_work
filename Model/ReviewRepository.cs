using Microsoft.EntityFrameworkCore;
namespace Model
{
    public class ReviewRepository
    {
        private ServiceContext context;
        public ReviewRepository(ServiceContext context)
        {
            this.context = context;
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