using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class Domicilio
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Domicilio() { }

        public void Alta(ref BE.Domicilio domicilio, BE.Usuario usuario, BE.Comercio comercio)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DomicilioAlta";
                    conexion.sqlCmd.Parameters.AddWithValue("@codLocalidad", domicilio.Local.CodLocalidad);
                    conexion.sqlCmd.Parameters.AddWithValue("@calle", domicilio.Calle);
                    conexion.sqlCmd.Parameters.AddWithValue("@numero", domicilio.Numero);
                    if(string.IsNullOrEmpty(domicilio.Piso))
                        conexion.sqlCmd.Parameters.AddWithValue("@piso", DBNull.Value);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@piso", domicilio.Piso);

                    if (string.IsNullOrEmpty(domicilio.Departamento))
                        conexion.sqlCmd.Parameters.AddWithValue("@departamento", DBNull.Value);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@departamento", domicilio.Departamento);

                    if (usuario == null)
                        conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", DBNull.Value);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);

                    if (comercio == null)
                        conexion.sqlCmd.Parameters.AddWithValue("@codComercio", DBNull.Value);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        domicilio.CodDomicilio = Convert.ToInt32(conexion.sqlReader["codDomicilio"]);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(BE.Domicilio domicilio)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DomicilioModificar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio",domicilio.CodDomicilio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codLocalidad", domicilio.Local.CodLocalidad);
                    conexion.sqlCmd.Parameters.AddWithValue("@calle", domicilio.Calle);
                    conexion.sqlCmd.Parameters.AddWithValue("@numero", domicilio.Numero);
                    conexion.sqlCmd.Parameters.AddWithValue("@piso", domicilio.Piso);
                    conexion.sqlCmd.Parameters.AddWithValue("@departamento", domicilio.Departamento);
                    conexion.sqlCmd.ExecuteNonQuery();
                    
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Baja(BE.Domicilio domicilio)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DomicilioBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", domicilio.CodDomicilio);
                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public BE.Domicilio Obtener(BE.Domicilio domicilio)
        {
            BE.Domicilio _domicilio = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DomicilioObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", domicilio.CodDomicilio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        _domicilio = new BE.Domicilio();
                        _domicilio.CodDomicilio = Convert.ToInt32(conexion.sqlReader["CodDomicilio"]);
                        _domicilio.Local.CodLocalidad = Convert.ToInt32(conexion.sqlReader["CodLocalidad"]);
                        _domicilio.Local.Descripcion = Convert.ToString(conexion.sqlReader["Localidad"]);
                        _domicilio.Provincia.CodProvincia = Convert.ToInt32(conexion.sqlReader["CodProvincia"]);
                        _domicilio.Provincia.Descripcion = Convert.ToString(conexion.sqlReader["Provincia"]);
                        _domicilio.Calle = Convert.ToString(conexion.sqlReader["Calle"]);
                        _domicilio.Departamento = Convert.ToString(conexion.sqlReader["Departarmento"]);
                        _domicilio.Numero = Convert.ToString(conexion.sqlReader["Numero"]);
                        _domicilio.Piso = Convert.ToString(conexion.sqlReader["Piso"]);
                        if (conexion.sqlReader["FechaCreacion"] != DBNull.Value)
                            _domicilio.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]);
                        if (conexion.sqlReader["FechaModificacion"] != DBNull.Value)
                            _domicilio.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["FechaModificacion"]);
                        if (conexion.sqlReader["FechaBaja"] != DBNull.Value)
                            _domicilio.FechaBaja = Convert.ToDateTime(conexion.sqlReader["FechaBaja"]);
                    }

                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _domicilio;
        }

        

    }
}
