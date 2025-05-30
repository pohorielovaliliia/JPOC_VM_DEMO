using System;
using System.Data;
using System.Collections.Generic;
using Jpoc.Common;
using JPOC_VM_DEMO.App_Code.DataAccess;

namespace JPOC_VM_DEMO.App_Code.Model
{
    namespace Jpoc.Core.Models
    {
        /// <summary>
        /// User information model
        /// </summary>
        public class UserModel : IDisposable
        {
            // Properties
            public string Id { get; set; }
            public string UserCode { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public int RoleId { get; set; }
            public DateTime LastLogin { get; set; }
            public string InstitutionId { get; set; }
            public string InstitutionCode { get; set; }
            public PublicEnum.eUserType UserType { get; set; }
            public DateTime LoginExpire { get; set; }
            public bool AdminFlag { get; set; }
            public string UserHostAddress { get; set; }
            public string UserHostName { get; set; }
            public string UserAgent { get; set; }
            public string InstitutionName { get; set; }
            public bool IsBan { get; set; }
            public bool IsCreditError { get; set; }
            public PublicEnum.eLoginType LoginType { get; set; }
            public int SubscriptionID { get; set; }
            public bool IndTcAcceptance { get; set; }
            public bool IndRuacceptance { get; set; }
            public bool IndPpacceptance { get; set; }
            public int LoginCount { get; set; }
            public DataTable RoleFunction { get; set; }

            /// <summary>
            /// Returns true if the user is an Elsevier manager.
            /// </summary>
            public bool IsElsevierManager =>
                UserType == PublicEnum.eUserType.INT &&
                (RoleId == (int)PublicEnum.eRole.ElsevierAdmin ||
                 RoleId == (int)PublicEnum.eRole.ElsevierManager ||
                 RoleId == (int)PublicEnum.eRole.TechnicalSupport);

            /// <summary>
            /// Default constructor. Initializes default values.
            /// </summary>
            public UserModel()
            {
                Id = string.Empty;
                UserCode = string.Empty;
                Name = string.Empty;
                Email = string.Empty;
                RoleId = -1;
                LastLogin = new DateTime(1901, 1, 1);
                InstitutionId = string.Empty;
                InstitutionCode = string.Empty;
                UserType = PublicEnum.eUserType.Undefined;
                LoginExpire = new DateTime(2099, 12, 31);
                AdminFlag = false;
                UserHostAddress = string.Empty;
                UserHostName = string.Empty;
                UserAgent = string.Empty;
                InstitutionName = string.Empty;
                LoginType = PublicEnum.eLoginType.Undifined;
                IndTcAcceptance = false;
                IndRuacceptance = false;
                IndPpacceptance = false;
                RoleFunction = CreateRoleFunctionTable();
                SubscriptionID = 0;
                IsBan = false;
                IsCreditError = false;
                LoginCount = 0;
            }

            /// <summary>
            /// Loads user role functions from the database.
            /// </summary>
            public void LoadUserRoleFunctions()
            {
                var paramList = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("@role_id", System.Data.SqlDbType.Int)
                {
                    Direction = ParameterDirection.Input,
                    Value = this.RoleId
                }
            };
                DataTable roleTable = this.RoleFunction;
                DatabaseFactory.JPoCExecute(
                    "P_JP_Roles_Function_GetByRoleID",
                    ref roleTable,
                    paramList,
                    CommandType.StoredProcedure);

                foreach (DataRow dr in this.RoleFunction.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted) continue;
                    int functionId = Utilities.N0Int(dr["FunctionID"]);
                    int parentId = Utilities.N0Int(dr["ParentID"]);
                    // Additional logic if needed
                }
                this.RoleFunction.AcceptChanges();
            }

            /// <summary>
            /// Checks if the user has any role functions.
            /// </summary>
            public bool HasRoleFunction()
            {
                return this.RoleFunction != null && this.RoleFunction.Rows.Count > 0;
            }

            /// <summary>
            /// Creates the role function DataTable.
            /// </summary>
            private DataTable CreateRoleFunctionTable()
            {
                var dt = new DataTable();
                dt.Columns.Add(new DataColumn("FunctionID", typeof(int)));
                dt.Columns.Add(new DataColumn("ParentID", typeof(int)));
                dt.Columns.Add(new DataColumn("MenuName", typeof(string)));
                dt.Columns.Add(new DataColumn("URL", typeof(string)));
                dt.Columns.Add(new DataColumn("SequenceOrder", typeof(int)));
                dt.Columns.Add(new DataColumn("Abbr_Function", typeof(string)));
                return dt;
            }

            #region IDisposable Support
            private bool disposedValue = false;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // Dispose managed state (managed objects).
                        RoleFunction?.Dispose();
                    }
                    disposedValue = true;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion
        }
    }

}
