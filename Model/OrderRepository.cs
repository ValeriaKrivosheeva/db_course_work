using Microsoft.EntityFrameworkCore;
namespace Model
{
    public class OrderRepository
    {
        private ServiceContext context;
        public OrderRepository(ServiceContext context)
        {
            this.context = context;
        }
        public void Generate(int amount)
        {
            string query = @$"CREATE EXTENSION IF NOT EXISTS tsm_system_rows;
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
                            timestamp '2020-01-01' + random() * (timestamp '2021-12-12' - timestamp '2020-01-01'),
                            random_choice(array['by air', 'by land', 'by sea']) as random_char,
                            random_client_id()
                            FROM generate_series(1, {amount})";
            context.Database.ExecuteSqlRaw(query);
        }
    }
}