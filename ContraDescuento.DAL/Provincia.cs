using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.DAL
{
    public class Provincia
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion
        public Provincia() { }

        public List<BE.Provincia> Listar()
        {
            List<BE.Provincia> lstProvincia = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProvinciaListar";

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        lstProvincia = new List<BE.Provincia>();

                    while (conexion.sqlReader.Read())
                    {
                        BE.Provincia provincia = new BE.Provincia();
                        provincia.CodProvincia = Convert.ToInt32(conexion.sqlReader["codProvincia"]);
                        provincia.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                        lstProvincia.Add(provincia);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstProvincia;
        }

        public BE.Provincia Obtener(BE.Provincia provincia)
        {
            BE.Provincia _provincia = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProvinciaObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProvincia", provincia.CodProvincia);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    if (conexion.sqlReader.FieldCount > 0)
                        _provincia = new BE.Provincia();

                    while (conexion.sqlReader.Read())
                    {

                        provincia.CodProvincia = Convert.ToInt32(conexion.sqlReader["codProvincia"]);
                        provincia.Descripcion = Convert.ToString(conexion.sqlReader["descripcion"]);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _provincia;
        }
    }
}
