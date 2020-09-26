using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.DAL
{
    public class Backup
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion
        public Backup() { }


        public void Generar(string fullPath, string nombreBackup)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "Backup";
                    conexion.sqlCmd.Parameters.AddWithValue("@nombreBackup", nombreBackup);
                    conexion.sqlCmd.Parameters.AddWithValue("@directorio", fullPath);
                    conexion.sqlCmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
