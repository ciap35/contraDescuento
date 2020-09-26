using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.BLL
{
    public class Descuento
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        DAL.Descuento descuentoDAL = new DAL.Descuento();
        #endregion

        public Descuento() { }

        public Int32 ValidarStockPuntoDeVenta(BE.Descuento descuento)
        {
            try
            {
                return descuentoDAL.ValidarStockPuntoDeVenta(descuento.Producto, descuento.PuntoDeVenta);
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
        public void GenerarDescuento(BE.Descuento descuento)
        {
            try
            {
                if (ValidarStockPuntoDeVenta(descuento) >= descuento.Cantidad)
                {
                    descuento.AhorroTotal = Convert.ToDecimal(Math.Round((descuento.Producto.Precio * (descuento.Producto.Descuento / 100)), 2));
                    TFL.Hash hash = new TFL.Hash();
                    descuento.Cupon = hash.Encrypt(descuento.Usuario.CodUsuario.ToString() + descuento.Producto.CodProducto.ToString() + descuento.PuntoDeVenta.CodDomicilio.ToString());
                    descuentoDAL.GenerarCupon(descuento);
                }
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se encuentra stock disponible para la sucursal a retirar"));

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

        public List<BE.Descuento> ObtenerDescuentos(BE.Usuario usuario)
        {
            List<BE.Descuento> descuentos = null;
            try
            {
                descuentos = descuentoDAL.ObtenerDescuentos(usuario, true, false, string.Empty);

                BLL.Comercio comercioNeg = new Comercio();
                BLL.Producto productoNeg = new Producto();
                BLL.Domicilio domicilioNeg = new Domicilio();
                foreach (BE.Descuento desc in descuentos)
                {
                    if (desc.Producto != null)
                        desc.Producto = productoNeg.obtener(desc.Producto);
                    if (desc.Comercio != null)
                        desc.Comercio = comercioNeg.Obtener(desc.Comercio);
                    if (desc.PuntoDeVenta != null)
                        desc.PuntoDeVenta = domicilioNeg.Obtener(desc.PuntoDeVenta);
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
            return descuentos;
        }

        public List<BE.Descuento> ObtenerDescuentos(BE.Comercio comercio)
        {
            List<BE.Descuento> descuentos = null;
            try
            {
                descuentos = descuentoDAL.ObtenerDescuentos(comercio);

                BLL.Usuario usuarioNeg = new Usuario();
                BLL.Producto productoNeg = new Producto();
                BLL.Domicilio domicilioNeg = new Domicilio();
                foreach (BE.Descuento desc in descuentos)
                {
                    if (desc.Usuario != null)
                    {
                        desc.Usuario = usuarioNeg.Obtener(desc.Usuario);
                    }
                    if (desc.Producto != null)
                        desc.Producto = productoNeg.obtener(desc.Producto);
                    if (desc.Comercio != null)
                        desc.Comercio = comercio;
                    if (desc.PuntoDeVenta != null)
                        desc.PuntoDeVenta = domicilioNeg.Obtener(desc.PuntoDeVenta);
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
            return descuentos;
        }

        public void Confirmar(BE.Descuento descuento)
        {
            try
            {
                if (descuento.CodCupon == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("El código de cupón es inválido"));
                if (descuento.Usuario ==null || descuento.Comercio == null || descuento.Comercio.CodComercio == 0 || descuento.Producto == null || descuento.Producto.CodProducto == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("El cupón es inválido"));

                descuentoDAL.Confirmar(descuento);
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

        public void Cancelar(BE.Descuento descuento)
        {
            try
            {
                if (descuento.CodCupon == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("El código de cupón es inválido"));
                if (descuento.Usuario == null || descuento.Comercio == null || descuento.Comercio.CodComercio == 0 || descuento.Producto == null || descuento.Producto.CodProducto == 0)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("El cupón es inválido"));

                descuentoDAL.Cancelar(descuento);
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
    }
}
