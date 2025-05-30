using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using JPOC_VM_DEMO.Services.JpocData.T_JP_SiteSetting;

namespace JPOC_VM_DEMO.Common
{
    public class GlobalVariables : Jpoc.Tools.Core.GlobalVariables
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public GlobalVariables(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        #region Constants
        public static class Session
        {
            public const string USER_INF = "USER_INF";
            public const string SESS_CMS_ID = "JPOC_CMS_ID";
            public const string SESS_CMS_UserID = "JPOC_CMS_USERID";
            public const string SESS_CMS_UserRole = "JPOC_CMS_USERROLE";
            public const string SESS_CMS_Name = "JPOC_CMS_NAME";
            public const string SESS_CMS_Email = "JPOC_CMS_EMAIL";
            public const string SESS_CMS_LastLogin = "JPOC_CMS_LASTLOGIN";
            public const string SESS_CMS_Message = "JPOC_CMS_MESSAGE";
            public const string SESS_Tentative_DiseaseID = "JPOC_TENTATIVE_DISEASEID";
        }

        public static class Cookie
        {
            public const string ENQUETE = "ENQUETE";
            public const string ENQUETE_IPUSER_COMPLETEDENQUETE = "ENQUETE_IPUSER_COMPLETEDENQUETE";
        }

        public const int DISPLAY_MAX_LEN_USER_NAME = 38;
        public const string NAME_TITLE = "様";
        public const string REGEX_JPC_SOURCE_TEXT = @"★\[(JPC )*(?<jpc>([0-9].*?))\](?<name>(.*?))☆";
        public const string REGEX_JPC_REPLACE_TEXT = "<a target='_parent' href='/jpoc/drugDetails.aspx?SearchTerm={0}&SearchText={1}&Code={2}'>{3}</a>";
        public const int VM_INS_ID = 997;
        public const string VM_ADMIN_ID = "Admin";
        public const string VM_USER_ID = "VMUser";
        public const string VM_USER_PASS = "elsdemo";

        public List<string> NoLoginPage { get; } = new List<string>
        {
            "sorry", "author", "pwdrevoke", "privacypolicy", "underconstruction",
            "termsconditions", "agreement", "quotation", "quotationrequest",
            "quotationcontactform", "accesssummaryreportscheduler", "accesscontentsreportscheduler"
        };

        public struct VMUpdateMode
        {
            public const string BulkSync = "bulksync";
            public const string FileUpload = "fileupload";
        }
        #endregion

        #region HTTP Request Related
        public HttpContext Current => _httpContextAccessor.HttpContext;

        public HttpRequest Request => Current?.Request;
        #endregion

        #region Settings File Reading
        public AppPageSettings PageSettings => AppPageSettings.GetInstance(
            Path.Combine(SettingsFolderPhysicalPath, "PageSettings.xml"));
        #endregion

        #region Configuration Settings
        public static string SolrUrl => Configure.GetConfig("SolrUrl");
        public static string SourceName => Configure.GetConfig("SourceName");

        public static string IP_Restriction => Configure.GetConfig("IP_Restriction");

        public static string ConnectionString => Configure.GetConfig("ConnectionStrings");
        public static List<string> EnableClinicalKeySearchPageName => new List<string>
        {"contentpage", "searchdetails", "searchresults", "fulltextsearch", "actiondetails", "situationdetails", "evidencedetails", "imagetab"};

        public static bool EnableClinicalKeySearch => (!String.IsNullOrWhiteSpace(Configure.GetConfig("ClinikalKeyApiUrl")) 
            && !String.IsNullOrWhiteSpace(Configure.GetConfig("ClinikalKeyField"))) ? true : false; 

        public bool EnableCms
        {
            get
            {
                var val = Configure.GetConfig("EnableCms");
                return !string.IsNullOrEmpty(val) && bool.TryParse(val, out bool ret) && ret;
            }
        }

        // TODO: Continue with other configuration properties following the same pattern...

        public static string VMUpdateDate
        {
            get
            {
                if (RunTimeEnvironment == Jpoc.Common.PublicEnum.eRuntimeEnvironment.VM)
                {
                    return T_JP_SiteSetting_BusinessManager.View.GetParameter("VMUPDATEDATE");
                }
                return string.Empty; // For VM Only
            }
        }
        #endregion

        #region URL & Path Related
        protected string RequestUrl
        {
            get
            {
                if (Request != null)
                {
                    return Request.Path.ToString().Replace("{", string.Empty).Replace("}", string.Empty);
                }
                return string.Empty;
            }
        }

        public static string RootUrl => "~";

        public string RootPhysicalPath => Current?.Request.PathBase.Value ?? string.Empty;

        // Continue with other URL/Path properties...
        #endregion

        #region Token Authentication
        public class Rijndael128Bit
        {
            public static string iv = "!ZVY9QAX#FM>8R*V";
            public static string secretKey = Configure.GetConfig("Rijndael128BitKey");
        }

        public class STEP_SERVER_RECOGNITION
        {
            public const string PC_HTTP_STEP_INSTITUTIONCODE = "PC_HTTP_STEP_INSTITUTIONCODE_JPOC$";
        }
        #endregion
    }
}

