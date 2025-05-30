using System.Diagnostics;
using Jpoc.Common;
using Jpoc.Dac;
using System.Text.Json;

namespace JPOC_VM_DEMO.App_Code.CTRL.DTO
{
    public class DtoSearch : AbstractDto, IDisposable
    {
        public override void Init()
        {
            try
            {
                
            }
            catch (ElsException ex)
            {
                ex.AddStackFrame(new StackFrame(true));
                throw;
            }
            catch (Exception ex)
            {
                throw new ElsException("Error initializing search DTO", ex);
            }
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
