using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    
    public class Idioma
    {
        #region Propiedades Privadas
            private BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.Idioma idiomaDAL = null;
        #endregion

        public Idioma() { }

        public void Alta(BE.Idioma idioma)
        {
            try
            {
                if (idioma == null)
                    throw new ArgumentNullException("Por favor seleccione el idioma");

                idiomaDAL = new DAL.Idioma();
                idiomaDAL.Alta(idioma);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void Baja(BE.Idioma idioma)
        {
            try
            {
                idiomaDAL = new DAL.Idioma();
                idiomaDAL.Baja(idioma);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void Modificacion(BE.Idioma idioma)
        {
            try
            {
               idiomaDAL = new DAL.Idioma();
                idiomaDAL.Modificacion(idioma);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public BE.Idioma Obtener(BE.Idioma idioma)
        {
            idiomaDAL = new DAL.Idioma();
            try
            {
                return idiomaDAL.Obtener(idioma);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public List<BE.Idioma> Listar()
        {
           idiomaDAL = new DAL.Idioma();
            try
            {
             return idiomaDAL.Listar();
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
