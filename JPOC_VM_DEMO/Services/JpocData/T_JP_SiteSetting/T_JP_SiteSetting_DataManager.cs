using JPOC_VM_DEMO.Services.JpocData.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace JPOC_VM_DEMO.Services.JpocData.T_JP_SiteSetting
{
    public class T_JP_SiteSetting_DataManager
    {
        //private static readonly IDatabaseFactory _databaseFactory = new DatabaseFactory(); //TODO: FIX STAIC METHODS
        private readonly IDatabaseFactory _databaseFactory; //TODO: FIX STAIC METHODS

        public T_JP_SiteSetting_DataManager(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region Object

        private static T_JP_SiteSetting SetObject(DataTable dt)
        {
            try
            {
                if (dt?.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    return new T_JP_SiteSetting
                    {
                        Parameter = row["Parameter"] is DBNull ? string.Empty : row["Parameter"].ToString(),
                        Value = row["Value"] is DBNull ? string.Empty : row["Value"].ToString(),
                        modified_by = row["modified_by"] is DBNull ? 0 : Convert.ToInt32(row["modified_by"]),
                        modified_date = (DateTime)(row["modified_date"] is DBNull ? (DateTime?)null : Convert.ToDateTime(row["modified_date"]))
                    };
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private static List<T_JP_SiteSetting> SetList(DataTable dt)
        {
            try
            {
                var list = new List<T_JP_SiteSetting>();

                if (dt?.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new T_JP_SiteSetting
                        {
                            Parameter = row["Parameter"] is DBNull ? string.Empty : row["Parameter"].ToString(),
                            Value = row["Value"] is DBNull ? string.Empty : row["Value"].ToString(),
                            modified_by = row["modified_by"] is DBNull ? 0 : Convert.ToInt32(row["modified_by"]),
                            modified_date = (DateTime)(row["modified_date"] is DBNull ? (DateTime?)null : Convert.ToDateTime(row["modified_date"]))
                        });
                    }
                }
                return list.Count > 0 ? list : null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Return Object

        // Keep sync method for compatibility
        public T_JP_SiteSetting ReturnObject(string storeProc)
        {
            return ReturnObjectAsync(storeProc).GetAwaiter().GetResult();
        }

        public async Task<T_JP_SiteSetting> ReturnObjectAsync(string storeProc)
        {
            try
            {
                var ds = await _databaseFactory.ExecuteAsync(storeProc);
                var dt = ds?.Tables[0];
                return dt?.Rows.Count > 0 ? SetObject(dt) : null;
            }
            catch
            {
                return null;
            }
        }

        // Keep sync method for compatibility
        public T_JP_SiteSetting ReturnObject(string storeProc, List<SqlParameter> sqlParameters)
        {
            return ReturnObjectAsync(storeProc, sqlParameters).GetAwaiter().GetResult();
        }

        public async Task<T_JP_SiteSetting> ReturnObjectAsync(string storeProc, List<SqlParameter> sqlParameters)
        {
            try
            {
                var ds = await _databaseFactory.ExecuteAsync(storeProc, sqlParameters);
                var dt = ds?.Tables[0];
                return dt?.Rows.Count > 0 ? SetObject(dt) : null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Return List of Object

        // Keep sync method for compatibility
        public List<T_JP_SiteSetting> ReturnList(string storeProc)
        {
            return ReturnListAsync(storeProc).GetAwaiter().GetResult();
        }

        public async Task<List<T_JP_SiteSetting>> ReturnListAsync(string storeProc)
        {
            try
            {
                var ds = await _databaseFactory.ExecuteAsync(storeProc);
                var dt = ds?.Tables[0];
                return dt?.Rows.Count > 0 ? SetList(dt) : null;
            }
            catch
            {
                return null;
            }
        }

        // Keep sync method for compatibility
        public List<T_JP_SiteSetting> ReturnList(string storeProc, List<SqlParameter> sqlParameters)
        {
            return ReturnListAsync(storeProc, sqlParameters).GetAwaiter().GetResult();
        }

        public async Task<List<T_JP_SiteSetting>> ReturnListAsync(string storeProc, List<SqlParameter> sqlParameters)
        {
            try
            {
                var ds = await _databaseFactory.ExecuteAsync(storeProc, sqlParameters);
                var dt = ds?.Tables[0];
                return dt?.Rows.Count > 0 ? SetList(dt) : null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Return as Datatable

        // Keep sync method for compatibility
        public DataTable ReturnDT(string storeProc)
        {
            return ReturnDTAsync(storeProc).GetAwaiter().GetResult();
        }

        public async Task<DataTable> ReturnDTAsync(string storeProc)
        {
            try
            {
                var ds = await _databaseFactory.ExecuteAsync(storeProc);
                var dt = ds?.Tables[0];
                return dt?.Rows.Count > 0 ? dt : null;
            }
            catch
            {
                return null;
            }
        }

        // Keep sync method for compatibility
        public DataTable ReturnDT(string storeProc, List<SqlParameter> sqlParameters)
        {
            return ReturnDTAsync(storeProc, sqlParameters).GetAwaiter().GetResult();
        }

        public async Task<DataTable> ReturnDTAsync(string storeProc, List<SqlParameter> sqlParameters)
        {
            try
            {
                var ds = await _databaseFactory.ExecuteAsync(storeProc, sqlParameters);
                var dt = ds?.Tables[0];
                return dt?.Rows.Count > 0 ? dt : null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Return as Dataset

        // Keep sync method for compatibility
        public DataSet ReturnSet(string storeProc)
        {
            return ReturnSetAsync(storeProc).GetAwaiter().GetResult();
        }

        public async Task<DataSet> ReturnSetAsync(string storeProc)
        {
            try
            {
                return await _databaseFactory.ExecuteAsync(storeProc);
            }
            catch
            {
                return null;
            }
        }

        // Keep sync method for compatibility
        public DataSet ReturnSet(string storeProc, List<SqlParameter> sqlParameters)
        {
            return ReturnSetAsync(storeProc, sqlParameters).GetAwaiter().GetResult();
        }

        public async Task<DataSet> ReturnSetAsync(string storeProc, List<SqlParameter> sqlParameters)
        {
            try
            {
                return await _databaseFactory.ExecuteAsync(storeProc, sqlParameters);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Return String

        // Keep sync method for compatibility
        public string ReturnDTStatus(string storeProc, List<SqlParameter> sqlParameters)
        {
            return ReturnDTStatusAsync(storeProc, sqlParameters).GetAwaiter().GetResult();
        }

        public async Task<string> ReturnDTStatusAsync(string storeProc, List<SqlParameter> sqlParameters)
        {
            try
            {
                var ds = await _databaseFactory.ExecuteAsync(storeProc, sqlParameters);
                var dt = ds?.Tables[0];
                return dt?.Rows.Count > 0 ? dt.Rows[0][0].ToString() : null;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}

