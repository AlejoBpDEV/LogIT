using Dapper;
using TercerosExternos.Domain.Interfaces;
using TercerosExternos.Infrastructure.Data;

namespace TercerosExternos.Infrastructure.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        private readonly DapperConnectionFactory _connectionFactory;

        public DapperRepository(DapperConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, parameters);
            }
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                return await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}