using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class Localidad
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.Localidad localidadDAL = null;
        #endregion
        public Localidad() { }

        public List<BE.Localidad> ListarPorProvincia(ref BE.Provincia provincia)
        {
            try
            {
                localidadDAL = new DAL.Localidad();
                return localidadDAL.ListarPorProvincia(ref provincia);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public BE.Localidad Obtener(BE.Localidad localidad)
        {
            try
            {
                localidadDAL = new DAL.Localidad();
               return localidadDAL.Obtener(localidad);
            }
            catch(Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
    }
}
