using System;
using System.Data;
using Microsoft.Data.SqlClient;
using JPOC_VM_DEMO.Common;

namespace JPOC_VM_DEMO.Services.JpocData.Utilities
{
    public interface IDatabaseFactory
    {
        DataSet Execute(string query);
        string ConnectionString { get; }
        Task<string> ScalarAsync(string query);
        Task<string> ScalarAsync(string storedProcedure, List<SqlParameter> parameters);
        Task<DataSet> ExecuteAsync(string query);
        Task<DataSet> ExecuteAsync(string storedProcedure, List<SqlParameter> parameters);
        Task<DataSet> ExecuteSqlQueryWithParamAsync(string query, List<SqlParameter> parameters);
        string ConvertToSafeQuery(string query);
        int ConvertToInteger(object value);
        string ConvertToString(object value);
        DateTime ConvertToDateTime(object value);
        bool ConvertToBoolean(object value);
    }

    public class DatabaseFactoryNew : IDatabaseFactory
    {
        private readonly string _connectionString;
        private const int CommandTimeout = 600;

        public DatabaseFactoryNew()
        {
            _connectionString = GlobalVariables.ConnectionString;
        }

        public string ConnectionString => _connectionString;

        #region Database I/O

        public async Task<string> ScalarAsync(string query)
        {
            using var conn = new SqlConnection(_connectionString);
            using var comm = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = query,
                CommandTimeout = CommandTimeout
            };

            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }

            var result = await comm.ExecuteScalarAsync();
            return ConvertToString(result);
        }

        public async Task<string> ScalarAsync(string storedProcedure, List<SqlParameter> parameters)
        {
            using var conn = new SqlConnection(_connectionString);
            using var comm = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.StoredProcedure,
                CommandText = storedProcedure,
                CommandTimeout = CommandTimeout
            };

            if (parameters != null)
            {
                comm.Parameters.AddRange(parameters.ToArray());
            }

            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }

            var result = await comm.ExecuteScalarAsync();
            return ConvertToString(result);
        }
        public DataSet Execute(string query)
        {
            using var conn = new SqlConnection(_connectionString);
            using var comm = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = query,
                CommandTimeout = CommandTimeout
            };

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            var ds = new DataSet();
            using var da = new SqlDataAdapter(comm);
            da.Fill(ds);
            return ds;
        }

        public async Task<DataSet> ExecuteAsync(string query)
        {
            using var conn = new SqlConnection(_connectionString);
            using var comm = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = query,
                CommandTimeout = CommandTimeout
            };

            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }

            var ds = new DataSet();
            using var da = new SqlDataAdapter(comm);
            await Task.Run(() => da.Fill(ds)); // SqlDataAdapter.Fill doesn't have async version
            return ds;
        }

        public async Task<DataSet> ExecuteSqlQueryWithParamAsync(string query, List<SqlParameter> parameters)
        {
            using var conn = new SqlConnection(_connectionString);
            using var comm = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.Text,
                CommandText = query,
                CommandTimeout = CommandTimeout
            };

            if (parameters != null)
            {
                comm.Parameters.AddRange(parameters.ToArray());
            }

            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }

            var ds = new DataSet();
            using var da = new SqlDataAdapter(comm);
            await Task.Run(() => da.Fill(ds));
            return ds;
        }

        public async Task<DataSet> ExecuteAsync(string storedProcedure, List<SqlParameter> parameters)
        {
            using var conn = new SqlConnection(_connectionString);
            using var comm = new SqlCommand
            {
                Connection = conn,
                CommandType = CommandType.StoredProcedure,
                CommandText = storedProcedure,
                CommandTimeout = CommandTimeout
            };

            if (parameters != null)
            {
                comm.Parameters.AddRange(parameters.ToArray());
            }

            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }

            var ds = new DataSet();
            using var da = new SqlDataAdapter(comm);
            await Task.Run(() => da.Fill(ds));
            return ds;
        }

        #endregion

        #region Conversion Methods
        // These methods remain synchronous as they are simple conversions
        public string ConvertToSafeQuery(string query) => query.Replace("'", "''");

        public int ConvertToInteger(object value) =>
            Convert.IsDBNull(value) ? 0 : Convert.ToInt32(value);

        public string ConvertToString(object value) =>
            Convert.IsDBNull(value) ? string.Empty : Convert.ToString(value);

        public DateTime ConvertToDateTime(object value) =>
            Convert.IsDBNull(value) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(value);

        public bool ConvertToBoolean(object value) =>
            !Convert.IsDBNull(value) && Convert.ToBoolean(value);
        #endregion
    }
}

