using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TercerosExternos.Infrastructure.Data
{
    public class DapperConnectionFactory
    {
        private readonly string _connectionString;

        public DapperConnectionFactory(IConfiguration configuration)
        {
            // Obtener la cadena de conexión desde la configuración
            _connectionString = configuration.GetConnectionString("LrpGenericoDB");

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'LrpGenericoDB' no está configurada.");
            }
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}