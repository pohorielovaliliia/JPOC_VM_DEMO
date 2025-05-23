using System.Data;
using System.ComponentModel.DataAnnotations;
using static JPOC_VM_DEMO.Common.PublicEnum;

namespace JPOC_VM_DEMO.Models
{
    /// <summary>
    /// User information model
    /// </summary>
    [Serializable]
    public class UserModel : IDisposable
    {
        /// <summary>
        /// User ID
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Login ID
        /// </summary>
        [Required]
        public string UserCode { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Role ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Last login time
        /// </summary>
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// Institution ID
        /// </summary>
        public string InstitutionId { get; set; }

        /// <summary>
        /// Institution code
        /// </summary>
        public string InstitutionCode { get; set; }

        /// <summary>
        /// User type
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// Login expiration date
        /// </summary>
        public DateTime LoginExpire { get; set; }

        /// <summary>
        /// Administrator flag
        /// </summary>
        public bool AdminFlag { get; set; }

        /// <summary>
        /// IP address
        /// </summary>
        public string UserHostAddress { get; set; }

        /// <summary>
        /// Host name
        /// </summary>
        public string UserHostName { get; set; }

        /// <summary>
        /// Browser info
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Institution name
        /// </summary>
        public string InstitutionName { get; set; }

        /// <summary>
        /// Account ban flag
        /// </summary>
        public bool IsBan { get; set; }

        /// <summary>
        /// Credit error flag
        /// </summary>
        public bool IsCreditError { get; set; }

        /// <summary>
        /// Login type
        /// </summary>
        public LoginType LoginType { get; set; }

        /// <summary>
        /// Subscription ID
        /// </summary>
        public int SubscriptionId { get; set; }

        public bool IndTcAcceptance { get; set; }
        public bool IndRuAcceptance { get; set; }
        public bool IndPpAcceptance { get; set; }

        /// <summary>
        /// Login count
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// Administrator menu
        /// </summary>
        public DataTable RoleFunction { get; private set; }

        /// <summary>
        /// Elsevier administrator status
        /// </summary>
        /// <returns>True if user is an administrator</returns>
        /// <remarks>Administrator conditions: UserType is INT and RoleId is ElsevierAdmin or ElsevierManager</remarks>
        public bool IsElsevierManager =>
            UserType == UserType.INT &&
            (RoleId == (int)Role.ElsevierAdmin ||
             RoleId == (int)Role.ElsevierManager ||
             RoleId == (int)Role.TechnicalSupport);

        private readonly IDbService _dbService;
        private bool _disposedValue;

        public UserModel(IDbService dbService = null)
        {
            _dbService = dbService;

            // Initialize default values
            Id = string.Empty;
            UserCode = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            RoleId = -1;
            LastLogin = new DateTime(1901, 1, 1);
            InstitutionId = string.Empty;
            UserType = UserType.Undefined;
            LoginExpire = new DateTime(2099, 12, 31);
            AdminFlag = false;
            UserHostAddress = string.Empty;
            UserHostName = string.Empty;
            UserAgent = string.Empty;
            InstitutionName = string.Empty;
            LoginType = LoginType.Undefined;
            IndTcAcceptance = false;
            IndRuAcceptance = false;
            IndPpAcceptance = false;
            RoleFunction = CreateRoleFunctionTable();
            SubscriptionId = 0;
            IsBan = false;
            LoginCount = 0;
        }

        /// <summary>
        /// Load administrator menu settings
        /// </summary>
        /// <remarks>Sets administrator menu linked to role ID</remarks>
        public async Task LoadUserRoleFunctionsAsync()
        {
            if (_dbService == null)
            {
                throw new InvalidOperationException("Database service not initialized");
            }

            try
            {
                RoleFunction = await _dbService.GetRoleFunctionsAsync(RoleId);

                // Process role functions if needed
                foreach (DataRow dr in RoleFunction.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted) continue;

                    int functionId = ToInt32(dr["FunctionID"]);
                    int parentId = ToInt32(dr["ParentID"]);
                    // Additional processing can be added here
                }

                RoleFunction.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error loading user role functions", ex);
            }
        }
        /// <summary>
        /// Converts an object to integer with default value handling
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <param name="defaultValue">Default value if conversion fails</param>
        /// <param name="raiseExceptionWhenCastError">Whether to throw exceptions on cast errors</param>
        /// <returns>Converted integer value or default value</returns>
        public static int ToInt32(object obj, int defaultValue = 0, bool raiseExceptionWhenCastError = false)
        {
            int returnValue = defaultValue;

            try
            {
                if (obj == null || obj == DBNull.Value)
                {
                    returnValue = defaultValue;
                }
                else if (!IsNumeric(obj))
                {
                    returnValue = defaultValue;
                }
                else
                {
                    try
                    {
                        returnValue = Convert.ToInt32(obj);
                    }
                    catch (Exception ex) when (!raiseExceptionWhenCastError)
                    {
                        returnValue = defaultValue;
                    }
                }
            }
            catch (Exception ex) when (!raiseExceptionWhenCastError)
            {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        // Helper method to check if object is numeric (similar to VB.NET's IsNumeric)
        private static bool IsNumeric(object expression)
        {
            if (expression == null)
                return false;

            return expression is sbyte
                || expression is byte
                || expression is short
                || expression is ushort
                || expression is int
                || expression is uint
                || expression is long
                || expression is ulong
                || expression is float
                || expression is double
                || expression is decimal
                || (expression is string && decimal.TryParse((string)expression,
                       System.Globalization.NumberStyles.Any,
                       System.Globalization.CultureInfo.CurrentCulture,
                       out _));
        }


        /// <summary>
        /// Check if role function exists
        /// </summary>
        /// <returns>True if exists</returns>
        public bool HasRoleFunction() =>
            RoleFunction?.Rows.Count > 0;

        /// <summary>
        /// Create role function table
        /// </summary>
        private static DataTable CreateRoleFunctionTable()
        {
            var dt = new DataTable();
            dt.Columns.AddRange(new[]
            {
                new DataColumn("FunctionID", typeof(int)),
                new DataColumn("ParentID", typeof(int)),
                new DataColumn("MenuName", typeof(string)),
                new DataColumn("URL", typeof(string)),
                new DataColumn("SequenceOrder", typeof(int)),
                new DataColumn("Abbr_Function", typeof(string))
            });
            return dt;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    RoleFunction?.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public interface IDbService
    {
        Task<DataTable> GetRoleFunctionsAsync(int roleId);
    }
}
