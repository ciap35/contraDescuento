using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class Provincia
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.Provincia provinciaDAL = null;
        #endregion
        public Provincia() { }

        public List<BE.Provincia> Listar()
        {
            try
            {
                provinciaDAL = new DAL.Provincia();
                return provinciaDAL.Listar();
            }
            catch(Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public BE.Provincia Obtener(BE.Provincia provincia)
        {
            try
            {
                provinciaDAL = new DAL.Provincia();
               return  provinciaDAL.Obtener(provincia);
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
