using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ContraDescuento.BE;
using ContraDescuento.BLL;

namespace ContraDescuento.GUI
{
    public partial class Index : System.Web.UI.Page
    {
        #region Propiedades Privadas
        BLL.Bitacora exceptionLogger = new BLL.Bitacora();
        BLL.Permiso permisoNeg = new BLL.Permiso();
        List<BE.Traduccion> lstTraduccion = new List<BE.Traduccion>();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            divMensajeError.Visible = false;
            divMensajeOK.Visible = false;
            try
            {
                ObtenerIdiomaSeleccionado();
                ValidarPermisos();

                //BindOfertasDelDia(null);
                //BindOfertasVigentes(null);

                if (!IsPostBack)
                {
                    BindCategoria();
                    
                    MostrarProductosVigentes();
                    MostrarOfertasDelDia();
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


        private string TraducirPalabra(string palabra, string clave)
        {
            #region traducir texto
            string resultado = string.Empty;
            BE.Idioma idiomaUsuario = null;
            string traduccionResultado = palabra;
            if ((BE.Idioma)Session["Idioma"] != null)
            {
                idiomaUsuario = (BE.Idioma)Session["Idioma"];
                #endregion



                try
                {
                    BLL.Traductor traduccionNeg = new BLL.Traductor();
                    BE.Traductor traductor = new BE.Traductor();
                    traductor.Idioma = idiomaUsuario;
                    lstTraduccion.Clear();
                    if (idiomaUsuario.Descripcion == string.Empty)
                    {
                        BLL.Idioma IdiomaNeg = new BLL.Idioma();
                        idiomaUsuario = IdiomaNeg.Obtener(idiomaUsuario);
                    }

                    foreach (BE.Traductor traduc in traduccionNeg.ListarTraducciones(traductor))
                    {
                        lstTraduccion.Add(traduc.Traduccion);
                    }
                    if (lstTraduccion.Count > 0)
                    {
                        foreach (BE.Traduccion traduccion in lstTraduccion)
                        {
                            string traduccionTexto = traduccion.ControlID.ToString().Replace(" ", "").ToUpper();
                            clave = clave.Replace(" ", "").ToUpper();
                            if (traduccionTexto == clave && traduccion.Texto.ToUpper().Replace(" ", "") != "LOREMIPSUM")
                            {
                                traduccionResultado = traduccion.Texto;
                            }
                        }
                    }
                    return traduccionResultado;
                }
                catch (Exception ex)
                {
                    BE.Bitacora exception = new BE.Bitacora(ex, true);
                    exceptionLogger.Grabar(exception);

                    MostrarError(TraducirPalabra("Ocurrió un error al traducir la página: ", "Ocurrió un error al traducir la página: ") + ex.Message);
                    //MostrarError("Ocurrió un error al traducir la página: " + ex.Message);
                }
            }
            return traduccionResultado;
        }

        private void BindCategoria()
        {
            try
            {
                BLL.Producto productoNeg = new BLL.Producto();
                ddlCategoria.DataSource = null;
                Session["Categoria"] = productoNeg.ListarCategoria();
                ddlCategoria.DataSource =  (List<BE.Categoria>)Session["Categoria"];
                ddlCategoria.DataBind();
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
               
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }

        private void BindSubCategoria(BE.Categoria categoria)
        {
            try
            {
                BLL.Producto productoNeg = new BLL.Producto();
                ddlSubCategoria.DataSource = null;
                Session["SubCategoria"] = productoNeg.ListarSubCategoria(categoria);
                if (ddlCategoria.SelectedValue != string.Empty)
                {
                    pnSubCategoria.Visible = true;
                    ddlSubCategoria.DataSource = (List<BE.SubCategoria>)Session["SubCategoria"];
                    ddlSubCategoria.DataBind();
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
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
                        if (!permisoNeg.PeticionValida && URL.ToUpper() != "INDEX.ASPX")
                        {
                            Response.ClearHeaders();
                            Response.AddHeader("REFRESH", "5;URL=Index.aspx");
                        }
                    }
                }

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error al validar  los permisos: ", "Ocurrió un error al validar  los permisos: ") + ex.Message);
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


        private void MostrarProductosVigentes()
        {
            try
            {
                BLL.Comercio comercio = new BLL.Comercio();
                List<BE.Comercio> lstComercio = comercio.ListarProductosVigentes();
                BindOfertasVigentes(lstComercio);
                
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }

        private void MostrarOfertasDelDia()
        {
            try
            {
                BLL.Comercio comercio = new BLL.Comercio();
                List<BE.Comercio> lstComercio = comercio.ListarOfertasDelDia();
                BindOfertasDelDia(lstComercio);
                
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }



        private void BindOfertasVigentes(List<BE.Comercio> lstComercio)
        {
            try
            {
                rptProductos.DataSource = null;

                if (lstComercio != null && lstComercio.Count > 0) {
                    Session["OfertasVigentes"] = lstComercio;
                }
                else
                {
                    lstComercio = (List<BE.Comercio>)Session["OfertasVigentes"];
                }
                List<BE.Comercio> productosNoRepetidos = new List<BE.Comercio>();

                foreach(BE.Comercio com in lstComercio)
                {
                    bool existe = false;
                        foreach(BE.Comercio comercio in productosNoRepetidos)
                        {
                        if (comercio.LstProducto[0].CodProducto == com.LstProducto[0].CodProducto)
                            existe = true;
                        }
                    if (!existe)
                        productosNoRepetidos.Add(com);
                }
                rptProductos.DataSource = productosNoRepetidos;
                rptProductos.DataBind();
                
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error la mostrar los productos vigentes: ", "Ocurrió un error la mostrar los productos vigentes: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error la mostrar los productos vigentes: ", "Ocurrió un error la mostrar los productos vigentes: ") + ex.Message);
            }
        }

        private void BindOfertasDelDia(List<BE.Comercio> lstComercio)
        {
            try
            {
                rptOfertaDelDia.DataSource = null;

                if (lstComercio != null && lstComercio.Count > 0)
                {
                    Session["OfertasDelDia"] = lstComercio;
                }
                else
                {
                    lstComercio = (List<BE.Comercio>)Session["OfertasDelDia"];
                }

                    List<BE.Comercio> productosNoRepetidos = new List<BE.Comercio>();

                    foreach (BE.Comercio com in lstComercio)
                    {
                        bool existe = false;
                        foreach (BE.Comercio comercio in productosNoRepetidos)
                        {
                            if (comercio.LstProducto[0].CodProducto == com.LstProducto[0].CodProducto)
                                existe = true;
                        }
                        if (!existe)
                            productosNoRepetidos.Add(com);
                    }
                    rptOfertaDelDia.DataSource = productosNoRepetidos;
                    rptOfertaDelDia.DataBind();
                
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

        protected void rptProductos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.DataItem != null)
                {
                    BE.Comercio comercio = (BE.Comercio)e.Item.DataItem;
                    System.Web.UI.HtmlControls.HtmlImage image = (System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgProducto");
                    System.Web.UI.HtmlControls.HtmlImage imageComercio = (System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgComercio");
                    System.Web.UI.WebControls.Literal litTitulProducto = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litTitulProducto");

                    System.Web.UI.WebControls.Literal litPorcentajeDescuentoProducto = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPorcentajeDescuentoProducto");
                    System.Web.UI.WebControls.Literal litPrecioAnteriorProducto = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPrecioAnteriorProducto");
                    System.Web.UI.WebControls.Literal litPrecioActualProducto = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPrecioActualProducto");
                    System.Web.UI.WebControls.Literal litComercioNombre = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litComercioNombre");

                    #region Traducción de controles
                    /*Controles Repeater Traducción*/
                    System.Web.UI.WebControls.LinkButton btnVerDetalleProducto = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("btnVerDetalleProducto");
                    object ctrbtnVerDetalleProducto = (object)btnVerDetalleProducto;

                    if ((BE.Idioma)Session["Idioma"]!=null)
                    Traducir(((BE.Idioma)Session["Idioma"]),ref ctrbtnVerDetalleProducto);

                    if (btnVerDetalleProducto != null && ctrbtnVerDetalleProducto != null) {
                        btnVerDetalleProducto.Text = ((System.Web.UI.WebControls.LinkButton)ctrbtnVerDetalleProducto).Text;
                    }//[+] VER MÁS

                    System.Web.UI.HtmlControls.HtmlGenericControl pPrecioAnterior = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("pPrecioAnterior");
                    object ctrlpPrecioAnterior = (object)pPrecioAnterior;
                    if ((BE.Idioma)Session["Idioma"] != null)
                        Traducir(((BE.Idioma)Session["Idioma"]), ref ctrlpPrecioAnterior);

                    if (pPrecioAnterior != null && ctrlpPrecioAnterior != null && pPrecioAnterior.InnerText != string.Empty)
                    {
                        pPrecioAnterior.InnerText = ((System.Web.UI.HtmlControls.HtmlGenericControl)ctrlpPrecioAnterior).InnerText;
                    }// Antes: $



                    System.Web.UI.HtmlControls.HtmlGenericControl pPrecioActual = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("pPrecioActual");
                    object ctrlpPrecioActual = (object)pPrecioActual;
                    if ((BE.Idioma)Session["Idioma"] != null)
                        Traducir(((BE.Idioma)Session["Idioma"]), ref ctrlpPrecioActual);

                    if (ctrlpPrecioActual != null && ctrlpPrecioActual != null && pPrecioActual.InnerText != string.Empty)
                    {
                        pPrecioActual.InnerText = ((System.Web.UI.HtmlControls.HtmlGenericControl)pPrecioActual).InnerText;
                    }// Precio final: $
                    #endregion
                    if (image != null)
                    {
                        image.Src = "data:image/png;base64," + Convert.ToBase64String(comercio.LstProducto[0].Foto);
                    }
                    else
                    {
                        image.Src = @"~/Recurso/Imagen/empty.png";
                    }
                    if(imageComercio != null)
                    {
                        imageComercio.Src = "data:image/png;base64," + Convert.ToBase64String(comercio.Logo);
                    }
                    else
                    {
                        imageComercio.Src = @"~/Recurso/Imagen/empty.png";
                    }


                    if (litTitulProducto != null)
                    {
                        if (comercio.LstProducto[0].Titulo.Length > 10)
                            litTitulProducto.Text = comercio.LstProducto[0].Titulo.Substring(0, comercio.LstProducto[0].Titulo.Length - 1).PadRight(3, '.');
                        else
                            litTitulProducto.Text = comercio.LstProducto[0].Titulo;
                    }
                    if (btnVerDetalleProducto != null)
                    {
                        btnVerDetalleProducto.CommandArgument = Convert.ToString(comercio.LstProducto[0].CodProducto);
                        //btnVerDetalleProducto.PostBackUrl = "Ofertas.aspx?codProducto=" + Convert.ToString(comercio.LstProducto[0].CodProducto);
                    }

                    if(litComercioNombre!= null)
                    {
                        litComercioNombre.Text = comercio.NombreComercio;
                    }

                    if (litPorcentajeDescuentoProducto != null)
                    {
                        litPorcentajeDescuentoProducto.Text = Convert.ToString(comercio.LstProducto[0].Descuento);
                    }
                    if(litPrecioAnteriorProducto!=null)
                    {
                        litPrecioAnteriorProducto.Text = Convert.ToString(comercio.LstProducto[0].Precio);
                    }
                    if(litPrecioActualProducto!=null)
                    {
                        litPrecioActualProducto.Text = Convert.ToString(Math.Round(comercio.LstProducto[0].Precio - (comercio.LstProducto[0].Precio * (comercio.LstProducto[0].Descuento / 100)), 2));
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error la mostrar los productos vigentes: ", "Ocurrió un error la mostrar los productos vigentes: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error la mostrar los productos vigentes: ", "Ocurrió un error la mostrar los productos vigentes: ") + ex.Message);
            }
        }

        protected void rptOfertaDelDia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            try
            {
                if (e.Item.DataItem != null)
                {
                    BE.Comercio comercio = (BE.Comercio)e.Item.DataItem;
                    System.Web.UI.HtmlControls.HtmlImage imgDescuentoDelDia = (System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("imgDescuentoDelDia");
                    System.Web.UI.WebControls.Literal litDescuentoDelDiaTitulo = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litDescuentoDelDiaTitulo");
                    System.Web.UI.WebControls.Literal litDescuentoDelDiaPorcentajeDescuento = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litDescuentoDelDiaPorcentajeDescuento");
                    System.Web.UI.WebControls.Literal litDescuentoDelDiaPrecioAnterior = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litDescuentoDelDiaPrecioAnterior");
                    System.Web.UI.WebControls.Literal litDescuentoDelDiaPrecioActual = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litDescuentoDelDiaPrecioActual");
                    System.Web.UI.WebControls.LinkButton btnVerMasOfertaDelDia = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("btnVerMasOfertaDelDia");


                    #region Traducción de controles
                    /*Controles Repeater Traducción*/
                    object ctrbtnVerMasOfertaDelDia = (object)btnVerMasOfertaDelDia;

                    if ((BE.Idioma)Session["Idioma"] != null)
                        Traducir(((BE.Idioma)Session["Idioma"]), ref ctrbtnVerMasOfertaDelDia);

                    if (btnVerMasOfertaDelDia != null && ctrbtnVerMasOfertaDelDia != null)
                    {
                        btnVerMasOfertaDelDia.Text = ((System.Web.UI.WebControls.LinkButton)ctrbtnVerMasOfertaDelDia).Text;
                    }//[+] VER MÁS

                    System.Web.UI.HtmlControls.HtmlGenericControl litPrecioAnterior = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("litPrecioAnterior");
                    object ctrllitPrecioAnterior = (object)litPrecioAnterior;
                    if ((BE.Idioma)Session["Idioma"] != null)
                        Traducir(((BE.Idioma)Session["Idioma"]), ref ctrllitPrecioAnterior);

                    if (litPrecioAnterior != null && ctrllitPrecioAnterior != null && litPrecioAnterior.InnerText != string.Empty)
                    {
                        litPrecioAnterior.InnerText = ((System.Web.UI.HtmlControls.HtmlGenericControl)ctrllitPrecioAnterior).InnerText;
                    }// Antes: $



                    System.Web.UI.HtmlControls.HtmlGenericControl litPrecioFinal = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("litPrecioFinal");
                    object ctrllitPrecioFinal = (object)litPrecioFinal;
                    if ((BE.Idioma)Session["Idioma"] != null)
                        Traducir(((BE.Idioma)Session["Idioma"]), ref ctrllitPrecioFinal);

                    if (ctrllitPrecioFinal != null && ctrllitPrecioFinal != null && litPrecioFinal.InnerText != string.Empty)
                    {
                        litPrecioFinal.InnerText = ((System.Web.UI.HtmlControls.HtmlGenericControl)litPrecioFinal).InnerText;
                    }// Precio final: $
                    #endregion





                    if (imgDescuentoDelDia != null)
                    {
                        imgDescuentoDelDia.Src = "data:image/png;base64," + Convert.ToBase64String(comercio.LstProducto[0].Foto);
                    }
                    else
                    {
                        imgDescuentoDelDia.Src = @"~/Recurso/Imagen/empty.png";
                    }
                    if (litDescuentoDelDiaTitulo != null)
                    {
                        if (comercio.LstProducto[0].Titulo.Length > 10)
                            litDescuentoDelDiaTitulo.Text = comercio.LstProducto[0].Titulo.Substring(0, comercio.LstProducto[0].Titulo.Length - 1).PadRight(3, '.');
                        else
                            litDescuentoDelDiaTitulo.Text = comercio.LstProducto[0].Titulo;
                    }
                    if(litDescuentoDelDiaPorcentajeDescuento != null)
                    {
                        litDescuentoDelDiaPorcentajeDescuento.Text = Convert.ToString(comercio.LstProducto[0].Descuento);
                    }
                    if(litDescuentoDelDiaPrecioAnterior != null)
                    {
                        litDescuentoDelDiaPrecioAnterior.Text = Convert.ToString(comercio.LstProducto[0].Precio);
                    }
                    if(litDescuentoDelDiaPrecioActual != null)
                    {
                        litDescuentoDelDiaPrecioActual.Text = Convert.ToString(Math.Round(comercio.LstProducto[0].Precio - (comercio.LstProducto[0].Precio * (comercio.LstProducto[0].Descuento / 100)), 2));
                    }
                    if (btnVerMasOfertaDelDia != null)
                    {
                        btnVerMasOfertaDelDia.CommandArgument = Convert.ToString(comercio.LstProducto[0].CodProducto);
                        //btnVerMasOfertaDelDia.PostBackUrl = "Ofertas.aspx?codProducto="+ Convert.ToString(comercio.LstProducto[0].CodProducto); 

                    }

                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error la mostrar las ofertas del día: ", "Ocurrió un error la mostrar las ofertas del día: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error la mostrar las ofertas del día: ", "Ocurrió un error la mostrar las ofertas del día: ") + ex.Message);
            }

        }

        private void MostrarMejoresDescuentos()
        {
            try
            {

            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }

        protected void rptOfertaDelDia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if(e.CommandName != string.Empty && e.CommandArgument != null)
                {
                    switch (e.CommandName)
                    {
                        case "VerMas":
                            List<BE.Comercio> lstComercio = (List<BE.Comercio>)Session["OfertasDelDia"];
                            BE.Comercio comercio = null;
                            foreach(BE.Comercio com in lstComercio)
                            {
                                if(com.LstProducto[0].CodProducto == Convert.ToInt32(e.CommandArgument))
                                {
                                    comercio = com;
                                }
                            }
                            CargarOfertaDelDia(comercio);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }

        private void CargarOfertaDelDia(BE.Comercio comercio)
        {
            try
            {
                if (comercio == null)
                    throw new BE.ExcepcionPersonalizada(true, new Exception("No se ha obtenido ningúna oferta, por favor intente nuevamente"));
                if (Session["usuario"] != null)
                {
                    BE.Usuario usuario = (BE.Usuario)Session["Usuario"];
                    Session["Oferta_" + Convert.ToString(usuario.CodUsuario)] = comercio;
                    Response.Redirect("Ofertas.aspx");
                }
                else
                    Response.Redirect("Logueese.aspx");
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }


        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != string.Empty && e.CommandArgument != null)
                {
                    switch (e.CommandName)
                    {
                        case "VerMas":

                            List<BE.Comercio> lstComercio = (List<BE.Comercio>)Session["OfertasVigentes"];
                            BE.Comercio comercio = null;
                            foreach (BE.Comercio com in lstComercio)
                            {
                                if (com.LstProducto[0].CodProducto == Convert.ToInt32(e.CommandArgument))
                                {
                                    comercio = com;
                                }
                            }
                            CargarOfertaDelDia(comercio);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            BE.Categoria cat = null;
            try
            {
                if(ddlCategoria.SelectedValue!=string.Empty)
                {
                    
                    List<BE.Categoria> lstCategoria = (List<BE.Categoria>)Session["Categoria"];
                    if (lstCategoria == null) {
                        BLL.Producto productoNeg = new BLL.Producto();
                        lstCategoria = productoNeg.ListarCategoria();
                        Session["Categoria"] = lstCategoria;
                            }

                    if (lstCategoria!=null && lstCategoria.Count>0)
                    {
                        cat = (from c in lstCategoria where c.CodCategoria == Convert.ToInt32(ddlCategoria.SelectedValue) select c).FirstOrDefault<BE.Categoria>();
                    }
                    if (cat != null)
                        BindSubCategoria(cat);
                }
                
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }

        protected void btnBuscarDescuentos_Click(object sender, EventArgs e)
        {
            try
            {

                BLL.Comercio comercioNeg = new BLL.Comercio();

                BE.Categoria categoria = null;
                BE.SubCategoria subCategoria = null;

                List<BE.Categoria> lstCategoria = (List<BE.Categoria>)Session["Categoria"];
                List < BE.SubCategoria > lstSubCategoria = (List<BE.SubCategoria>)Session["SubCategoria"];
                if (ddlCategoria.SelectedValue != string.Empty)
                    categoria = (from c in lstCategoria where c.CodCategoria == Convert.ToInt32(ddlCategoria.SelectedValue) select c).FirstOrDefault<BE.Categoria>();
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor, seleccione una categoria"));
                if(ddlSubCategoria.SelectedValue != string.Empty)
                subCategoria = (from s in lstSubCategoria where s.CodSubCategoria == Convert.ToInt32(ddlSubCategoria.SelectedValue) select s).FirstOrDefault<BE.SubCategoria>();
                else
                    throw new BE.ExcepcionPersonalizada(true, new Exception("Por favor, seleccione una SubCategoria"));

                if (categoria != null && subCategoria != null)
                {
                    int porcentajeDescuento = 0;
                    if (txtProductoDescuento.Text != string.Empty)
                        porcentajeDescuento = Convert.ToInt32(txtProductoDescuento.Text);

                    List<BE.Comercio> lstComercio = comercioNeg.ListarProductosVigentesPorCategoriaSubCategoriaDescuento(categoria, subCategoria, porcentajeDescuento);
                    if (lstComercio != null)
                    {
                        rptProductos.DataSource = null;
                        rptProductos.DataSource = lstComercio;
                        rptProductos.DataBind();
                    }
                    if (lstComercio != null && lstComercio.Count == 0)
                    {
                        PnResultadoSinDescuentos.Visible = true;
                        
                    }
                    else
                        PnResultadoSinDescuentos.Visible = false;
                    
                    
                }
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }

        protected void btnVerMasOfertas_Click(object sender, EventArgs e)
        {
            try
            {
                PnResultadoSinDescuentos.Visible = false;
                MostrarProductosVigentes();
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }

        protected void btnVerTodo_Click(object sender, EventArgs e)
        {
            try
            {
                PnResultadoSinDescuentos.Visible = false;
                BindCategoria();
                pnSubCategoria.Visible = false;
                txtProductoDescuento.Text = string.Empty;
                MostrarProductosVigentes();
            }
            catch (BE.ExcepcionPersonalizada ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
            catch (Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError(TraducirPalabra("Ocurrió un error: ", "Ocurrió un error: ") + ex.Message);
            }
        }


        private void Traducir(BE.Idioma idioma,ref object ControlATraducir)
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
                    TraducirControles(lstTraduccion,ref ControlATraducir);
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
                            if(ControlATraducir.GetType() == typeof(System.Web.UI.WebControls.LinkButton))
                            {
                                System.Web.UI.WebControls.LinkButton ctrlPage = (System.Web.UI.WebControls.LinkButton)ControlATraducir;
                                if(ctrlPage.ID.ToUpper() == traduccion.ControlID.ToUpper() && traduccion.Texto != string.Empty)
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

        private void ObtenerIdiomaSeleccionado()
        {
            try
            {
                var control = this.Page.Master.FindControl("litDDLIdioma");
                if(control != null) { 
                    BLL.Idioma IdiomaNeg = new BLL.Idioma();
                    //BE.Idioma idioma = new BE.Idioma();
                    //idioma.Descripcion = Convert.ToString();
                    //idioma = IdiomaNeg.Obtener(idioma);
                    //Session["Idioma"] = idioma;
                }
            }
            catch(Exception ex)
            {
                BE.Bitacora exception = new BE.Bitacora(ex, true);
                exceptionLogger.Grabar(exception);
                MostrarError("Ocurrió un error al obtener el idioma seleccionado: " + ex.Message);

            }
        }
    }
}