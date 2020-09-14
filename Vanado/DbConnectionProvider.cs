using Npgsql;
using System.Data;
using Vanado.Interfaces;

namespace Vanado
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
