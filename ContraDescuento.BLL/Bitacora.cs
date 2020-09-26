using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ContraDescuento.BLL
{
    public class Bitacora
    {
        DAL.Bitacora exceptionLogger = new DAL.Bitacora();
        public Bitacora() { }

        public void Grabar(BE.Bitacora exception)
        {
            try
            {
                exceptionLogger.Grabar(exception);
            }
            catch (Exception ex)
            {
                exceptionLogger.GrabarEnArchivo(ex);
            }
        }

        /// <summary>
        /// Muestra todos los mensajes en bitacora
        /// </summary>
        public List<BE.Bitacora> Listar()
        {
            try
            {
                return exceptionLogger.Listar();
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
