using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class Traduccion
    {
        #region Propiedades Privadas
        private DAL.Bitacora exceptionLogger = new DAL.Bitacora();
        #endregion

        public Traduccion() { }

        public void Alta(BE.Traductor traductor)
        {
            try
            {
                DAL.Traductor traductorDAL = new DAL.Traductor();
                traductorDAL.Alta(traductor);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void Baja(BE.Traductor traductor)
        {
            try
            {
                DAL.Traductor traductorDAL = new DAL.Traductor();
                traductorDAL.Baja(traductor);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void Modificacion(BE.Traductor traductor)
        {
            try
            {
                DAL.Traductor traductorDAL = new DAL.Traductor();
                traductorDAL.Modificacion(traductor);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public BE.Traductor Obtener(BE.Traductor traductor)
        {
            try
            {
                DAL.Traductor traductorDAL = new DAL.Traductor();
                return traductorDAL.Obtener(traductor);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public List<BE.Traductor>  ListarTraducciones(BE.Traductor traductor)
        {
            try
            {
                DAL.Traductor traductorDAL = new DAL.Traductor();
                return traductorDAL.Listar(traductor);
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
