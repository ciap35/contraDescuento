using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class Sistema
    {
        #region Propiedades Privadas
        private BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        #endregion

        public Sistema() { }

        #region "Base de datos"
        public bool ValidarExistenciaBaseDeDatos()
        {
            try
            {
                DAL.Sistema sistema = new DAL.Sistema();
                return sistema.ValidarExistenciaBaseDeDatos();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public void CrearBaseDeDatos()
        {
            try
            {
                string pathRestore = System.Configuration.ConfigurationSettings.AppSettings["BD_Path_Restore"];
                string fileBak = System.Configuration.ConfigurationSettings.AppSettings["BD_RestoreFile"];

                //Crear BD
                DAL.Sistema sistema = new DAL.Sistema();
                sistema.CrearBaseDeDatos();

                //Restaurar BD
                BLL.Restore restore = new BLL.Restore();
                restore.RealizarRestore(pathRestore+fileBak);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void CargarParametria()
        {
            try
            {
                DAL.Sistema sistema = new DAL.Sistema();
                sistema.CargarParametria();
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        #endregion
    }
}
