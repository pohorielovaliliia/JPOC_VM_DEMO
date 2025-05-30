using Jpoc.Common;
using Jpoc.Dac;
using JPOC_VM_DEMO.App_Code.CTRL.DTO;
using System.Data;
using System.Diagnostics;

namespace JPOC_VM_DEMO.App_Code.CTRL.Logic
{
    public class LogicSearch : AbstractLogic
    {
        #region Properties
        public DtoSearch dto => (DtoSearch) base.Dto;

        #endregion

        #region Constructor
        public LogicSearch(ElsDataBase dbManager, AbstractDto dto)
            : base(dbManager, dto)
        {
        }
        #endregion

        #region IDisposable
        public new void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Initialization
        public override void Init()
        {
            try
            {
                // Initialization logic here
            }
            catch (ElsException ex)
            {
                ex.AddStackFrame(new StackFrame(true));
                throw;
            }
            catch (Exception ex)
            {
                throw new ElsException(ex);
            }
        }
        #endregion

        #region Entry Validation
        public void CheckEntry(string targetType)
        {
            try
            {
                switch (targetType)
                {
                    case "Hoge":
                        //if (string.IsNullOrEmpty(Dto.LoginID))
                        //{
                        //    Dto.RtnCD = PublicEnum.ERtnCD.LogicalError;
                        //    Dto.MessageSet = Utilities.GetMessageSet("ERR0028", "ログインID");
                        //    return;
                        //}
                        break;
                    default:
                        break;
                }
            }
            catch (ElsException ex)
            {
                ex.AddStackFrame(new StackFrame(true));
                throw;
            }
            catch (Exception ex)
            {
                throw new ElsException(ex);
            }
        }
        #endregion

        #region Data Retrieval
        public void GetData(string institutionId = "")
        {
            DBManager.Connect();
            var dbManagerRef = DBManager; // Create a local variable to hold the reference
            var diseaseCategoryTable = Dto.DiseaseDataSet.T_JP_DiseaseCategory as DataTable;
            using (var dao = new DaoDiseaseCategory(ref dbManagerRef))
            {
                dao.GetByAllDisease(ref diseaseCategoryTable, true, institutionId);
            }
            using (var dao = new DaoDiseaseCategory(ref dbManagerRef))
            {
                dao.GetAll(ref diseaseCategoryTable, false);
            }
        }
        #endregion

        #region Display Data Retrieval
        /// <summary>
        /// Gets data for display, copied from MedicalCalculatorMenu.aspx
        /// </summary>
        public void GetAllMedicalCalculatorMenuData()
        {
            DBManager.Connect();
            var dbManagerRef = DBManager; // Create a local variable to hold the reference
            var diseaseCategoryTable = Dto.DiseaseDataSet.T_JP_DiseaseCategory as DataTable;
            using (var dao = new DaoMedicalCalculator(ref dbManagerRef))
            {
                dao.GetAll(ref diseaseCategoryTable, true);
            }
        }
        #endregion
    }
}
