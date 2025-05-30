using Jpoc.Common;
using Jpoc.Dac;
using Jpoc.Tools.Core;
using System.Diagnostics;

using PublicEnum = JPOC_VM_DEMO.Common.PublicEnum;

namespace JPOC_VM_DEMO.App_Code.CTRL.DTO
{
    public abstract class AbstractDto : IDisposable, IDto
    {
        #region Properties
        public PublicEnum.eRtnCD RtnCD { get; set; }
        public MessageSet MessageSet { get; set; }
        public string SessionID { get; set; } = string.Empty;
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public DS_DISEASE DiseaseDataSet { get; set; }
        public DS_DRUG DrugDataSet { get; set; }
        public DS_LAB_TEST LabTestDataSet { get; set; }
        public DS_SYSTEM SystemDataSet { get; set; }
        public DS_USER UserDataSet { get; set; }
        public DS_VIEW ViewDataSet { get; set; }
        #endregion

        #region Constructor
        protected AbstractDto()
        {
            try
            {
                
                BaseInit();
                Init();
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

        #region Initialization
        public abstract void Init();

        protected void BaseInit()
        {
            RtnCD = PublicEnum.eRtnCD.Normal;
            MessageSet = new MessageSet();
            SessionID = string.Empty;
            StartTime = 0.0;
            EndTime = 0.0;
            DiseaseDataSet = new DS_DISEASE();
            DrugDataSet = new DS_DRUG();
            LabTestDataSet = new DS_LAB_TEST();
            SystemDataSet = new DS_SYSTEM();
            UserDataSet = new DS_USER();
            ViewDataSet = new DS_VIEW();
        }
        #endregion

        #region IDisposable Implementation
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    DiseaseDataSet?.Dispose();
                    DrugDataSet?.Dispose();
                    LabTestDataSet?.Dispose();
                    SystemDataSet?.Dispose();
                    UserDataSet?.Dispose();
                    ViewDataSet?.Dispose();
                }

                // Clear datasets
                DiseaseDataSet?.Clear();
                DrugDataSet?.Clear();
                LabTestDataSet?.Clear();
                SystemDataSet?.Clear();
                UserDataSet?.Clear();
                ViewDataSet?.Clear();

                // Set to null
                DiseaseDataSet = null;
                DrugDataSet = null;
                LabTestDataSet = null;
                SystemDataSet = null;
                UserDataSet = null;
                ViewDataSet = null;

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

