psql --command "UPDATE pg_database SET datallowconn = 'false' WHERE datname = 'online_shop'" "host=localhost port=5432 dbname=postgres user=postgres password=LeraLera"
psql --command "SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'online_shop'" "host=localhost port=5432 dbname=postgres user=postgres password=LeraLera"
psql --command "DROP DATABASE online_shop" "host=localhost port=5432 dbname=postgres user=postgres password=LeraLera"
psql --command "CREATE DATABASE online_shop" "host=localhost port=5432 dbname=postgres user=postgres password=LeraLera"
pg_restore -d online_shop $1 -h localhost -p 5432 -d online_shop -U postgres