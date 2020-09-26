using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.BLL
{
    public class Comercio
    {
        #region Propiedades Privadas
        DAL.Bitacora exceptionLogger = new DAL.Bitacora();
        DAL.Comercio comercioDAL = new DAL.Comercio();
        #endregion

        public Comercio() {  }

        public void Alta(BE.Comercio comercio)
        {
            try
            {
                DAL.Usuario usuarioDAL = new DAL.Usuario();
                    comercioDAL.Alta(ref comercio);
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

        public void Baja(BE.Comercio comercio)
        {
            try
            {
                comercioDAL.Baja(comercio);
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
        public void Modificar(BE.Comercio comercio)
        {
            try
            {
                if (!comercio.MantenerLogo && (comercio.Logo == null || comercio.Logo.Length == 0))
                    throw new BE.ExcepcionPersonalizada(false, new Exception("Para modificar el comercio, debe incluir el logo"));
                if (comercio.NombreComercio.Length == 0)
                    throw new BE.ExcepcionPersonalizada(false, new Exception("Para modificar el comercio, debe incluir nombre del comercio"));
                if (comercio.Telefono == null)
                    throw new BE.ExcepcionPersonalizada(false, new Exception("Para modificar el comercio, debe incluir un teléfono de contacto"));

                BLL.Telefono telefonoNeg = new BLL.Telefono();
                telefonoNeg.Modificar(comercio.Telefono);
                DAL.Comercio comercioDAL = new DAL.Comercio();
                comercioDAL.Modificar(comercio);

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
        }

     
        public BE.Comercio Obtener(BE.Comercio comercio)
        {
            BE.Comercio _comercio = null;
            try
            {
                BLL.Telefono telefonoNeg = new Telefono();
                
                _comercio = comercioDAL.Obtener(comercio);
                _comercio.Telefono = telefonoNeg.Obtener(comercio.Telefono);
                
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                throw ex;
            }
            return _comercio;
        }
        public List<BE.Comercio> ListarProductosVigentes()
        {
            List<BE.Comercio> lstComercio = null;
            try
            {
                lstComercio = comercioDAL.ListarProductosVigentes();
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
            return lstComercio;
        }

        public List<BE.Comercio> ListarOfertasDelDia()
        {
            List<BE.Comercio> lstComercio = null;
            try
            {
                lstComercio = comercioDAL.ListarOfertasDelDia();
                
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
            return lstComercio;
        }

        public List<BE.Comercio> ListarProductosVigentesPorCategoriaSubCategoriaDescuento(BE.Categoria categoria, BE.SubCategoria subCategoria, decimal descuento)
        {
            List<BE.Comercio> lstComercio = null;
            try
            {
                lstComercio= comercioDAL.ListarProductosVigentesPorCategoriaSubCategoriaDescuento(categoria, subCategoria, descuento);
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
            return lstComercio;
        }

        public BE.Chart EstadisticaProductoMasCanjeado(BE.Comercio comercio)
        {
            BE.Chart chart = null;
            try
            {
                 chart = comercioDAL.EstadisticaProductoMasCanjeado(comercio);
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
            return chart;
        }
        public BE.Chart EstadisticaCantidadProductosPorPuntoVenta(BE.Comercio comercio)
        {
            BE.Chart chart = null;
            try
            {
                chart = comercioDAL.EstadisticaCantidadProductosPorPuntoVenta(comercio);
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
            return chart;
        }

        public BE.Chart EstadisticaCantidadCuponesCanjeadosPorFecha(BE.Comercio comercio)
        {
            BE.Chart chart = null;
            try
            {
                chart = comercioDAL.EstadisticaCantidadCuponesCanjeadosPorFecha(comercio);
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
            return chart;
        }

        public int EstadisticaCantidadPuntosDeVenta(BE.Comercio comercio)
        {
            int cantidadPuntosVenta = 0;
            try
            {
                cantidadPuntosVenta = comercioDAL.EstadisticaCantidadPuntosDeVenta(comercio);
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
            return cantidadPuntosVenta;
        }

        public int EstadisticaCantidadProductos(BE.Comercio comercio)
        {
            int cantidadProductos = 0;
            
            
            try
            {
                cantidadProductos = comercioDAL.EstadisticaCantidadProductos(comercio);
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
            return cantidadProductos;
        }

        public int EstadisticaCantidadTicketsPendientes(BE.Comercio comercio)
        {
            
            int cantidadTicketsPendientes = 0;
            try
            {
                cantidadTicketsPendientes = comercioDAL.EstadisticaCantidadTicketsPendientes(comercio);
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
            return cantidadTicketsPendientes;
        }
    }
}
