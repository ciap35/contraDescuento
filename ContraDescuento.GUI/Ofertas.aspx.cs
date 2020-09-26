using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ContraDescuento.GUI
{
    public partial class Ofertas : System.Web.UI.Page
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        BLL.Permiso permisoNeg = new BLL.Permiso();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                divMensajeInformativo.Visible = false;
                litMensajeInformativoOferta.Text = string.Empty;
                litMensajeInformativoOferta.Visible = false;
                ValidarPermisos();
                if (!IsPostBack)
                {

                    MostrarOferta();
                    Traducir();
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error: " + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error: " + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            divMensajeError.Visible = true;
            litMensajeError.Visible = true;
            litMensajeError.Text = mensaje.ToUpper();
        }

        private void MostrarMensajeOK(string mensaje)
        {
            divMensajeError.Visible = false;
            divMensajeOK.Visible = true;
            litMensajeOk.Visible = true;
            litMensajeOk.Text = mensaje.ToUpper();
        }

        private void ValidarPermisos()
        {
            try
            {
                string URL = this.Context.Request.CurrentExecutionFilePath.Replace("/", "");

                if (System.Web.HttpContext.Current.Session != null && HttpContext.Current.Session["Usuario"] != null)
                {
                    BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                    if (usuario.TipoUsuario != null && usuario.TipoUsuario.Grupo != null && usuario.TipoUsuario.Grupo.LstGrupos.Count > 0)
                    {
                        permisoNeg.ValidarSolicitud(URL, usuario.TipoUsuario.Grupo);
                        if (!permisoNeg.PeticionValida)
                        {
                            if (URL.ToUpper() == "MISDESCUENTOS.ASPX")
                            {
                                Response.Redirect("Index.aspx");
                            }

                            panelOferta.Visible = false;
                            panelSinStock.Visible = false;
                            OfertasPanel.Visible = false;
                            Response.ClearHeaders();
                            Response.AddHeader("REFRESH", "15;URL=Index.aspx");

                            MostrarError("No posee permisos para ingresar a esta página");

                        }

                        //if (usuario.TipoUsuario != null && usuario.TipoUsuario.Comercio)
                        //{
                        //    Response.Redirect("Index.aspx");
                        //}
                    }
                }
               

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al validar  los permisos: " + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al validar  los permisos: " + ex.Message);
            }
        }

        private void MostrarOferta()
        {
            try
            {
                BE.Comercio comercio = null;
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario != null)
                {

                    comercio = (BE.Comercio)Session["Oferta_" + Convert.ToString(usuario.CodUsuario)];
                    if (comercio != null)
                    {
                        BLL.PuntoDeVenta puntoDeVentaNeg = new BLL.PuntoDeVenta();
                        List<BE.Domicilio> lstPuntosDeVenta = puntoDeVentaNeg.ObtenerPuntosDeVenta(comercio.LstProducto[0]);
                        //List<BE.Domicilio> lstPuntosDeVenta = comercio.PuntoDeVenta;

                        BE.Producto prod = comercio.LstProducto[0];
                        bool SinStock = true;
                        foreach (BE.Domicilio dom in prod.PuntosDeVenta)
                        {
                            foreach (KeyValuePair<BE.Domicilio, Int32> stock in puntoDeVentaNeg.ListarStockPorPuntoDeVenta(comercio, dom, ref prod))
                            {
                                if (stock.Value > 0)
                                {

                                   // chkLstPuntosDeVentaDisponible.Items.Add(new ListItem(dom.ToString(), dom.CodDomicilio.ToString()));
                                    string domicilio = dom.Calle.ToString() + " " + dom.Numero.ToString();

                                    if (dom.Piso != string.Empty)
                                        domicilio += " - " + dom.Piso.ToString();
                                    if (dom.Departamento != string.Empty)
                                        domicilio += "" + dom.Departamento.ToString();

                                    chkLstPuntosDeVentaDisponible.Items.Add(new ListItem(domicilio, dom.CodDomicilio.ToString()));
                                    SinStock = false;
                                }

                            }

                        }
                        panelOferta.Visible = !SinStock;
                        if (SinStock)
                        {
                            panelSinStock.Visible = SinStock;
                        }
                        else
                        {
                            Session["PuntosDeVenta"] = lstPuntosDeVenta;
                            litTitulProducto.Text = comercio.LstProducto[0].Titulo;
                            litPorcentajeDescuentoProducto.Text = Convert.ToString(comercio.LstProducto[0].Descuento);
                            litPrecioAnteriorProducto.Text = Convert.ToString(comercio.LstProducto[0].Precio);
                            litPrecioActualProducto.Text = Convert.ToString(Math.Round(comercio.LstProducto[0].Precio - (comercio.LstProducto[0].Precio * (comercio.LstProducto[0].Descuento / 100)), 2));
                            if (comercio.LstProducto[0].Foto != null)
                            {
                                imgProducto.Src = "data:image/png;base64," + Convert.ToBase64String(comercio.LstProducto[0].Foto);
                            }
                            else
                            {
                                imgProducto.Src = @"~/Recurso/Imagen/empty.png";
                            }
                            litComercioNombre.Text = comercio.NombreComercio;
                            if (comercio.Logo != null)
                            {
                                imgComercio.Src = "data:image/png;base64," + Convert.ToBase64String(comercio.Logo);
                            }
                            else
                            {
                                imgComercio.Src = @"~/Recurso/Imagen/empty.png";
                            }


                        }
                    }
                }
                else
                {
                    Response.Redirect("Logueese.aspx");

                }

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar la oferta: " + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar la oferta: " + ex.Message);
            }
        }


        protected void btnCanjear_Click(object sender, EventArgs e)
        {
            try
            {
                divMensajeOK.Visible = false;
                divMensajeInformativo.Visible = false;
                divMensajeError.Visible = false;
                BE.Comercio comercio = null;
                BE.Usuario usuario = (BE.Usuario)Session["usuario"];
                if (usuario != null)
                {

                    comercio = (BE.Comercio)Session["Oferta_" + Convert.ToString(usuario.CodUsuario)];
                    if (comercio != null)
                    {
                        BE.Descuento CuponDescuento = new BE.Descuento();
                        CuponDescuento.Producto = comercio.LstProducto[0];

                        BLL.PuntoDeVenta puntoDeVentaNeg = new BLL.PuntoDeVenta();
                        List<BE.Domicilio> lstPuntosDeVenta = puntoDeVentaNeg.ObtenerPuntosDeVenta(comercio.LstProducto[0]);
                        BE.Producto prod = comercio.LstProducto[0];
                        ////foreach(BE.Domicilio dom in lstPuntosDeVenta)
                        ////{
                        //    puntoDeVentaNeg.ListarStockPorPuntoDeVenta(comercio, dom, ref prod);

                        //    //Elijo un punto de venta....
                        //}

                        foreach (ListItem item in chkLstPuntosDeVentaDisponible.Items)
                        {
                            if (item.Selected)
                            {
                                List<BE.Domicilio> ptoVentaLst = (List<BE.Domicilio>)Session["PuntosDeVenta"];
                                CuponDescuento.PuntoDeVenta = (from pto in ptoVentaLst where Convert.ToString(pto.CodDomicilio) == Convert.ToString(item.Value) select pto).FirstOrDefault<BE.Domicilio>();
                                //Obtengo el punto de venta...
                            }
                        }
                        if (CuponDescuento.PuntoDeVenta == null)
                            throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor seleccione un punto de venta para retirar"));
                        CuponDescuento.Comercio = comercio;
                        CuponDescuento.Usuario = usuario;
                        CuponDescuento.Cantidad = Convert.ToInt32(txtProductoCantidad.Text);
                        BLL.Descuento descuentoNegocio = new BLL.Descuento();
                        if (descuentoNegocio.ValidarStockPuntoDeVenta(CuponDescuento) < Convert.ToInt32(txtProductoCantidad.Text))
                        {
                            divMensajeInformativo.Visible = true;
                            litMensajeInformativoOferta.Text = "No hay stock suficiente para la cantidad solicitada para el punto de venta, seleccione otro punto de venta o una cantidad inferior";
                            litMensajeInformativoOferta.Visible = true;
                        }
                        else
                        {
                            descuentoNegocio.GenerarDescuento(CuponDescuento);
                            MostrarMensajeOK("¡Se ha generado su cupón de descuento éxitosamente!");
                        }


                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                divMensajeOK.Visible = false;
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al canjear la oferta: " + ex.Message);
            }
            catch (Exception ex)
            {
                divMensajeOK.Visible = false;
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al canjear la oferta: " + ex.Message);
            }
        }


        private void Traducir()
        {
            BE.Idioma idiomaUsuario = null;
            try
            {
                if ((BE.Idioma)Session["Idioma"] != null)
                    idiomaUsuario = (BE.Idioma)Session["Idioma"];

                if (idiomaUsuario != null)
                {
                    object ctllitTituloOfertas = (object)litTituloOfertas;
                    Traducir(idiomaUsuario, ref ctllitTituloOfertas);

                    object ctllitPrecioAnterior = (object)litPrecioAnterior;
                    Traducir(idiomaUsuario, ref ctllitPrecioAnterior);



                    object ctllitPorcentaje = (object)litPorcentaje;
                    Traducir(idiomaUsuario, ref ctllitPorcentaje);

                    object ctllitPrecioFinal = (object)litPrecioFinal;
                    Traducir(idiomaUsuario, ref ctllitPrecioFinal);

                    object ctllitPuntoDeVentaDisponibleTitulo = (object)litPuntoDeVentaDisponibleTitulo;
                    Traducir(idiomaUsuario, ref ctllitPuntoDeVentaDisponibleTitulo);

                    object ctllitABMCantidadTitulo = (object)litABMCantidadTitulo;
                    Traducir(idiomaUsuario, ref ctllitABMCantidadTitulo);

                    object ctlbtnCanjear = (object)btnCanjear;
                    Traducir(idiomaUsuario, ref ctlbtnCanjear);
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar la oferta: " + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al mostrar la oferta: " + ex.Message);
            }
        }


        private void Traducir(BE.Idioma idioma, ref object ControlATraducir)
        {
            try
            {

                BLL.Traductor traduccionNeg = new BLL.Traductor();
                BE.Traductor traductor = new BE.Traductor();
                traductor.Idioma = idioma;
                lstTraduccion.Clear();
                if (idioma.Descripcion == string.Empty)
                {
                    BLL.Idioma IdiomaNeg = new BLL.Idioma();
                    idioma = IdiomaNeg.Obtener(idioma);
                }

                foreach (BE.Traductor traduc in traduccionNeg.ListarTraducciones(traductor))
                {
                    lstTraduccion.Add(traduc.Traduccion);
                }
                if (lstTraduccion.Count > 0)
                {
                    TraducirControles(lstTraduccion, ref ControlATraducir);
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al traducir: " + ex.Message);
            }
        }

        private void TraducirControles(List<BE.Traduccion> traducciones, ref object ControlATraducir)
        {
            try
            {
                foreach (BE.Traduccion traduccion in traducciones)
                {
                    if (ControlATraducir != null)
                    {

                        if (ControlATraducir.GetType() == typeof(System.Web.UI.HtmlControls.HtmlAnchor))
                        {
                            System.Web.UI.HtmlControls.HtmlAnchor ctrlPage = (System.Web.UI.HtmlControls.HtmlAnchor)ControlATraducir;
                            if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                ctrlPage.InnerText = traduccion.Texto;
                        }
                        if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.Literal))
                        {
                            System.Web.UI.WebControls.Literal ctrlPage = (System.Web.UI.WebControls.Literal)ControlATraducir;
                            if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                ctrlPage.Text = traduccion.Texto;
                        }
                        if (ControlATraducir.GetType() == typeof(System.Web.UI.HtmlControls.HtmlGenericControl))
                        {
                            System.Web.UI.HtmlControls.HtmlGenericControl ctrlPage = (System.Web.UI.HtmlControls.HtmlGenericControl)ControlATraducir;
                            if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                ctrlPage.InnerText = traduccion.Texto;
                        }
                        if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.Button))
                        {
                            System.Web.UI.WebControls.Button ctrlPage = (System.Web.UI.WebControls.Button)ControlATraducir;
                            if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                ctrlPage.Text = traduccion.Texto;
                        }
                        if (ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.LinkButton))
                        {
                            System.Web.UI.WebControls.LinkButton ctrlPage = (System.Web.UI.WebControls.LinkButton)ControlATraducir;
                            if (ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
                                ctrlPage.Text = traduccion.Texto;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
            }
        }
    }
}