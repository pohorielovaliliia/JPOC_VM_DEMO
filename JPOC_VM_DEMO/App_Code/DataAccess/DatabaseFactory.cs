using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using JPOC_VM_DEMO.Common;

namespace JPOC_VM_DEMO.App_Code.DataAccess
{
    public class DatabaseFactory
    {
        //private readonly string _connectionString;
        //private readonly ILogger<DatabaseFactory> _logger;

        public static SqlConnection GetConn()
        {
            var conn = new SqlConnection(GlobalVariables.ConnectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);
                throw;
            }
            return conn;
        }

        public static string JPoCScalar(string query, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            string result = string.Empty;
            using (var conn = GetConn())
            using (var comm = new SqlCommand(query, conn))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                try
                {
                    var scalar = comm.ExecuteScalar();
                    result = scalar?.ToString() ?? string.Empty;
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, ex.Message);
                    throw;
                }
            }
            return result;
        }

        public static string JPoCScalar(SqlConnection conn, string query, List<SqlParameter> parameters, SqlTransaction transaction, CommandType commandType = CommandType.Text)
        {
            string result = string.Empty;
            if (conn == null) conn = GetConn();
            if (conn.State != ConnectionState.Open) conn.Open();
            using (var comm = new SqlCommand(query, conn, transaction))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                comm.Prepare();
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                var scalar = comm.ExecuteScalar();
                result = scalar?.ToString() ?? string.Empty;
            }
            return result;
        }

        public static string JPoCScalar(SqlConnection conn, string query, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            string result = string.Empty;
            if (conn == null) conn = GetConn();
            if (conn.State != ConnectionState.Open) conn.Open();
            using (var comm = new SqlCommand(query, conn))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                comm.Prepare();
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                var scalar = comm.ExecuteScalar();
                result = scalar?.ToString() ?? string.Empty;
            }
            return result;
        }

        public static int JPoCExecuteNoResult(string query, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int result = 0;
            using (var conn = GetConn())
            using (var comm = new SqlCommand(query, conn))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                try
                {
                    result = comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, ex.Message);
                    throw;
                }
            }
            return result;
        }

        public static DataSet JPoCExecute(string query, string tableName = "")
        {
            var ds = new DataSet();
            using (var conn = GetConn())
            using (var comm = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(comm))
            {
                comm.CommandType = CommandType.Text;
                comm.CommandTimeout = 600;
                try
                {
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        da.Fill(ds, tableName);
                        AddRowKey(ds, tableName);
                    }
                    else
                    {
                        da.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, ex.Message);
                    throw;
                }
            }
            return ds;
        }

        public static void JPoCExecute(string query, ref DataSet ds, string tableName, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            if (ds == null) ds = new DataSet();
            using (var conn = GetConn())
            using (var comm = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(comm))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                comm.Prepare();
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                try
                {
                    da.Fill(ds, tableName);
                    AddRowKey(ds, tableName);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, ex.Message);
                    throw;
                }
            }
        }

        public static DataTable JPoCExecute(string query, ref DataTable dt, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            if (dt == null) dt = new DataTable();
            using (var conn = GetConn())
            using (var comm = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(comm))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                comm.Prepare();
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                try
                {
                    da.Fill(dt);
                    AddRowKey(dt);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, ex.Message);
                    throw;
                }
            }
            return dt;
        }

        public static int JPoCExecute(SqlConnection conn, string query, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            int ret = 0;
            if (conn == null) conn = GetConn();
            if (conn.State != ConnectionState.Open) conn.Open();
            using (var comm = new SqlCommand(query, conn))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                comm.Prepare();
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                ret = comm.ExecuteNonQuery();
            }
            return ret;
        }

        public static int JPoCExecute(SqlConnection conn, string query, List<SqlParameter> parameters, SqlTransaction transaction, CommandType commandType = CommandType.StoredProcedure)
        {
            int ret = 0;
            if (conn == null) conn = GetConn();
            if (conn.State != ConnectionState.Open) conn.Open();
            using (var comm = new SqlCommand(query, conn, transaction))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                comm.Prepare();
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                ret = comm.ExecuteNonQuery();
            }
            return ret;
        }

        public static int JPoCExecute(SqlConnection conn, SqlTransaction transaction, string query, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int ret = 0;
            if (conn == null) conn = GetConn();
            if (conn.State != ConnectionState.Open) conn.Open();
            using (var comm = new SqlCommand(query, conn, transaction))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                comm.Prepare();
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                ret = comm.ExecuteNonQuery();
            }
            return ret;
        }

        public static DataTable JPoCExecute(SqlConnection conn, string query, ref DataTable dt, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            if (dt == null) dt = new DataTable();
            if (conn == null) conn = GetConn();
            if (conn.State != ConnectionState.Open) conn.Open();
            using (var comm = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(comm))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                comm.Prepare();
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                da.Fill(dt);
                AddRowKey(dt);
            }
            return dt;
        }

        public static SqlDataReader JPoCReader(SqlConnection conn, string query, List<SqlParameter> parameters, CommandType commandType = CommandType.Text)
        {
            if (conn == null) conn = GetConn();
            if (conn.State != ConnectionState.Open) conn.Open();
            var comm = new SqlCommand(query, conn)
            {
                CommandType = commandType,
                CommandTimeout = 600
            };
            comm.Prepare();
            if (parameters != null)
            {
                comm.Parameters.AddRange(parameters.ToArray());
            }
            // Note: Caller is responsible for disposing the reader and connection
            return comm.ExecuteReader();
        }

        public static SqlDataReader JPoCReader(SqlConnection conn, string query, List<SqlParameter> parameters, SqlTransaction transaction, CommandType commandType = CommandType.Text)
        {
            if (conn == null) conn = GetConn();
            if (conn.State != ConnectionState.Open) conn.Open();
            var comm = new SqlCommand(query, conn, transaction)
            {
                CommandType = commandType,
                CommandTimeout = 600
            };
            comm.Prepare();
            if (parameters != null)
            {
                comm.Parameters.AddRange(parameters.ToArray());
            }
            // Note: Caller is responsible for disposing the reader and connection
            return comm.ExecuteReader();
        }

        public static DataSet JPoCExecute(string query, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            var ds = new DataSet();
            using (var conn = GetConn())
            using (var comm = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(comm))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, ex.Message);
                    throw;
                }
            }
            return ds;
        }

        public static DataTable JPoCExecute(string query, List<SqlParameter> parameters, ref DataTable dt, CommandType commandType = CommandType.StoredProcedure)
        {
            if (dt == null) dt = new DataTable();
            using (var conn = GetConn())
            using (var comm = new SqlCommand(query, conn))
            using (var da = new SqlDataAdapter(comm))
            {
                comm.CommandType = commandType;
                comm.CommandTimeout = 600;
                if (parameters != null)
                {
                    comm.Parameters.AddRange(parameters.ToArray());
                }
                try
                {
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, ex.Message);
                    throw;
                }
            }
            return dt;
        }

        // Utility methods
        public static void AddRowKey(DataSet ds, string tableName)
        {
            if (!ds.Tables.Contains(tableName)) return;
            AddRowKey(ds.Tables[tableName]);
            ds.Tables[tableName].AcceptChanges();
        }

        public static void AddRowKey(DataTable dt)
        {
            if (!dt.Columns.Contains("__ROWKEY"))
            {
                dt.Columns.Add(new DataColumn("__ROWKEY", typeof(int)));
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i].RowState == DataRowState.Deleted) continue;
                dt.Rows[i]["__ROWKEY"] = i;
            }
            dt.AcceptChanges();
        }
    }

}
