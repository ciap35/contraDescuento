using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraDescuento.BLL
{
    public class PuntoDeVenta
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        #endregion
        public PuntoDeVenta() { }

        public void Alta(BE.Domicilio PuntoDeVenta, BE.Comercio comercio, BE.Usuario usuario)
        {
            try
            {
                if (PuntoDeVenta.Local != null)
                {
                    BLL.Domicilio domicilioNeg = new BLL.Domicilio();
                    domicilioNeg.Alta(ref PuntoDeVenta, null, comercio);
                    DAL.PuntoDeVenta puntoDeVentaDAL = new DAL.PuntoDeVenta();
                    puntoDeVentaDAL.Alta(PuntoDeVenta, comercio, usuario);
                }
                else
                    throw new Exception("Por favor, seleccione una localidad");
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void Baja(BE.Domicilio PuntoDeVenta, BE.Comercio comercio, BE.Usuario usuario)
        {
            try
            {
                DAL.PuntoDeVenta puntoDeVentaDAL = new DAL.PuntoDeVenta();
                if(puntoDeVentaDAL.ValidarBaja(PuntoDeVenta, comercio))
                puntoDeVentaDAL.Baja(PuntoDeVenta, comercio, usuario);
                else
                    throw new Exception("No se puede dar de baja el punto de venta ya que posee productos asignados.");

            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }

        public void Modificar(BE.Domicilio PuntoDeVenta)
        {
            try
            {
                DAL.Domicilio domicilioDAL = new DAL.Domicilio();
                domicilioDAL.Modificar(PuntoDeVenta);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
        }
        /// <summary>
        /// Devuelve los códigos de los punto de venta asociados a un comercio
        /// </summary>
        /// <param name="comercio"></param>
        /// <returns></returns>
        public List<BE.Domicilio> ObtenerPuntosDeVenta(BE.Comercio comercio)
        {
            List<BE.Domicilio> lstPuntosVenta = null;
            try
            {
                if (comercio.PuntoDeVenta.Count == 0)
                    lstPuntosVenta = new List<BE.Domicilio>();

                DAL.PuntoDeVenta PuntoDeVentaDAL = new DAL.PuntoDeVenta();
                return PuntoDeVentaDAL.Obtener(comercio);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstPuntosVenta;
        }

        public List<BE.Domicilio> ObtenerPuntosDeVenta(BE.Producto producto)
        {
            List<BE.Domicilio> lstPuntosVenta = lstPuntosVenta = new List<BE.Domicilio>();
            try
            {
                DAL.PuntoDeVenta PuntoDeVentaDAL = new DAL.PuntoDeVenta();
                BLL.Domicilio domicilioNeg = new Domicilio();
                
                foreach(BE.Domicilio dom in PuntoDeVentaDAL.ObtenerPorProducto(producto))
                {
                    lstPuntosVenta.Add(domicilioNeg.Obtener(dom));
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstPuntosVenta;
        }

        public List<BE.Domicilio> Listar(BE.Comercio comercio)
        {
            List<BE.Domicilio> lstPuntosVenta = null;
            try
            {
                if (comercio.PuntoDeVenta.Count == 0)
                    lstPuntosVenta = new List<BE.Domicilio>();


                DAL.Domicilio domicilioDAL = new DAL.Domicilio();
                foreach (BE.Domicilio puntoVenta in ObtenerPuntosDeVenta(comercio))
                {
                    lstPuntosVenta.Add(domicilioDAL.Obtener(puntoVenta));
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return lstPuntosVenta;
        }

        public Dictionary<BE.Domicilio,int> ListarStockPorPuntoDeVenta(BE.Comercio comercio,BE.Domicilio dom,ref BE.Producto producto)
        {
            try
            {
                Dictionary<BE.Domicilio, int> puntosDeVenta = new Dictionary<BE.Domicilio, int>();
                DAL.ProductoPuntoDeVenta productoPuntoDeVentaDAL = new DAL.ProductoPuntoDeVenta();
                productoPuntoDeVentaDAL.ListarStockPorPuntoDeVenta(comercio,dom, ref producto);

            }
            catch(BE.ExcepcionPersonalizada ex)
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
            return producto.PuntosDeVentaStock;
        }

        //public Dictionary<BE.Domicilio, int> ListarStockPorProducto(BE.Comercio comercio, ref BE.Producto producto)
        //{
        //    try
        //    {
        //        Dictionary<BE.Domicilio, int> puntosDeVenta = new Dictionary<BE.Domicilio, int>();
        //        DAL.ProductoPuntoDeVenta productoPuntoDeVentaDAL = new DAL.ProductoPuntoDeVenta();
        //        productoPuntoDeVentaDAL.ListarStockPorProducto(comercio, ref producto);

        //    }
        //    catch (BE.ExcepcionPersonalizada ex)
        //    {
        //        BE.Bitacora exception = new BE.Bitacora(ex, true);
        //        exceptionLogger.Grabar(exception);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BE.Bitacora exception = new BE.Bitacora(ex);
        //        exceptionLogger.Grabar(exception);
        //        throw ex;
        //    }
        //    return producto.PuntosDeVentaStock;
        //}
    }


    //public class PuntoDeVentaException : System.Exception
    //{
    //    private BE.Bitacora exception = null;
    //    public PuntoDeVentaException(bool ExControlada, Exception ex) : base(String.Format("Error: {0}", ex.Message))
    //    {
    //        exception = new BE.Bitacora(ex);
    //        exception.ExcepcionControlada = ExControlada;
    //    }

    //    public BE.Bitacora obtenerExcepcion()
    //    {
    //        return exception;
    //    }

    //}
}
