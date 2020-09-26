using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
namespace ContraDescuento.BLL
{

    public class Backup
    {
        #region Propiedades Privadas
        private BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        private DAL.Backup backup = null;
        #endregion

        public Backup() { }


        public void GenerarBackup()
        {
            try
            {
                backup = new DAL.Backup();
                if (!Directory.Exists(ConfigurationSettings.AppSettings["Path.Backup"]))
                    Directory.CreateDirectory(ConfigurationSettings.AppSettings["Path.Backup"]);
                string fileName = "ContraDescuento_Backup_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss")+ ".bak";
                string fullPath = ConfigurationSettings.AppSettings["Path.Backup"] + fileName ;
                BLL.DigitoVerificador digVerif = new BLL.DigitoVerificador();
                List<BE.IAuditable> lstRegCorruptos = digVerif.ValidarDigitosVerificadores();
                if(lstRegCorruptos.Count > 0)
                {
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se puede realizar el backup en este momento, base de datos corrupta"));
                }
                else { 
                backup.Generar(fullPath, fileName);
                }

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                exceptionLogger.Grabar(ex.obtenerExcepcion());
                throw ex;
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
