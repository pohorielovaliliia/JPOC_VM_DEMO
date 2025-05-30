using Jpoc.Common;
using Jpoc.Dac;
using Jpoc.Tools.Core;
using System.Data;
using System.Diagnostics;

using Utilities = JPOC_VM_DEMO.Common.Utilities;
using AbstractDto = JPOC_VM_DEMO.App_Code.CTRL.DTO.AbstractDto;

namespace JPOC_VM_DEMO.App_Code.CTRL.Logic
{
    public abstract class AbstractLogic : IDisposable, ILogic
    {
        #region Fields
        protected readonly AbstractDto _dto;
        private ElsDataBase _dbManager;
        private bool _disposedValue;
        #endregion

        #region Properties
        protected ElsDataBase DBManager => _dbManager;

        protected AbstractDto Dto => _dto;
        #endregion

        #region Constructors
        private AbstractLogic()
        {
            try
            {
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

        protected AbstractLogic(ElsDataBase dbManager, AbstractDto dto)
        {
            try
            {
                _dbManager = dbManager;
                _dto = dto;
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
        #endregion

        #region IDisposable Implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: Dispose managed resources
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Session Management
        public string GetSessionIdFromUser(int userId)
        {
            try
            {
                var dt = new DataTable();
                var dao = new DaoUser(ref _dbManager);

                var ret = dao.GetByPK(ref dt, userId, true);
                if (ret > 0)
                {
                    return Utilities.NZ(dt.Rows[0]["session_id"]);
                }

                return string.Empty;
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

        public string GetSessionIdFromInsUser(int userId)
        {
            try
            {
                var dt = new DataTable();
                var dao = new DaoInsUser(ref _dbManager);

                var ret = dao.GetByPK(ref dt, userId, true);
                if (ret > 0)
                {
                    return Utilities.NZ(dt.Rows[0]["session_id"]);
                }

                return string.Empty;
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
    }
}

