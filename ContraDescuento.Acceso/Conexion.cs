using Microsoft.Win32.SafeHandles;
using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ContraDescuento.Acceso
{
    public class Conexion : IDisposable
    {
        private SqlConnection ConnectionDB = null;
        public SqlCommand sqlCmd = new SqlCommand();
        public SqlDataReader sqlReader = null;
        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public Conexion()
        {
            ConnectionDB = new SqlConnection();
            ConnectionDB.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ContraDescuento"].ConnectionString;
        }

        public Conexion(string Key)
        {
            ConnectionDB = new SqlConnection();
            ConnectionDB.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[Key].ConnectionString;
        }

        public void Abrir()
        {
            try
            {
                if (ConnectionDB.State == System.Data.ConnectionState.Open)
                    ConnectionDB.Close();
                else if (ConnectionDB.State == System.Data.ConnectionState.Closed)
                {
                    ConnectionDB.Open();
                    sqlCmd.Connection = ConnectionDB;
                }
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Cerrar()
        {
            try
            {
                if (ConnectionDB.State != System.Data.ConnectionState.Closed)
                    ConnectionDB.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
    }
}
