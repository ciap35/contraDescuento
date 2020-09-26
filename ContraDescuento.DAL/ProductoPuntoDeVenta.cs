using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class ProductoPuntoDeVenta
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public ProductoPuntoDeVenta() { }

        public void Alta(BE.Producto producto,BE.Comercio comercio,BE.Domicilio puntoDeVenta,Int32 stock)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoPuntoDeVentaAlta";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", puntoDeVenta.CodDomicilio);
                    conexion.sqlCmd.Parameters.AddWithValue("@cantidad", stock);

                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Baja(BE.Producto producto, BE.Comercio comercio, BE.Domicilio puntoDeVenta)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoPuntoDeVentaBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", puntoDeVenta.CodDomicilio);

                    conexion.sqlCmd.ExecuteNonQuery();
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void Modificar(BE.Producto producto, BE.Domicilio domicilio,BE.Comercio comercio)
        //{
        //    try
        //    {
        //        using (conexion = new Acceso.Conexion())
        //        {
        //            conexion.Abrir();
        //            conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            conexion.sqlCmd.CommandText = "ProductoPuntoDeVentaModificar";
        //            conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
        //            conexion.sqlCmd.Parameters.AddWithValue("@codComercio", domicilio.CodDomicilio);
        //            conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
        //            conexion.sqlCmd.ExecuteNonQuery();
        //            conexion.Cerrar();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public BE.Producto Obtener(BE.Producto producto, BE.Comercio comercio, BE.Domicilio puntoDeVenta)
        {
            BE.Producto _producto = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoPuntoDeVentaObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", producto.PuntosDeVenta);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();
                    
                    while(conexion.sqlReader.Read())
                    {
                        _producto.CodProducto = producto.CodProducto;
                        _producto.Titulo = Convert.ToString(conexion.sqlReader["Producto.titulo"]);
                        _producto.Descripcion = Convert.ToString(conexion.sqlReader["Producto.descripcion"]);
                        _producto.Precio = Convert.ToDecimal(conexion.sqlReader["Producto.precio"]);
                        _producto.FechaVigenciaDesde = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaDesde"]);
                        _producto.FechaVigenciaHasta = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaHasta"]);
                        _producto.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["Producto.fechaCreacion"]);
                        _producto.FechaBaja = Convert.ToDateTime(conexion.sqlReader["Producto.fechaBaja"]);
                        _producto.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["Producto.fechaModificacion"]);
                        _producto.Foto = (byte[])(conexion.sqlReader["Producto.foto"]);
                        //_producto.p = Convert.ToInt32(conexion.sqlReader["Producto_PuntoDeVenta.Cantidad"]);
                        BE.Domicilio dom = new BE.Domicilio() { CodDomicilio = Convert.ToInt32(conexion.sqlReader["Domicilio.codDomicilio"]) };
                        _producto.PuntosDeVentaStock.Add(dom, Convert.ToInt32(conexion.sqlReader["Producto_PuntoDeVenta.Cantidad"]));
                        _producto.PuntosDeVenta.Add(dom);
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _producto;
        }

        public Dictionary<BE.Domicilio,int> ListarStockPorPuntoDeVenta(BE.Comercio comercio,BE.Domicilio dom,ref BE.Producto producto)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoPuntoDeVentaObtener";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
                    conexion.sqlCmd.Parameters.AddWithValue("@codDomicilio", dom.CodDomicilio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        producto.PuntosDeVentaStock.Clear();
                        //BE.Domicilio dom = new BE.Domicilio() { CodDomicilio = Convert.ToInt32(conexion.sqlReader["Domicilio.codDomicilio"]) };
                        producto.PuntosDeVentaStock.Add(dom, Convert.ToInt32(conexion.sqlReader["Producto_PuntoDeVenta.Cantidad"]));
                    }
                    conexion.Cerrar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return producto.PuntosDeVentaStock;
        }

        public bool ValidarBaja(BE.Producto producto, BE.Comercio comercio, BE.Domicilio puntoDeVenta)
        {
            bool operacionValida = false;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoPuntoDeVentaValidarBaja";

                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto);
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
    }
}
