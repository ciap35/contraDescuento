using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class Producto
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.Producto productoDAL = new DAL.Producto();
        BLL.PuntoDeVenta puntoDeVentaNeg = new BLL.PuntoDeVenta();
        BLL.Domicilio domicilioNeg = new BLL.Domicilio();
        
        DAL.ProductoPuntoDeVenta productoPuntoDeVentaDAL = new DAL.ProductoPuntoDeVenta();
        #endregion

        public Producto() { }

        public void Alta(ref BE.Producto producto, BE.Comercio comercio)
        {
            try
            {
                if (producto.Titulo == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor complete el título del producto"));
                if (producto.FechaVigenciaDesde.Date < DateTime.Now.Date)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("La fecha de vigencia no puede ser anterior a la fecha actual"));
                if (producto.Descripcion == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor complete la descripción del producto"));
                if (producto.Precio == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor complete el precio del producto"));
                if (producto.Precio > 999999)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("El rango de precio del producto debe ser inferior a 999.999,99"));
                if (producto.Descuento > 100)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("El rango de precio del descuento debe ser inferior al 100%"));
                if (producto.PuntosDeVentaStock == null || producto.PuntosDeVentaStock.Count == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor, ingrese la cantidad de productos para los puntos de venta deseados"));
                if (producto.Foto == null)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor cargue la imagen del producto"));
                if (comercio == null || comercio.PuntoDeVenta == null)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor seleccione los puntos de venta"));

                if (producto.FechaVigenciaDesde.Date > producto.FechaVigenciaHasta.Date)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("La fecha de vigencia DESDE no puede ser mayor que la de HASTA"));


                productoDAL.Alta(ref producto);
                foreach (KeyValuePair<BE.Domicilio,int> dom in producto.PuntosDeVentaStock)
                {
                    if (dom.Value >0)
                        productoPuntoDeVentaDAL.Alta(producto, comercio, dom.Key,dom.Value);  //productoPuntoDeVentaDAL.Alta(producto, comercio, dom);
                    else
                        throw new BE.ExcepcionPersonalizada(false, new Exception("El stock a registrar para el punto de venta debe ser mayor a cero"));
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public void Modificar(BE.Producto producto, BE.Comercio comercio)
        {
            try
            {
                if (producto.Titulo == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor complete el título del producto"));

                if (producto.Descripcion == string.Empty)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor complete la descripción del producto"));
                if (producto.Precio == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor complete el precio del producto"));
                if (producto.Precio > 999999)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("El rango de precio del producto debe ser inferior a 999.999,99"));
                if (producto.Descuento > 100)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("El rango de precio del descuento debe ser inferior al 100%"));
                if (producto.PuntosDeVentaStock == null || producto.PuntosDeVentaStock.Count == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor, ingrese la cantidad de productos para los puntos de venta deseados"));
                if (producto.Foto == null && !producto.MantenerFoto)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor cargue la imagen del producto"));
                if (comercio == null || comercio.PuntoDeVenta == null)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor seleccione los puntos de venta"));

                if (Convert.ToDateTime(producto.FechaVigenciaDesde) > Convert.ToDateTime(producto.FechaVigenciaHasta))
                    throw new BE.ExcepcionPersonalizada(true, new Exception("La fecha de vigencia DESDE no puede ser mayor que la de HASTA"));

                productoDAL.Modificar(producto);
                List<BE.Domicilio> lstPuntosDeVentaAnteriores = puntoDeVentaNeg.ObtenerPuntosDeVenta(producto);

                foreach (BE.Domicilio dom in lstPuntosDeVentaAnteriores)
                {
                    productoPuntoDeVentaDAL.Baja(producto, comercio, dom);
                }
                foreach (KeyValuePair<BE.Domicilio, int> dom in producto.PuntosDeVentaStock)
                {
                    if (dom.Value > 0)
                        productoPuntoDeVentaDAL.Alta(producto, comercio, dom.Key, dom.Value);  //productoPuntoDeVentaDAL.Alta(producto, comercio, dom);
                    else
                        throw new BE.ExcepcionPersonalizada(false, new Exception("Para dar de alta el punto de venta del producto, indique el stock"));
                }
                //foreach (BE.Domicilio domNuevo in producto.PuntosDeVenta)
                //{
                //    productoPuntoDeVentaDAL.Alta(producto, comercio, domNuevo);
                //}
                }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public void BajaProductoPuntoDeVenta(BE.Producto producto, BE.Comercio comercio, BE.Domicilio puntoDeVenta)
        {
            try
            {
                if (productoPuntoDeVentaDAL.ValidarBaja(producto, comercio, puntoDeVenta))
                    productoPuntoDeVentaDAL.Baja(producto, comercio, puntoDeVenta);
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha podido dar de baja el producto, posee descuentos vigentes"));
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex,true);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public void Baja(BE.Producto producto, BE.Comercio comercio)
        {
            try
            {
                productoDAL.Baja(producto, comercio);
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public List<BE.Categoria> ListarCategoria()
        {
            try
            {
                return productoDAL.ListarCategoria();
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        public List<BE.SubCategoria> ListarSubCategoria(BE.Categoria categoria)
        {
            try
            {
                return productoDAL.ListarSubCategoria(categoria);
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }

        }
        public List<BE.Producto> Listar(BE.Comercio comercio)
        {
            List<BE.Producto> lstProducto = null;
            try
            {
                lstProducto = productoDAL.Listar(comercio);

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                exceptionLogger.Grabar(ex.obtenerExcepcion());
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstProducto;

        }
        public BE.Producto obtener(BE.Producto producto)
        {
            try
            {
                return productoDAL.Obtener(producto);
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora bitacora = new BE.Bitacora(ex);
                exceptionLogger.Grabar(bitacora);
                throw ex;
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
       
    }
}
