using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.DAL
{
    public class Telefono
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Telefono() { }

        public void Alta(BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TelefonoAlta";

                    conexion.sqlCmd.Parameters.AddWithValue("@celular", usuario.Telefono.Celular);
                    conexion.sqlCmd.Parameters.AddWithValue("@caracteristica", usuario.Telefono.Caracteristica);
                    conexion.sqlCmd.Parameters.AddWithValue("@nroTelefono", usuario.Telefono.NroTelefono);
                    conexion.sqlCmd.Parameters.AddWithValue("@observacion", usuario.Telefono.Observacion);
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", DBNull.Value);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while(conexion.sqlReader.Read())
                    {
                        usuario.Telefono.CodTelefono = Convert.ToInt32(conexion.sqlReader["codTelefono"]);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(BE.Telefono telefono)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "TelefonoModificar";

                    conexion.sqlCmd.Parameters.AddWithValue("@codTelefono", telefono.CodTelefono);
                    conexion.sqlCmd.Parameters.AddWithValue("@celular", telefono.Celular);
                    conexion.sqlCmd.Parameters.AddWithValue("@caracteristica", telefono.Caracteristica);
                    conexion.sqlCmd.Parameters.AddWithValue("@nroTelefono", telefono.NroTelefono);
                    conexion.sqlCmd.Parameters.AddWithValue("@observacion", telefono.Observacion);
                    //conexion.sqlCmd.Parameters.AddWithValue("@codComercio", DBNull.Value);

                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BE.Telefono Obtener(BE.Telefono telefono)
        {
            BE.Telefono _telefono = null;
            try
            {
                using (Acceso.Conexion Conexion = new Acceso.Conexion())
                {
                    Conexion.Abrir();
                    Conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    Conexion.sqlCmd.CommandText = "TelefonoObtener";
                    Conexion.sqlCmd.Parameters.AddWithValue("@codTelefono", telefono.CodTelefono);

                    Conexion.sqlReader = Conexion.sqlCmd.ExecuteReader();
                    while (Conexion.sqlReader.Read())
                    {
                        _telefono = new BE.Telefono();
                        _telefono.CodTelefono = telefono.CodTelefono;
                        _telefono.Celular = Convert.ToBoolean(Conexion.sqlReader["Celular"]);
                        _telefono.NroTelefono = Convert.ToString(Conexion.sqlReader["NroTelefono"]);
                        _telefono.Caracteristica = Convert.ToString(Conexion.sqlReader["Característica"]);
                        _telefono.Observacion = Convert.ToString(Conexion.sqlReader["Observacion"]);
                    }
                    Conexion.Cerrar();
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return _telefono;
        }
    }
}
