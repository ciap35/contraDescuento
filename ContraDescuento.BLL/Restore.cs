using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class Restore
    {
        #region Propiedades Privadas
        private BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        private DAL.Restore restore = null;
        #endregion

        public Restore() { }

        public void RealizarRestore(string path)
        {
            try
            {
                restore = new DAL.Restore();
                restore.RealizarRestore(path);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
    }
}
