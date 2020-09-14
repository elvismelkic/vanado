using Npgsql;
using System.Data;
using WebApplication3.Interfaces;

namespace WebApplication3
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=vanado";

        public IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}
