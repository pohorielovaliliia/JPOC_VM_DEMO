using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using JPOC_VM_DEMO.Services.JpocData.Utilities;

namespace JPOC_VM_DEMO.Services.JpocData.T_JP_SiteSetting
{
    /// <summary>
    /// Business manager for site settings
    /// </summary>
    public class T_JP_SiteSetting_BusinessManager
    {
        private static readonly T_JP_SiteSetting_DataManager T_JP_SiteSetting_DataManager = new T_JP_SiteSetting_DataManager(new DatabaseFactoryNew()); //TODO: FIX STATIC METHODS

        public static class View
        {
            // Keep sync method for compatibility
            public static string GetParameter(string parameter)
            {
                return GetParameterAsync(parameter).GetAwaiter().GetResult();
            }

            public static async Task<string> GetParameterAsync(string parameter)
            {
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("@Parameter", parameter)
                };

                return await T_JP_SiteSetting_DataManager.ReturnDTStatusAsync("[P_JP_SiteSetting_GET]", sqlParameters);
            }
        }

        public static class Insert
        {
            // Insert methods would go here
        }

        public static class Update
        {
            // Keep sync method for compatibility
            public static string UpdateParameter(string parameter, string value, string modifiedBy)
            {
                return UpdateParameterAsync(parameter, value, modifiedBy).GetAwaiter().GetResult();
            }

            public static async Task<string> UpdateParameterAsync(string parameter, string value, string modifiedBy)
            {
                var sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("@Parameter", parameter),
                    new SqlParameter("@Value", value),
                    new SqlParameter("@modifiedBy", modifiedBy)
                };

                return await T_JP_SiteSetting_DataManager.ReturnDTStatusAsync("[P_JP_SiteSetting_Update]", sqlParameters);
            }
        }

        public static class Delete
        {
            // TODO: Delete methods would go here
        }

        public static class Import
        {
            // TODO: Import methods would go here
        }
    }
}
