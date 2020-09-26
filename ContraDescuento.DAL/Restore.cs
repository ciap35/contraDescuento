using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.DAL
{
    public class Restore
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion
        public Restore() { }

        public void RealizarRestore(string path)
        {
            try
            {
                using (conexion = new Acceso.Conexion("ConnStrMaster"))
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.Text;
                    string pathRestore = System.Configuration.ConfigurationSettings.AppSettings["BD_Path_Restore"];
                    string fileBak = System.Configuration.ConfigurationSettings.AppSettings["BD_RestoreFile"];
                    conexion.sqlCmd.CommandText = "ALTER DATABASE [ContraDescuento] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
                        "USE [Master]  RESTORE DATABASE [ContraDescuento] FROM DISK ='" +pathRestore+fileBak + "'" + " WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 5;"+ @"  ALTER SERVER ROLE [sysadmin] ADD MEMBER [NT AUTHORITY\SYSTEM];";
                    conexion.sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
