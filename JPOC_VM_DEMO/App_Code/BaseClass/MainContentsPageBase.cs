using JPOC_VM_DEMO.App_Code.BaseClass;
using System.Data;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Text.RegularExpressions;
using JPOC_VM_DEMO.Common;

using GlobalVariables = JPOC_VM_DEMO.Common.GlobalVariables;
using PublicEnum = JPOC_VM_DEMO.Common.PublicEnum;
using JPOC_VM_DEMO.App_Code.Util;

namespace JPOC_VM_DEMO.App_Code.BaseClass
{
    public class MainContentsPageBase : PageBaseModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        #region Properties

        #region IDs
        public string DiseaseID
        {
            get
            {
                return _session.GetString("DiseaseID") ?? string.Empty;
            }
        }

        public string SituationID
        {
            get
            {
                return _session.GetString("SituationID") ?? string.Empty;
            }
        }

        protected internal string ParamCode
        {
            get
            {
                var code = _session.GetString("Code");
                return !string.IsNullOrEmpty(code) ? System.Net.WebUtility.UrlDecode(code) : string.Empty;
            }
        }

        protected internal string SrlIdString
        {
            get
            {
                return Regex.Replace(ParamCode, @"\[\s*SRLID", "", RegexOptions.IgnoreCase).Replace("]", "");
            }
        }

        protected internal string ParamOrderSetID
        {
            get
            {
                return _session.GetString("OrderSetID") ?? string.Empty;
            }
        }
        #endregion

        #region Tables
        protected DataSet DataSource { get; set; }

        protected DataTable DiseaseTable =>
            DataSource?.Tables.Contains("Disease") == true ? DataSource.Tables["Disease"] : null;

        protected DataView DiseaseView =>
            DiseaseTable != null ? new DataView(DiseaseTable, string.Empty, "__ROWKEY", DataViewRowState.CurrentRows) : null;

        protected DataRowView DiseaseRow =>
            DiseaseView?.Count > 0 ? DiseaseView[0] : null;

        protected DataTable SituationTable =>
            DataSource?.Tables.Contains("Situation") == true ? DataSource.Tables["Situation"] : null;

        protected DataTable SituationOrderSetTable => DataSource?.Tables["SituationOrderSet"];
        protected DataTable SituationOrderSetSampleTable => DataSource?.Tables["SituationOrderSetSample"];
        protected DataTable SituationOrderSetSampleItemTable => DataSource?.Tables["SituationOrderSetSampleItem"];
        protected DataTable PatientProfileTable => DataSource?.Tables["SituationOrderSetPatientProfile"];
        protected DataTable ImagesTable => DataSource?.Tables["Images"];
        protected DataTable JournalsTable => DataSource?.Tables["Journals"];
        protected DataTable EvidenceTable => DataSource?.Tables["Evidence"];
        protected DataTable DiseaseActionTypeTable => DataSource?.Tables["DiseaseActionType"];
        protected DataTable ActionItemTable => DataSource?.Tables["ActionItem"];
        protected DataTable DecisionDiagramTable => DataSource?.Tables["DecisionDiagram"];
        protected DataTable DecisionDiagramExplanationsTable => DataSource?.Tables["DecisionDiagramExplanations"];
        protected DataTable DiffrentialDiagnosisTable => DataSource?.Tables["DiffrentialDiagnosis"];
        protected DataTable DiseasePopupLinkListTable => DataSource?.Tables["DiseasePopupLinkList"];
        protected DataTable DiseaseSearchResultTable => DataSource?.Tables["DiseaseSearch"];
        protected DataTable LabTestTable1Table => DataSource?.Tables["LabTestTable1"];
        protected DataTable LabTestTable2Table => DataSource?.Tables["LabTestTable2"];
        protected DataTable LabTestTable4Table => DataSource?.Tables["LabTestTable4"];
        protected DataTable LabTestTable5Table => DataSource?.Tables["LabTestTable5"];

        protected DataTable DrugDataTable =>
            DataSource?.Tables.Contains("DrugData") == true ? DataSource.Tables["DrugData"] : null;

        protected bool EnableClinicalKeySearch
        {
            get
            {
                if (!GlobalVariables.EnableClinicalKeySearch) return false;

                if (GlobalVariables.RunTimeEnvironment.Equals(PublicEnum.eRuntimeEnvironment.DEMO) ||
                    GlobalVariables.RunTimeEnvironment.Equals(PublicEnum.eRuntimeEnvironment.VM))
                {
                    return false;
                }

                if (!HasUserInfo) return false;
                if (UserInfo.RoleId == (int) PublicEnum.eRole.Agency ||
                    UserInfo.RoleId == (int) PublicEnum.eRole.DvdUser ||
                    UserInfo.RoleId == (int) PublicEnum.eRole.Individual ||
                    UserInfo.RoleId == (int) PublicEnum.eRole.Trial)
                {
                    return false;
                }

                return GlobalVariables.EnableClinicalKeySearchPageName.Contains(PageName);
            }
        }
        #endregion
        #endregion

        public MainContentsPageBase(IHttpContextAccessor httpContextAccessor, ILogger<PageBase> logger, JPoCUtility jpocUtility)
            : base(httpContextAccessor, logger, jpocUtility)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = httpContextAccessor.HttpContext.Session;
            DataSource = null;
        }

        protected virtual void OnPageInitialize()
        {
            // Replace Page_Init event handler
        }

        protected void SetMessage(string message)
        {
            // This needs to be implemented differently in .NET Core
            // Consider using TempData, ViewData, or a service for message handling
            TempData["Message"] = message;
        }
    }
}

