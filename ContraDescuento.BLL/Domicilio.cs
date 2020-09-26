using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public  class Domicilio
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        #endregion

        public Domicilio() { }

        /// <summary>
        /// Alta de domicilio para comercio/usuario dependiendo los parámetros envíados.
        /// </summary>
        /// <param name="domicilio"></param>
        /// <param name="usuario"> Usuario != null; Domicilio de usuario</param>
        /// <param name="comercio">Usuario != null; Domicilio de comercio</param>
        public void Alta(ref BE.Domicilio domicilio,BE.Usuario usuario,BE.Comercio comercio)
        {
            try
            {
                DAL.Domicilio domicilioDAL = new DAL.Domicilio();
                domicilioDAL.Alta(ref domicilio,usuario,comercio);
            }
            catch(Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public BE.Domicilio Obtener(BE.Domicilio domicilio)
        {
            try
            {
                DAL.Domicilio domicilioDAL = new DAL.Domicilio();
                return domicilioDAL.Obtener(domicilio);
            }
            catch(Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
    }
    //public class DomicilioException : System.Exception
    //{
    //    private BE.Bitacora exception = null;
    //    public DomicilioException(bool ExControlada, Exception ex) : base(String.Format("Error: {0}", ex.Message))
    //    {
    //        exception = new BE.Bitacora(ex);
    //        exception.ExcepcionControlada = ExControlada;
    //    }

    //    public BE.Bitacora obtenerExcepcion()
    //    {
    //        return exception;
    //    }

    //}
}
