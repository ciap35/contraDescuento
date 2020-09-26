using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace ContraDescuento.DAL
{


    public class Sistema
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public bool ValidarExistenciaBaseDeDatos()
        {
            bool BaseExistente = false;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.Cerrar();
                    BaseExistente = true;
                };
            }
            catch (SqlException sqlEx)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BaseExistente;
        }

        public void CrearBaseDeDatos()
        {
            try
            {
                using (conexion = new Acceso.Conexion("ConnStrMaster"))
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.Text;
                    conexion.sqlCmd.CommandText = @"CREATE DATABASE ContraDescuento;"; //CREATE USER [NT AUTHORITY\SYSTEM] FOR LOGIN [NT AUTHORITY\SYSTEM];
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CargarParametria()
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "Sys_Cargar_Params";
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
