using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.DAL
{
    public class Localidad
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Localidad() { }

        public List<BE.Localidad> ListarPorProvincia(ref BE.Provincia provincia)
        {
            List<BE.Localidad> lstLocalidad = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "LocalidadListarPorProvincia";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProvincia", provincia.CodProvincia);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        lstLocalidad = new List<BE.Localidad>();

                    while (conexion.sqlReader.Read())
                    {
                        
                        BE.Localidad localidad = new BE.Localidad();
                        localidad.CodLocalidad = Convert.ToInt32(conexion.sqlReader["codLocalidad"]);
                        localidad.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                        lstLocalidad.Add(localidad);
                    }
                    provincia.Local = lstLocalidad;
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstLocalidad;
        }

        public BE.Localidad Obtener(BE.Localidad localidad)
        {
            BE.Localidad _localidad = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "LocalidadObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codLocalidad", _localidad.CodLocalidad);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.FieldCount > 0)
                        _localidad = new BE.Localidad();

                    while (conexion.sqlReader.Read())
                    {

                        _localidad.CodLocalidad = Convert.ToInt32(conexion.sqlReader["codLocalidad"]);
                        _localidad.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _localidad;
        }
    }
}
