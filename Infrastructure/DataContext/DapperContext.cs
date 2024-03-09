using Npgsql;

namespace Infrastructure.DataContext;

public class DapperContext
{

    private readonly string _connectionString = "Server=localhost;Port=5432;Database=ShopDb;User Id=postgres;Password=2007";

    public NpgsqlConnection Connection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
