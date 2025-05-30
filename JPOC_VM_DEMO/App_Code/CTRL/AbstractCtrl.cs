using Jpoc.Common;
using Jpoc.Dac;
using Jpoc.Tools.Core;
using System.Data;
using System.Diagnostics;

using Utilities = JPOC_VM_DEMO.Common.Utilities;
using GlobalVariables = JPOC_VM_DEMO.Common.GlobalVariables;
using AbstractDto = JPOC_VM_DEMO.App_Code.CTRL.DTO.AbstractDto;
using AbstractLogic = JPOC_VM_DEMO.App_Code.CTRL.Logic.AbstractLogic;
using PublicEnum = JPOC_VM_DEMO.Common.PublicEnum;
using JPOC_VM_DEMO.App_Code.CTRL.Logic;

namespace JPOC_VM_DEMO.App_Code.CTRL
{
    public abstract class AbstractCtrl : IDisposable, ICtrl
    {
        #region Fields
        protected readonly AbstractDto _dto;
        protected AbstractLogic _logic;
        private ElsDataBase _dbManager;
        private bool _disposedValue;
        #endregion

        #region Properties
        protected ElsDataBase DbManager => _dbManager ?? throw new InvalidOperationException("Database manager not initialized");
        protected AbstractDto Dto => _dto;
        protected AbstractLogic Logic => _logic;

        protected string ObjTypeName
        {
            get
            {
                var objName = GetType().ToString();
                var ns = Utilities.GetNameSpace(objName);
                return objName.Replace(ns, string.Empty)
                             .Replace(".Ctrl", string.Empty);
            }
        }
        #endregion

        #region Constructors
        private AbstractCtrl()
        {
            Init();
        }

        protected AbstractCtrl(AbstractDto dto) : this()
        {
            try
            {
                _dto = dto;
                _dbManager = new ElsDataBase(GlobalVariables.ConnectionString);

                // Case sensitive - maintain original behavior
                //var logicName = $"jpoc.Logic{ObjTypeName}";
                var logicName = $"JPOC_VM_DEMO.App_Code.CTRL.Logic.Logic{ObjTypeName}";
                var logicType = Type.GetType(logicName);

                if (logicType != null)
                {
                    var obj = Activator.CreateInstance(logicType, _dbManager, _dto);
                    if (obj != null)
                    {
                        _logic = (AbstractLogic)obj;
                    }
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
                    if (_dbManager != null)
                    {
                        if (_dbManager.State == ConnectionState.Connecting)
                        {
                            if (_dbManager.HasTransaction)
                            {
                                _dbManager.RollbackTransaction();
                            }
                            _dbManager.DisConnect();
                        }
                        _dbManager.Dispose();
                    }
                }

                _dbManager = null;
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
        public string GetSessionID(int userID, PublicEnum.eUserType userType)
        {
            try
            {
                if (userType == PublicEnum.eUserType.INT)
                {
                    return string.Empty;
                }

                DbManager.Connect();

                //TODO: FIX 

                //return userType == PublicEnum.UserType.INS
                //    ? Logic?.GetSessionIdFromInsUser(userID) ?? string.Empty
                //    : Logic?.GetSessionIdFromUser(userID) ?? string.Empty;
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
            finally
            {
                DbManager.DisConnect();
            }
        }
        #endregion
    }
}

