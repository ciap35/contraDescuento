using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{

    public class Telefono
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        #endregion

        public Telefono() { }

        public void Alta(ref BE.Usuario usuario)
        {
            try
            {
                DAL.Telefono telefonoDAL = new DAL.Telefono();
                if (usuario.Telefono != null)
                    telefonoDAL.Alta(usuario);
                else
                    throw new TelefonoNegocioException(true, new Exception("Por favor complete el teléfono"));
            }
            catch(TelefonoNegocioException ex)
            {
                exceptionLogger.Grabar(ex.obtenerExcepcion());
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void Modificar(BE.Telefono telefono)
        {
            try
            {
                DAL.Telefono telefonoDAL = new DAL.Telefono();
                telefonoDAL.Modificar(telefono);
            }
            catch(Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public BE.Telefono Obtener(BE.Telefono telefono)
        {
            try
            {
                DAL.Telefono telefonoDAL = new DAL.Telefono();
                return telefonoDAL.Obtener(telefono);
            }
            catch(Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
    }

    public class TelefonoNegocioException : System.Exception
    {
        private BE.Bitacora exception = null;
        public TelefonoNegocioException(bool ExControlada, Exception ex) : base(String.Format("Error: {0}", ex.Message))
        {
            exception = new BE.Bitacora(ex);
            exception.ExcepcionControlada = ExControlada;
        }

        public BE.Bitacora obtenerExcepcion()
        {
            return exception;
        }

    }
}
