using System.Data;
using Microsoft.Data.SqlClient;

namespace JPOC_VM_DEMO.Services
{
    public interface IDatabaseService
    {
        Task<DataTable> ExecuteQueryAsync(string sqlQuery);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<DataTable> ExecuteQueryAsync(string sqlQuery)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sqlQuery, connection);
            var dataTable = new DataTable();

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            dataTable.Load(reader);

            return dataTable;
        }
    }
}
