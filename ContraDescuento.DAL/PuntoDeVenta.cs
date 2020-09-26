using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class PuntoDeVenta
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public PuntoDeVenta() { }

        public void Alta(BE.Domicilio PuntoDeVenta,BE.Comercio comercio,BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PuntoDeVentaAlta";

                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio); 
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", PuntoDeVenta.CodDomicilio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);

                    conexion.sqlCmd.ExecuteNonQuery();

                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Baja(BE.Domicilio PuntoDeVenta, BE.Comercio comercio, BE.Usuario usuario)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PuntoDeVentaBaja";

                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", PuntoDeVenta.CodDomicilio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);

                    conexion.sqlCmd.ExecuteNonQuery();

                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidarBaja(BE.Domicilio puntoDeVenta, BE.Comercio comercio)
        {
            bool operacionValida = false;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PuntoDeVentaValidarBaja";

                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", puntoDeVenta.CodDomicilio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                  
                    while (conexion.sqlReader.Read())
                    {
                        operacionValida = Convert.ToBoolean(conexion.sqlReader["Resultado"]);
                    }

                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operacionValida;
        }

        //public void Modificar(BE.Domicilio puntoDeVenta, BE.Comercio comercio, BE.Usuario usuario)
        //{
        //    throw new NotImplementedException();
        //}

        public List<BE.Domicilio> Obtener(BE.Comercio comercio)
        {
            List<BE.Domicilio> lstDomicilio = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PuntoDeVentaObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if(conexion.sqlReader.FieldCount > 0)
                        lstDomicilio = new List<BE.Domicilio>();

                    while (conexion.sqlReader.Read())
                    {
                        lstDomicilio.Add(new BE.Domicilio() { CodDomicilio = Convert.ToInt32(conexion.sqlReader["codDomicilio"]) });
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstDomicilio;
        }

        public List<BE.Domicilio> ObtenerPorProducto(BE.Producto producto)
        {
            List<BE.Domicilio> lstDomicilio = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "PuntoDeVentaObtenerPorProducto";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        lstDomicilio = new List<BE.Domicilio>();

                    while (conexion.sqlReader.Read())
                    {
                        BE.Domicilio dom = new BE.Domicilio() { CodDomicilio = Convert.ToInt32(conexion.sqlReader["codDomicilio"]) };
                        lstDomicilio.Add(dom);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstDomicilio;
        }
    }
}
