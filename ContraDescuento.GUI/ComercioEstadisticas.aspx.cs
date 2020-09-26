using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class ComercioEstadistincas : System.Web.UI.Page
    {
        #region Propiedades Privadas
        static BE.Usuario usuario = null;
        static BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region WebMethods
        [System.Web.Services.WebMethod]
        public static string DescuentosPorProducto()
        {
            JArray array = new JArray();
            string listaProductos = string.Empty;
            try
            {
                usuario = (BE.Usuario)System.Web.HttpContext.Current.Session["Usuario"];
                if (usuario != null)
                {
                    BLL.Comercio comercioNeg = new BLL.Comercio();

                    BE.Chart chart = comercioNeg.EstadisticaProductoMasCanjeado(usuario.Comercio);
                    
                    foreach(BE.ChartItem cItem in chart.LstChartItems)
                    {
                        JObject obj = new JObject();
                        
                        JProperty jpLabel = new JProperty("etiqueta",cItem.Etiqueta);
                        obj.Add(jpLabel);
                        JProperty jpValue = new JProperty("valor",cItem.Valor);
                        obj.Add(jpValue);
                        array.Add(obj);
                        
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
            }
            return array.ToString();
        }


        [System.Web.Services.WebMethod]
        public static string CantidadProductosPorPuntoVenta()
        {
            JArray array = new JArray();
            string listaProductos = string.Empty;
            try
            {
                usuario = (BE.Usuario)System.Web.HttpContext.Current.Session["Usuario"];
                if (usuario != null)
                {
                    BLL.Comercio comercioNeg = new BLL.Comercio();

                    BE.Chart chart = comercioNeg.EstadisticaCantidadProductosPorPuntoVenta(usuario.Comercio);

                    foreach (BE.ChartItem cItem in chart.LstChartItems)
                    {
                        JObject obj = new JObject();

                        JProperty jpLabel = new JProperty("etiqueta", cItem.Etiqueta);
                        obj.Add(jpLabel);
                        JProperty jpValue = new JProperty("valor", cItem.Valor);
                        obj.Add(jpValue);
                        array.Add(obj);
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
            }
            return array.ToString();
        }


        [System.Web.Services.WebMethod]
        public static string EstadisticaCantidadCuponesCanjeadosPorFecha()
        {
            JArray array = new JArray();
            string listaProductos = string.Empty;
            try
            {
                usuario = (BE.Usuario)System.Web.HttpContext.Current.Session["Usuario"];
                if (usuario != null)
                {
                    BLL.Comercio comercioNeg = new BLL.Comercio();

                    BE.Chart chart = comercioNeg.EstadisticaCantidadCuponesCanjeadosPorFecha(usuario.Comercio);

                    foreach (BE.ChartItem cItem in chart.LstChartItems)
                    {
                        JObject obj = new JObject();

                        JProperty jpLabel = new JProperty("etiqueta", cItem.Etiqueta);
                        obj.Add(jpLabel);
                        JProperty jpValue = new JProperty("valor", cItem.Valor);
                        obj.Add(jpValue);
                        array.Add(obj);
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
            }
            return array.ToString();
        }
        #endregion
    }
}