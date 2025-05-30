using System.Diagnostics;
using Jpoc.Common;
using JPOC_VM_DEMO.App_Code.CTRL.DTO;
using JPOC_VM_DEMO.App_Code.CTRL.Logic;

namespace JPOC_VM_DEMO.App_Code.CTRL
{
    public class CtrlSearch : AbstractCtrl
    {
        #region Properties
        public DtoSearch Dto => (DtoSearch)base.Dto;

        private LogicSearch Logic => (LogicSearch)base.Logic;
        #endregion

        #region Constructor
        public CtrlSearch(AbstractDto dto) : base(dto)
        {
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

        #region IDisposable
        public new void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Data Retrieval
        public void GetData(string institutionId = "")
        {
            Logic.GetData(institutionId);
        }

        public void GetAllMedicalCalculatorMenuData()
        {
            Logic.GetAllMedicalCalculatorMenuData();
        }
        #endregion
    }
}
